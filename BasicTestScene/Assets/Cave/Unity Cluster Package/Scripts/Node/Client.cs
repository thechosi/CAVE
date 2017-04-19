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
    class Client : NetworkNode
    {
        
        public override void Connect()
        {
            connections.Add(AweSock.TcpConnect(NodeInformation.serverIp, 8888));
            InitializeSelf();
        }


        void InitializeSelf()
        {
            ParticleSystem[] particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                SynchroMessage message = WaitForNextMessage(connections[0]);
                particleSystem.Stop();
                particleSystem.randomSeed = (uint)message.data;
                particleSystem.Clear();
                particleSystem.Play();
            }
        }


        public override void FinishFrame()
        {
            BroadcastMessage(new SynchroMessage(SynchroMessageType.FinishedRendering, 0));
        }


        public SynchroMessage WaitForNextMessage()
        {
            return WaitForNextMessage(connections[0]);
        }

    }
}
