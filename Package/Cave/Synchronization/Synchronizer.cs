using UnityEngine;
using System.Collections;

namespace Cave
{
   
    public class Synchronizer : MonoBehaviour
    {
        public string[] relevantAxes = { "Vertical", "Horizontal" };
        public string[] relevantButtons = { };
        public string[] relevantKeys = { "mouse 0", "mouse 1", "mouse 2", "flystick 0", "flystick 1", "flystick 2", "flystick 3" };
        public GameObject flyStick = null;

        private bool started;
        
        public static NetworkNode node;

        void Start()
        {
            flyStick = GameObject.Find("Flystick");
            GUISynchronizer.Prepare();
            started = false;
            
            if (!NodeInformation.isMaster())
            {
                QualitySettings.vSyncCount = 0;
                node = new Client();
            }
            else
            {
                node = new Server(NodeInformation.developmentMode ? 0 : NodeInformation.slaves.Count);
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
                if (NodeInformation.isMaster() && node.connections.Count != ((Server)node).targetClientNumber || !NodeInformation.isMaster() && node.connections.Count == 0)
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

            if (NodeInformation.isMaster())
            {
                InputMessage inputMessage = new InputMessage();

                TimeSynchronizer.BuildMessage(inputMessage.inputTimeMessage);
                InputSynchronizer.BuildMessage(this, inputMessage.inputInputMessage);
                ParticleSynchronizer.BuildMessage(inputMessage.inputParticleMessage);
                AnimatorSynchronizer.BuildMessage(inputMessage.inputAnimatorMessage);
                TrackingSynchronizer.BuildMessage(inputMessage.inputTrackingMessage);
                TransformationSynchronizer.BuildMessage(inputMessage.inputTransformationMessage);
                EventSynchronizer.BuildMessage(inputMessage.inputEventsMessage);

                node.BroadcastMessage(inputMessage);
            }
            else
            {
                InputMessage inputMessage = new InputMessage();
                ((Client)node).WaitForNextMessage(inputMessage);

                TimeSynchronizer.ProcessMessage(inputMessage.inputTimeMessage);
                InputSynchronizer.ProcessMessage(this, inputMessage.inputInputMessage);
                ParticleSynchronizer.ProcessMessage(inputMessage.inputParticleMessage);
                AnimatorSynchronizer.ProcessMessage(inputMessage.inputAnimatorMessage);
                TrackingSynchronizer.ProcessMessage(inputMessage.inputTrackingMessage);
                TransformationSynchronizer.ProcessMessage(inputMessage.inputTransformationMessage);
                EventSynchronizer.ProcessMessage(inputMessage.inputEventsMessage);
            }
            
            StartCoroutine(EndOfFrame());
        }

        IEnumerator EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            node.FinishFrame();
        }

        void OnDestroy()
        {
            node.Disconnect();
        }

    }
}