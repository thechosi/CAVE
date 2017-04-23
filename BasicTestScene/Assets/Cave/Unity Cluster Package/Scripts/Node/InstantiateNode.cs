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
                
                // The slave needs to connect to the MainCamera
                if (cameraSlave == null)
                {
                    cameraSlave = GameObject.FindWithTag("MainCamera");

                    Debug.Log(NodeInformation.cameraRoation);
                    cameraSlave.transform.Rotate(NodeInformation.cameraRoation);
                }
            }

        }


        void Update()
        {
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