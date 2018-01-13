using System;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;

namespace Cave
{
    public class Server : NetworkNode
    {
        public int targetClientNumber = 1;

        private ISocket listenSocket;

        public Server(int targetClientNumber)
        {
            this.targetClientNumber = targetClientNumber;
        }

        public override void Connect()
        {
            listenSocket = AweSock.TcpListen(NodeInformation.own.port + 1);
            while (connections.Count < targetClientNumber)
            {
                connections.Add(AweSock.TcpAccept(listenSocket));
                InitializeClient(connections[connections.Count - 1]);
            }
        }

        void InitializeClient(ISocket connection)
        {
           	ParticleSynchronizer.InitializeFromServer(this, connection);
			RandomSynchronizer.InitializeFromServer(this, connection);
        }

        public override void Disconnect()
        {
            base.Disconnect();
            listenSocket.Close();
        }

        public override void FinishFrame()
        {
            int counter = 0;
            while (counter < connections.Count)
            {
                FinishMessage message = new FinishMessage();
                WaitForNextMessage(connections[counter], message);

                if (message.finished)
                    counter++;
                else
                    throw new Exception("Received unexpected message.");
            }
        }

    }
}
