using System;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;

namespace UnityClusterPackage
{ 
    class Server : NetworkNode
    {
        public int targetClientNumber = 1;
        public int maxSecondsToWaitForConnections = 10;

        private ISocket listenSocket;

        public Server(int targetClientNumber, int maxSecondsToWaitForConnections)
        {
            this.targetClientNumber = targetClientNumber;
            this.maxSecondsToWaitForConnections = maxSecondsToWaitForConnections;
        }

        public override void Connect()
        {
            listenSocket = AweSock.TcpListen(NodeInformation.serverPort + 1);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (connections.Count < targetClientNumber && watch.ElapsedMilliseconds < 1000 * maxSecondsToWaitForConnections)
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


        void InitializeClient(ISocket connection)
        {
           ParticleSynchronizer.InitializeFromServer(this, connection);
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
                SynchroMessage message = WaitForNextMessage(connections[counter]);

                if (message.type == SynchroMessageType.FinishedRendering)
                    counter++;
                else
                    throw new Exception("Received unexpected message.");
            }
        }

    }
}
