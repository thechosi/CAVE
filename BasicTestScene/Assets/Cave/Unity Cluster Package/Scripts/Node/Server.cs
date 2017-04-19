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
            listenSocket = AweSock.TcpListen(8888 + (NodeInformation.type.Equals("slave") ? 1 : 0));
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
            ParticleSystem[] particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                SendMessage(new SynchroMessage(SynchroMessageType.SetParticleSeed, particleSystem.randomSeed), connection);
                particleSystem.Stop();
                particleSystem.useAutoRandomSeed = false;
                particleSystem.Clear();
                particleSystem.Play();
            }
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
