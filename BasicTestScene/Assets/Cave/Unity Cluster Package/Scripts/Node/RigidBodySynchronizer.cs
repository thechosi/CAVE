using AwesomeSockets.Domain.Sockets;
using UnityClusterPackage;
using UnityEngine;
using UnityEngine.Networking;

public class RigidBodySynchronizer : MonoBehaviour {

    private static void InitializeNetworkTransforms()
    {
        NetworkTransform[] networkTransforms = Resources.FindObjectsOfTypeAll(typeof(NetworkTransform)) as NetworkTransform[];
        foreach (NetworkTransform networkTransform in networkTransforms)
        {
            networkTransform.interpolateMovement = 0;
            networkTransform.sendInterval = 0.01f;
            networkTransform.movementTheshold = 0.0001f;
            networkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
        }
    }

    public static void InitializeFromServer(Server server, ISocket client)
    {
        InitializeNetworkTransforms();
    }

    public static void InitializeFromClient(Client client)
    {
        InitializeNetworkTransforms();
        Rigidbody[] rigidbodies = Resources.FindObjectsOfTypeAll(typeof(Rigidbody)) as Rigidbody[];
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            Destroy(rigidbody);
        }
    }

}
