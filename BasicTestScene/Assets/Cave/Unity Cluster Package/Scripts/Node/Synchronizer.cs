using UnityEngine;
using System.Collections;

namespace UnityClusterPackage
{
   
    public class Synchronizer : MonoBehaviour
    {
        public int targetClientNumber = 1;

        private bool started;
        
        public static NetworkNode node;

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
                node = new Server(targetClientNumber);
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
            ParticleSynchronizer.Synchronize(node);
            node.FinishFrame();
        }

        void OnDestroy()
        {
            node.Disconnect();
        }

    }
}