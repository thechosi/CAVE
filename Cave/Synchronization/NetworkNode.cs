using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace Cave
{
    public abstract class NetworkNode
    {
        public List<ISocket> connections;
        Buffer inBuf;
        Buffer outBuf;

        public NetworkNode()
        {
            inBuf = Buffer.New(64);
            outBuf = Buffer.New(64);

            connections = new List<ISocket>();
        }

        public void ReceiveNextMessage(bool skipConnectingEvents, ISocket connection, ISynchroMessage targetMessage)
        {
            try
            {
                Buffer.Resize(inBuf, sizeof(int));
                AweSock.ReceiveMessage(connection, inBuf);
                int messageLength = Buffer.Get<int>(inBuf);

                Buffer.Resize(inBuf, messageLength);
                int received = 0;

                do
                {
                    received += AweSock.ReceiveMessage(connection, inBuf, received);
                    if (received == 0)
                        throw new SocketException();
                } while (received < messageLength);

                targetMessage.Deserialize(inBuf);
            }
            catch (SocketException)
            {
                connections.Remove(connection);
            }
        }

        public void WaitForNextMessage(ISocket connection, ISynchroMessage targetMessage)
        {
            if (connections.Count > 0)
            {
                ReceiveNextMessage(true, connection, targetMessage);
            }
        }

        public abstract void Connect();

        public abstract void FinishFrame();

        public void BroadcastMessage(ISynchroMessage message)
        {
            Buffer.Resize(outBuf, message.GetLength() + sizeof(int));
            Buffer.Add(outBuf, message.GetLength());
            message.Serialize(outBuf);
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

        public void SendMessage(ISynchroMessage message, ISocket connection)
        {
            Buffer.Resize(outBuf, message.GetLength() + sizeof(int));
            Buffer.Add(outBuf, message.GetLength());
            message.Serialize(outBuf);
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
    }
}
