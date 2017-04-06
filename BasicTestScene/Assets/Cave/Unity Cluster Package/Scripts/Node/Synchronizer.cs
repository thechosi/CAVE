using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace UnityClusterPackage
{
    public enum SynchroMessageType
    {
        SetDeltaTime,
        SetTime,
        FinishedRendering
    }

    public class SynchroMessage : MessageBase
    {
        public SynchroMessageType type;
        public float data;

        public SynchroMessage()
        {

        }

        public SynchroMessage(SynchroMessageType type, float data)
        {
            this.type = type;
            this.data = data;
        }
    }

    public class Synchronizer : MonoBehaviour
    {
        public static float deltaTime;
        public static float time;
        public bool enableTimeout = false;
        private int reliableChannelId;
        private int hostId;
        private List<int> connections;
        void Start()
        {
            // Initializing the Transport Layer with no arguments (default settings)
            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelId = config.AddChannel(QosType.ReliableSequenced);
            HostTopology topology = new HostTopology(config, 10);
            hostId = NetworkTransport.AddHost(topology, 8888 + (NodeInformation.type.Equals("slave") ? 1 : 0));

            connections = new List<int>();

            if (NodeInformation.type.Equals("slave"))
            {
                byte error;
                int connectionId = NetworkTransport.Connect(hostId, NodeInformation.serverIp, 8888, 0, out error);
            }
        }


        SynchroMessage ReceiveNextMessage(bool skipConnectingEvents)
        {
            SynchroMessage message = null;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;
            NetworkEventType recData = NetworkTransport.ReceiveFromHost(hostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    connections.Add(connectionId);
                    if (skipConnectingEvents)
                        message = ReceiveNextMessage(skipConnectingEvents);
                    break;
                case NetworkEventType.DataEvent:
                    NetworkReader networkReader = new NetworkReader(recBuffer);
                    message = new SynchroMessage();
                    message.Deserialize(networkReader);
                    //Debug.Log(message.type.ToString() + ": " + message.data);
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
                var watch = System.Diagnostics.Stopwatch.StartNew();
                SynchroMessage message;
                do
                {
                    message = ReceiveNextMessage(true);
                } while (message == null && (!enableTimeout || watch.ElapsedMilliseconds < 1000));
                if (message == null)
                {
                    throw new TimeoutException("Waiting unsuccessfully for the next message which ");
                }
                return message;
            }
            else
            {
                return null;
            }
        }

        void WaitForConnections(int targetNumber)
        {
            while (connections.Count < targetNumber)
            {
                ReceiveNextMessage(false);
            }
        }

        void BroadcastMessage(SynchroMessage message)
        {
            byte error;
            NetworkWriter networkWriter = new NetworkWriter();
            message.Serialize(networkWriter);

            foreach (int connectionId in connections)
                NetworkTransport.Send(hostId, connectionId, reliableChannelId, networkWriter.AsArray(), networkWriter.Position, out error);
        }

        void Update()
        {
            if (NodeInformation.type.Equals("master"))
            {
                WaitForConnections(1);
                BroadcastMessage(new SynchroMessage(SynchroMessageType.SetDeltaTime, Time.deltaTime));
                BroadcastMessage(new SynchroMessage(SynchroMessageType.SetTime, Time.time));
                deltaTime = Time.deltaTime;
                time = Time.time;
            }
            else
            {
                WaitForConnections(1);
                deltaTime = -1;
                time = -1;
                do
                {
                    SynchroMessage message = WaitForNextMessage();

                    if (message.type == SynchroMessageType.SetDeltaTime)
                        deltaTime = message.data;
                    else if (message.type == SynchroMessageType.SetTime)
                        time = message.data;
                    else
                        throw new Exception("Received unexpected message.");

                } while (time == -1 || deltaTime == -1);
            }
        }

        void OnPostRender()
        {
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
    }
}