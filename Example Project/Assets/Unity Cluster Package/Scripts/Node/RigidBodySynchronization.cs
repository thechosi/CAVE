using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RigidBodySynchronization : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        if (!GetComponent<NetworkIdentity>().hasAuthority)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

}
