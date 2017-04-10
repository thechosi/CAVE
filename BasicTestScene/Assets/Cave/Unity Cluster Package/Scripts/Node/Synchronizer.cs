using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine.Networking;

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

    public class SynchroMessage : MessageBase
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
        
        private int reliableChannelId;
        private int hostId;
        private List<int> connections;
        private bool doClientInitialization;
        private bool started;
        private static float axisHorizontal;
        private static float axisVertical;

        byte[] recBuffer;
        NetworkReader networkReader;
        

        void Start()
        {
            recBuffer = new byte[1024];
            networkReader = new NetworkReader(recBuffer);
            started = false;
            GlobalConfig gConfig = new GlobalConfig();
            NetworkTransport.Init(gConfig);

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
            HostTopology topology = new HostTopology(config, 10);
            hostId = NetworkTransport.AddHost(topology, 8888 + (NodeInformation.type.Equals("slave") ? 1 : 0));

            connections = new List<int>();

            if (NodeInformation.type.Equals("slave"))
            {
                QualitySettings.vSyncCount = 0;
                byte error;
                int connectionId = NetworkTransport.Connect(hostId, NodeInformation.serverIp, 8888, 0, out error);
            }
        }

        void InitializeClient(int connectionId)
        {
            ParticleSystem[] particleSystems = FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                SendMessage(new SynchroMessage(SynchroMessageType.SetParticleSeed, particleSystem.randomSeed), connectionId);
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
                SynchroMessage message = WaitForNextMessage();
                particleSystem.Stop();
                particleSystem.randomSeed = (uint)message.data;
                particleSystem.Clear();
                particleSystem.Play();
            }
            doClientInitialization = false;
        }

        SynchroMessage ReceiveNextMessage(bool skipConnectingEvents)
        {
            SynchroMessage message = null;
            int connectionId;
            int channelId;
            int bufferSize = 1024;
            int dataSize;
            byte error;
            NetworkEventType recData = NetworkTransport.ReceiveFromHost(hostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            if (error > 0)
                throw new NetworkInformationException(error);
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    connections.Add(connectionId);
                    if (NodeInformation.type.Equals("master"))
                        InitializeClient(connectionId);
                    else
                        doClientInitialization = true;

                    if (skipConnectingEvents)
                        message = ReceiveNextMessage(skipConnectingEvents);
                    break;
                case NetworkEventType.DataEvent:
                    message = new SynchroMessage();
                    networkReader.SeekZero();
                    message.Deserialize(networkReader);
                    break;
                case NetworkEventType.DisconnectEvent:
                    connections.Remove(connectionId);
                    if (skipConnectingEvents)
                        message = ReceiveNextMessage(skipConnectingEvents);
                    break;
            }
            return message;
        }

        SynchroMessage WaitForNextMessage()
        {
            if (connections.Count > 0)
            {
                SynchroMessage message;
                do
                {
                    if (connections.Count == 0)
                        return null;
                    message = ReceiveNextMessage(true);
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
                ReceiveNextMessage(false);
            }
            if (watch.ElapsedMilliseconds >= 1000 * maxSecondsToWaitForConnections)
            {
                Debug.Log("Waiting unsuccessfully for the next connection.");
                Application.Quit();
            }
        }

        void BroadcastMessage(SynchroMessage message)
        {
            byte error;
            NetworkWriter networkWriter = new NetworkWriter();
            message.Serialize(networkWriter);

            foreach (int connectionId in connections)
            {
                NetworkTransport.Send(hostId, connectionId, reliableChannelId, networkWriter.AsArray(), networkWriter.Position, out error);
                if (error > 0)
                    throw new NetworkInformationException(error);
            }
        }

        void SendMessage(SynchroMessage message, int connectionId)
        {
            byte error;
            NetworkWriter networkWriter = new NetworkWriter();
            message.Serialize(networkWriter);

            NetworkTransport.Send(hostId, connectionId, reliableChannelId, networkWriter.AsArray(), networkWriter.Position, out error);
        }

        void CheckConnection()
        {
            if (!started)
            {
                WaitForConnections(NodeInformation.type.Equals("master") ? targetClientNumber : 1);
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
            if (doClientInitialization)
                InitializeSelf();

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
                    SynchroMessage message = WaitForNextMessage();

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
                    SynchroMessage message = WaitForNextMessage();

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

        private float lasttime;
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
                    SynchroMessage message = WaitForNextMessage();

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
            byte error;
            foreach (int connection in connections)
            {
                NetworkTransport.Disconnect(hostId, connection, out error);
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