using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour {

    private float translateVelocity = 0.125f;
    private Vector3 directionVector;

    // Use this for initialization
    void Start () {
        NetworkView nView = GetComponent<NetworkView>();
        if (!nView.isMine) {
            enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        directionVector = new Vector3(-1.0f*Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        transform.position += directionVector * translateVelocity;
    }
}
