using UnityEngine;
using System.Collections;

namespace Cave
{
   
    public class Synchronizer : MonoBehaviour
    {
        

        private bool started;
        
        public static NetworkNode node;

        void Start()
        {
            RigidBodySynchronizer.Prepare();
            GUISynchronizer.Prepare();
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

                TimeSynchronizer.BuildMessage(inputMessage.inputTimeMessage);
                InputSynchronizer.BuildMessage(inputMessage.inputInputMessage);
                ParticleSynchronizer.BuildMessage(inputMessage.inputParticleMessage);
                AnimatorSynchronizer.BuildMessage(inputMessage.inputAnimatorMessage);
                TrackingSynchronizer.BuildMessage(inputMessage.inputTrackingMessage);
                RigidBodySynchronizer.BuildMessage(inputMessage.inputRigidBodyMessage);
                TransformationSynchronizer.BuildMessage(inputMessage.inputTransformationMessage);
                EventSynchronizer.BuildMessage(inputMessage.inputEventsMessage);

                node.BroadcastMessage(inputMessage);
            }
            else
            {
                InputMessage inputMessage = new InputMessage();
                ((Client)node).WaitForNextMessage(inputMessage);

                TimeSynchronizer.ProcessMessage(inputMessage.inputTimeMessage);
                InputSynchronizer.ProcessMessage(inputMessage.inputInputMessage);
                ParticleSynchronizer.ProcessMessage(inputMessage.inputParticleMessage);
                AnimatorSynchronizer.ProcessMessage(inputMessage.inputAnimatorMessage);
                TrackingSynchronizer.ProcessMessage(inputMessage.inputTrackingMessage);
                RigidBodySynchronizer.ProcessMessage(inputMessage.inputRigidBodyMessage);
                TransformationSynchronizer.ProcessMessage(inputMessage.inputTransformationMessage);
                EventSynchronizer.ProcessMessage(inputMessage.inputEventsMessage);
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