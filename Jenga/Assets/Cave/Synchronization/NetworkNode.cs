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
            inBuf = Buffer.New(4 + 8 + 4 * 100);
            outBuf = Buffer.New(4 + 8 + 4 * 100);

            connections = new List<ISocket>();
        }


        public void ReceiveNextMessage(bool skipConnectingEvents, ISocket connection, ISynchroMessage targetMessage)
        {
            try
            {
                inBuf = Buffer.New(targetMessage.GetLength());
                int received = AweSock.ReceiveMessage(connection, inBuf);
                if (received == 0)
                    throw new SocketException();
                targetMessage.Deserialize(inBuf);
            }
            catch (SocketException e)
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
            outBuf = Buffer.New(message.GetLength());
            Buffer.ClearBuffer(outBuf);
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
            outBuf = Buffer.New(message.GetLength());
            Buffer.ClearBuffer(outBuf);
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
