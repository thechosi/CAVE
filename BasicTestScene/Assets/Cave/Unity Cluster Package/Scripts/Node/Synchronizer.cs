using UnityEngine;
using System.Collections;

namespace UnityClusterPackage
{
   
    public class Synchronizer : MonoBehaviour
    {
        

        private bool started;
        
        public static NetworkNode node;

        void Start()
        {
            RigidBodySynchronizer.Prepare();
            started = false;
            
            if (NodeInformation.type.Equals("slave"))
            {
                QualitySettings.vSyncCount = 0;
                node = new Client();
            }
            else
            {
                node = new Server(NodeInformation.numberOfSlaves);
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
                if (NodeInformation.type.Equals("master") && node.connections.Count != NodeInformation.numberOfSlaves || NodeInformation.type.Equals("slave") && node.connections.Count == 0)
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

            if (NodeInformation.type.Equals("master"))
            {
                InputMessage inputMessage = new InputMessage();

                TimeSynchronizer.BuildMessage(inputMessage);
                InputSynchronizer.BuildMessage(inputMessage);
                ParticleSynchronizer.BuildMessage(inputMessage);
                AnimatorSynchronizer.BuildMessage(inputMessage);

                node.BroadcastMessage(inputMessage);
            }
            else
            {
                InputMessage inputMessage = new InputMessage();
                ((Client)node).WaitForNextMessage(inputMessage);

                TimeSynchronizer.ProcessMessage(inputMessage);
                InputSynchronizer.ProcessMessage(inputMessage);
                ParticleSynchronizer.ProcessMessage(inputMessage);
                AnimatorSynchronizer.ProcessMessage(inputMessage);
            }
            
            StartCoroutine(EndOfFrame());
        }

        IEnumerator EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            //ParticleSynchronizer.Synchronize(node);
            node.FinishFrame();
        }

        void OnDestroy()
        {
            node.Disconnect();
        }

    }
}