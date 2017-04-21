using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

namespace UnityClusterPackage
{

    public class InstantiateNode : MonoBehaviour
    {

        public GameObject CameraHolder;
        public GameObject cameraSlave;

        void Start()
        {
            NetworkManager networkManager = GetComponent<NetworkManager>();
            networkManager.networkAddress = NodeInformation.serverIp;
            networkManager.networkPort = NodeInformation.serverPort;
            if (NodeInformation.type.Equals("master"))
            {
                networkManager.StartServer();

                GameObject obj = (GameObject)Instantiate(CameraHolder, transform.position, transform.rotation);
                NetworkServer.Spawn(obj);
            }
            else if (NodeInformation.type.Equals("slave"))
            {
                networkManager.StartClient();

                // Disable rigidbodies for slaves
                Rigidbody[] rigidbodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
                foreach (Rigidbody rigidbody in rigidbodies)
                {
                    rigidbody.useGravity = false;
                }
                // The slave needs to connect to the MainCamera
                if (cameraSlave == null)
                {
                    cameraSlave = GameObject.FindWithTag("MainCamera");

                    Debug.Log(NodeInformation.cameraRoation);
                    cameraSlave.transform.Rotate(NodeInformation.cameraRoation);
                }
            }

            // Configure all network transform components (we will synchronize manually)
            NetworkTransform[] networkTransforms = FindObjectsOfType(typeof(NetworkTransform)) as NetworkTransform[];
            foreach (NetworkTransform networkTransform in networkTransforms)
            {
                networkTransform.interpolateMovement = 0;
                networkTransform.sendInterval = 0;
                networkTransform.movementTheshold = 0.000001f;
            }
        }


        void Update()
        {
            // Manually start synchronization for all network tranforms
            NetworkTransform[] networkTransforms = FindObjectsOfType(typeof(NetworkTransform)) as NetworkTransform[];
            foreach (NetworkTransform networkTransform in networkTransforms)
            {
                networkTransform.SetDirtyBit(1);
            }

            // Workaround
            // It may take to long to connect to Server
            if (cameraSlave == null)
            {
                cameraSlave = GameObject.FindWithTag("MainCamera");
                Debug.Log(cameraSlave);
                if (cameraSlave != null)
                {
                    Debug.Log(NodeInformation.cameraRoation);
                    cameraSlave.transform.Rotate(NodeInformation.cameraRoation);
                }
            }
        }
    }
}