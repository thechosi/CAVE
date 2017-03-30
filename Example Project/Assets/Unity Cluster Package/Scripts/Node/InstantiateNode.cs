using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

namespace UnityClusterPackage {
	
	public class InstantiateNode : MonoBehaviour {
		
		public GameObject MultiProjectionCamera;
        
		void Start ()
		{
		    NetworkManager networkManager = GetComponent<NetworkManager>();
            networkManager.networkAddress = NodeInformation.serverIp;
            networkManager.networkPort = NodeInformation.serverPort;
            if ( NodeInformation.type.Equals("master") )
            {
                networkManager.StartServer();

                GameObject obj = (GameObject)Instantiate(MultiProjectionCamera, transform.position, transform.rotation);
                NetworkServer.Spawn(obj);
			}
			else if ( NodeInformation.type.Equals("slave") )
            {
                networkManager.StartClient();
			}
		}
	}
}