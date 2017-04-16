using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using AwesomeSockets.Domain;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using UnityEngine.Networking;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace UnityClusterPackage
{
    public enum SynchroMessageType
    {
        SetDeltaTime,
        SetTime,
        FinishedRendering,
        SetParticleSeed,
        SetHorizontalAxis,
        SetVerticalAxis
    }

    public class SynchroMessage : UnityEngine.Networking.MessageBase
    {
        public SynchroMessageType type;
        public double data;

        public SynchroMessage()
        {

        }

        public SynchroMessage(SynchroMessageType type, double data)
        {
            this.type = type;
            this.data = data;
        }
    }

    public class Synchronizer : MonoBehaviour
    {
        public static float deltaTime;
        public static float time;
        public int targetClientNumber = 1;
        public int maxSecondsToWaitForConnections = 10;

        private ISocket listenSocket;
        private List<ISocket> connections;
        private bool started;
        private static float axisHorizontal;
        private static float axisVertical;

        private int measureTime = 0;
        Buffer inBuf;
        Buffer outBuf;

        void Start()
        {
            inBuf = Buffer.New();
            outBuf = Buffer.New();

            started = false;

            listenSocket = AweSock.TcpListen(8888 + (NodeInformation.type.Equals("slave") ? 1 : 0));

            connections = new List<ISocket>();

            if (NodeInformation.type.Equals("slave"))
            {
                QualitySettings.vSyncCount = 0;
            }
        }

        void InitializeClient(ISocket connection)
        {
            ParticleSystem[] particleSystems = FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                SendMessage(new SynchroMessage(SynchroMessageType.SetParticleSeed, particleSystem.randomSeed), connection);
                particleSystem.Stop();
                particleSystem.useAutoRandomSeed = false;
                particleSystem.Clear();
                particleSystem.Play();
            }
        }

        void InitializeSelf()
        {
            ParticleSystem[] particleSystems = FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                SynchroMessage message = WaitForNextMessage(connections[0]);
                particleSystem.Stop();
                particleSystem.randomSeed = (uint)message.data;
                particleSystem.Clear();
                particleSystem.Play();
            }
        }

        SynchroMessage ReceiveNextMessage(bool skipConnectingEvents, ISocket connection)
        {
            try
            {
                int received = AweSock.ReceiveMessage(connection, inBuf);
                if (received == 0)
                    throw new SocketException();
                return new SynchroMessage((SynchroMessageType)Buffer.Get<int>(inBuf), Buffer.Get<double>(inBuf));
            }
            catch (SocketException e)
            {
                connections.Remove(connection);
                return null;
            }
        }

        SynchroMessage WaitForNextMessage(ISocket connection)
        {
            if (connections.Count > 0)
            {
                SynchroMessage message;
                do
                {
                    if (connections.Count == 0)
                        return null;
                    message = ReceiveNextMessage(true, connection);
                } while (message == null || connections.Count == 0);
                return message;
            }
            else
            {
                return null;
            }
        }

        void WaitForConnections(int targetNumber)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (connections.Count < targetNumber && watch.ElapsedMilliseconds < 1000 * maxSecondsToWaitForConnections)
            {
                connections.Add(AweSock.TcpAccept(listenSocket));
                InitializeClient(connections[connections.Count - 1]);
            }
            if (watch.ElapsedMilliseconds >= 1000 * maxSecondsToWaitForConnections)
            {
                Debug.Log("Waiting unsuccessfully for the next connection.");
                Application.Quit();
            }
        }

        void BroadcastMessage(SynchroMessage message)
        {
            Buffer.ClearBuffer(outBuf);
            Buffer.Add(outBuf, (int)message.type);
            Buffer.Add(outBuf, message.data);
            Buffer.FinalizeBuffer(outBuf);

            foreach (ISocket connection in connections)
            {
                try
                {
                    int bytesSent = AweSock.SendMessage(connection, outBuf);

                    if (bytesSent == 0)
                        throw new NetworkInformationException();
                }
                catch (SocketException)
                {
                    connections.Remove(connection);
                }
            }
        }

        void SendMessage(SynchroMessage message, ISocket connection)
        {
            Buffer.ClearBuffer(outBuf);
            Buffer.Add(outBuf, (int)message.type);
            Buffer.Add(outBuf, message.data);
            Buffer.FinalizeBuffer(outBuf);

            try
            {
                int bytesSent = AweSock.SendMessage(connection, outBuf);

                if (bytesSent == 0)
                    throw new NetworkInformationException();
            }
            catch (SocketException)
            {
                connections.Remove(connection);
            }
        }

        void CheckConnection()
        {
            if (!started)
            {
                if (NodeInformation.type.Equals("master"))
                    WaitForConnections(targetClientNumber);
                else
                {
                    connections.Add(AweSock.TcpConnect(NodeInformation.serverIp, 8888));
                    InitializeSelf();
                }
                started = true;
            }
            else
            {
                if (connections.Count == 0)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                }
            }

        }

        void SynchronizeTime()
        {
            if (NodeInformation.type.Equals("master"))
            {
                BroadcastMessage(new SynchroMessage(SynchroMessageType.SetDeltaTime, Time.deltaTime));
                BroadcastMessage(new SynchroMessage(SynchroMessageType.SetTime, Time.time));
                deltaTime = Time.deltaTime;
                time = Time.time;

                NetworkTransform[] networkTransforms = FindObjectsOfType(typeof(NetworkTransform)) as NetworkTransform[];
                foreach (NetworkTransform networkTransform in networkTransforms)
                {
                    networkTransform.SetDirtyBit(1);
                }
            }
            else
            {
                deltaTime = -1;
                time = -1;
                do
                {
                    SynchroMessage message = WaitForNextMessage(connections[0]);

                    if (message.type == SynchroMessageType.SetDeltaTime)
                        deltaTime = (float)message.data;
                    else if (message.type == SynchroMessageType.SetTime)
                        time = (float)message.data;
                    else
                        throw new Exception("Received unexpected message.");

                } while (time == -1 || deltaTime == -1);

                NetworkTransform[] networkTransforms = FindObjectsOfType(typeof(NetworkTransform)) as NetworkTransform[];
                foreach (NetworkTransform networkTransform in networkTransforms)
                {
                    Rigidbody rigidBody = networkTransform.GetComponentInParent<Rigidbody>();
                    if (rigidBody != null)
                        rigidBody.useGravity = false;
                }
            }
        }


        void SynchronizeInput()
        {
            if (NodeInformation.type.Equals("master"))
            {
                BroadcastMessage(new SynchroMessage(SynchroMessageType.SetHorizontalAxis, Input.GetAxis("Horizontal")));
                BroadcastMessage(new SynchroMessage(SynchroMessageType.SetVerticalAxis, Input.GetAxis("Vertical")));
                axisHorizontal = Input.GetAxis("Horizontal");
                axisVertical = Input.GetAxis("Vertical");
            }
            else
            {
                axisHorizontal = -2;
                axisVertical = -2;
                do
                {
                    SynchroMessage message = WaitForNextMessage(connections[0]);

                    if (message.type == SynchroMessageType.SetHorizontalAxis)
                        axisHorizontal = (float)message.data;
                    else if (message.type == SynchroMessageType.SetVerticalAxis)
                        axisVertical = (float)message.data;
                    else
                        throw new Exception("Received unexpected message.");

                } while (axisHorizontal == -2 || axisVertical == -2);
            }
        }

        void Update()
        {
            CheckConnection();

            SynchronizeTime();
            SynchronizeInput();

            StartCoroutine(EndOfFrame());
        }

        private void SynchronizeParticles()
        {
            ParticleSystem[] particleSystems = FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.time -= Time.deltaTime;
                particleSystem.time += deltaTime;
            }
        }

        IEnumerator EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            SynchronizeParticles();

            if (NodeInformation.type.Equals("master"))
            {
                int counter = 0;
                while (counter < connections.Count)
                {
                    SynchroMessage message = WaitForNextMessage(connections[counter]);

                    if (message.type == SynchroMessageType.FinishedRendering)
                        counter++;
                    else
                        throw new Exception("Received unexpected message.");
                }
            }
            else
            {
                BroadcastMessage(new SynchroMessage(SynchroMessageType.FinishedRendering, 0));
            }
        }

        void OnDestroy()
        {
            foreach (ISocket connection in connections)
            {
                connection.Close();
            }
        }

        public static float GetAxis(string orientation)
        {
            if (orientation == "Vertical")
                return axisVertical;
            else
                return axisHorizontal;
        }
    }
}