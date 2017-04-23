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
    public class Client : NetworkNode
    {
        
        public override void Connect()
        {
            connections.Add(AweSock.TcpConnect(NodeInformation.serverIp, 8888));
            InitializeSelf();
        }
        
        void InitializeSelf()
        {
           ParticleSynchronizer.InitializeFromClient(this);
           RigidBodySynchronizer.InitializeFromClient(this);
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
