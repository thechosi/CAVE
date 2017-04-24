using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace UnityClusterPackage
{
    public abstract class NetworkNode
    {
        protected List<ISocket> connections;
        Buffer inBuf;
        Buffer outBuf;

        public NetworkNode()
        {
            inBuf = Buffer.New();
            outBuf = Buffer.New();

            connections = new List<ISocket>();
        }


        public SynchroMessage ReceiveNextMessage(bool skipConnectingEvents, ISocket connection)
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

        public SynchroMessage WaitForNextMessage(ISocket connection)
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

        public abstract void Connect();

        public abstract void FinishFrame();

        public void BroadcastMessage(SynchroMessage message)
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

        public void SendMessage(SynchroMessage message, ISocket connection)
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


        public virtual void Disconnect()
        {
            foreach (ISocket connection in connections)
            {
                connection.Close();
            }
        }

        public bool LostConnection()
        {
            return connections.Count == 0;
        }
    }
}
