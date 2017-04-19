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
   
    public class Synchronizer : MonoBehaviour
    {
        public int targetClientNumber = 1;
        public int maxSecondsToWaitForConnections = 10;

        private bool started;
        
        private NetworkNode node;

        void Start()
        {
            started = false;
            
            if (NodeInformation.type.Equals("slave"))
            {
                QualitySettings.vSyncCount = 0;
                node = new Client();
            }
            else
            {
                node = new Server(targetClientNumber, maxSecondsToWaitForConnections);
            }
        }


        void CheckConnection()
        {
            if (!started)
            {
                node.Connect();
                started = true;
            }
            else
            {
                if (node.LostConnection())
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                }
            }

        }

        void Update()
        {
            CheckConnection();

            TimeSynchronizer.Synchronize(node);
            InputSynchronizer.Synchronize(node);

            StartCoroutine(EndOfFrame());
        }

        IEnumerator EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            ParticleSynchronizer.Synchronize();
            node.FinishFrame();
        }

    }
}