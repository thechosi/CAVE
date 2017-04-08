using UnityEngine;
using System.Collections;
using UnityClusterPackage;
using UnityEngine.Networking;

public class FlyCamera : MonoBehaviour {

    private float translateVelocity = 0.125f;
    private Vector3 directionVector;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        directionVector = new Vector3(Synchronizer.GetAxis("Horizontal"), 0, Synchronizer.GetAxis("Vertical"));
        transform.position += directionVector * translateVelocity;
    }
}
