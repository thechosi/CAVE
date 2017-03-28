using UnityEngine;
using System.Collections;
using System;

namespace UnityClusterPackage {
	
	public class InstantiateNode : MonoBehaviour {
		
		public GameObject MultiProjectionCamera;
		
		void Awake() {
			Network.sendRate = 100;
		}
		
		void Start () {
			
			if ( NodeInformation.type.Equals("master") )
			{
				Network.proxyIP = NodeInformation.serverIp;
				bool useNat = Network.HavePublicAddress();
				Network.InitializeServer( NodeInformation.nodes, NodeInformation.serverPort, useNat );
				
				Network.Instantiate( MultiProjectionCamera, transform.position, transform.rotation, 0 );
			}
			else if ( NodeInformation.type.Equals("slave") )
			{
				Network.Connect( NodeInformation.serverIp, NodeInformation.serverPort );
			}
			
		}
		
		void OnServerInitialized()
		{
			Debug.Log( "Server initialized and ready." );
		}
		
		void OnPlayerConnected(NetworkPlayer player) {
			Debug.Log( "Node connected " + player.ipAddress + ":" + player.port );
		}
		
		void OnPlayerDisconnected( NetworkPlayer node ) {
			Debug.Log( "Clean up after player " + node );
			Network.RemoveRPCs(node);
			Network.DestroyPlayerObjects(node);
		}
		
		void OnConnectedToServer()
		{
			Debug.Log( "Connected to server." );
		}
		
		void OnFailedToConnect(NetworkConnectionError error)
		{
			Debug.Log( "Could not connect to server: " + error );
		}
		
		void OnDestroy() {
			
			if ( NodeInformation.type.Equals("master") )
			{
				Network.Disconnect();
			}
			else if ( NodeInformation.type.Equals("slave") )
			{
				Network.CloseConnection( Network.player, true );                
			}
		}
		
	}
}