using System;
using System.Threading;
using AwesomeSockets.Sockets;
using UnityEngine;
using UnityEngine.UI;

namespace Cave
{
    public class Client : NetworkNode
    {

        public override void Connect()
        {
            int tryCounter = 0;
            nextTry:
            try
            {
                connections.Add(AweSock.TcpConnect(NodeInformation.master.ip, NodeInformation.master.port + 1));
            }
            catch (Exception)
            {
                Debug.Log("Could not connect to server. Trying again.");

                if (++tryCounter >= 10)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    return;
                }

                Thread.Sleep(500);
                goto nextTry;
            }
            InitializeSelf();
        }

        void InitializeSelf()
        {
            ParticleSynchronizer.InitializeFromClient(this);
			RandomSynchronizer.InitializeFromClient(this);
        }

        public override void FinishFrame()
        {
            BroadcastMessage(new FinishMessage());
        }

        public void WaitForNextMessage(ISynchroMessage targetMessage)
        {
            WaitForNextMessage(connections[0], targetMessage);
        }

    }
}
