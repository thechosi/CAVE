using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FlyCamera : MonoBehaviour {

    private float translateVelocity = 0.125f;
    private Vector3 directionVector;

    // Use this for initialization
    void Start () {
        NetworkIdentity nView = GetComponent<NetworkIdentity>();
        if (!nView.hasAuthority) {
            enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        directionVector = new Vector3(-1.0f*Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        transform.position += directionVector * translateVelocity;
    }
}
