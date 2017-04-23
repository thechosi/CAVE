using UnityClusterPackage;
using UnityEngine;
using UnityEngine.Networking;

public class RigidBodySynchronizer : MonoBehaviour {

	// Use this for initialization
    void Synchronize(NetworkNode node)
    {
        if (NodeInformation.type.Equals("master"))
        {
            NetworkTransform[] networkTransforms = MonoBehaviour.FindObjectsOfType(typeof(NetworkTransform)) as NetworkTransform[];
            foreach (NetworkTransform networkTransform in networkTransforms)
            {
                networkTransform.SetDirtyBit(1);
            }
        }
    }

    public static void InitializeFromClient(Client client)
    {
        NetworkTransform[] networkTransforms = MonoBehaviour.FindObjectsOfType(typeof(NetworkTransform)) as NetworkTransform[];
        foreach (NetworkTransform networkTransform in networkTransforms)
        {
            Rigidbody rigidBody = networkTransform.GetComponentInParent<Rigidbody>();
            if (rigidBody != null)
                rigidBody.useGravity = false;
            networkTransform.interpolateMovement = 0;
            networkTransform.sendInterval = 0;
            networkTransform.movementTheshold = 0.000001f;
        }
    }

}
