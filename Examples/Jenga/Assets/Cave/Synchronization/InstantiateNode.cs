using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

namespace Cave
{

    public class InstantiateNode : MonoBehaviour
    {
        
        public GameObject origin;
        private GameObject originSlave;
        private TrackerSettings trackerSettings;
        public Transform originParent;

        void Start()
        {
            NetworkManager networkManager = GetComponent<NetworkManager>();
            networkManager.networkAddress = NodeInformation.serverIp;
            networkManager.networkPort = NodeInformation.serverPort;
            if (NodeInformation.type.Equals("master"))
            {
                networkManager.StartServer();
                //Instantiate the origin-GameObject
                GameObject objOrigin = Instantiate(origin, originParent);
                objOrigin.transform.localPosition = NodeInformation.originPosition;
                //Spawn the object for the clients
                NetworkServer.Spawn(objOrigin);

                //Test Master; if we remove this, then the master won't see anything (same goes for config)
                originSlave = FindOrigin();

                Transform ScreenplaneTransform = originSlave.transform.Find("Plane");
                ScreenplaneTransform.localScale = NodeInformation.screenplaneScale;
                //ScreenplaneTransform.eulerAngles = NodeInformation.screenplaneRotation;
                ScreenplaneTransform.localPosition = NodeInformation.screenplanePosition;


                trackerSettings = (TrackerSettings) originSlave.transform.Find("CameraHolder").GetComponent("TrackerSettings");
                trackerSettings.HostSettings = (TrackerHostSettings) GameObject.Find("NodeManager/TrackerHostSettings").GetComponent("TrackerHostSettings");

                trackerSettings = (TrackerSettings)originSlave.transform.Find("Flystick").GetComponent("TrackerSettings");
                trackerSettings.HostSettings = (TrackerHostSettings)GameObject.Find("NodeManager/TrackerHostSettings").GetComponent("TrackerHostSettings");

            }
            else if (NodeInformation.type.Equals("slave"))
            {
                networkManager.StartClient();
                //The camera manipulation happens in Update()

				// deactivate music and sound for clients
				AudioListener.volume = 0f;

            }

        }

        public static GameObject FindOrigin()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("MainCamera");
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.transform.Find("CameraHolder") != null)
                    return gameObject;
            }
            return null;
        }

        public void Update()
        {
            //The slave needs to turn the camera and adjust the screenplane
            if (NodeInformation.type.Equals("slave"))
            {
                //Only do it once!
                if (originSlave == null)
                {
                    //the origin-GameObject has the tag "MainCamera" to find it easily
                    originSlave = FindOrigin();
                    //Since the client needs some time to get the object from the server, we can only continue if we have the origin
                    if (originSlave != null)
                    {
                        originSlave.transform.parent = originParent;
                        //The plane is a child from origin
                        Transform ScreenplaneTransform = originSlave.transform.Find("Plane");
                        //Adjust the scale
                        ScreenplaneTransform.localScale = NodeInformation.screenplaneScale;
                        //Adjust the rotation
                        ScreenplaneTransform.eulerAngles = NodeInformation.screenplaneRotation;
                        //Adjust the position
                        ScreenplaneTransform.localPosition = NodeInformation.screenplanePosition;

                        //Find the camera to turn it to the right direction
                        Transform CameraTransform = originSlave.transform.Find("CameraHolder/Camera");
                        CameraTransform.eulerAngles = NodeInformation.cameraRoation;

                        //Apply the interpupillary distance
                        //Maybe we need to change this a bit, so that the distance is calculated by considering the direction the User looks.
                        //We see that when we test it.   
                        if(NodeInformation.cameraEye == "left")
                        {
                            CameraTransform.localPosition += new Vector3(-0.03f, 0, 0);
                        }
                        else
                        {
                            CameraTransform.localPosition += new Vector3(0.03f, 0, 0);
                        }
                    }
                }
            }
        }

    }
}