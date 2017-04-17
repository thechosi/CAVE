using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubes : MonoBehaviour {

    public float force;
    

	// Use this for initialization
	void Start () {
        force = 100f;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

        // as of now, only the cubes with the "+" material have this script, so only they move!

        // up
        if (Input.GetKey(KeyCode.Y))
        {
            // disable gravity (if necessary)
            //rigidbody.useGravity = false;
            rigidbody.AddForce(0, force, 0, ForceMode.Force);
        }

        // down
        if (Input.GetKey(KeyCode.X))
        {
            rigidbody.AddForce(0, -force, 0, ForceMode.Force);
        }

        // forward
        if (Input.GetKey(KeyCode.W))
        {
            //gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.05f, 0);

            rigidbody.AddForce(0, 0, force, ForceMode.Force);
        }

        // backward
        if (Input.GetKey(KeyCode.S))
        {
            //gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.01f, 0);
            rigidbody.AddForce(0, 0, -force, ForceMode.Force);
        }

        // left
        if (Input.GetKey(KeyCode.A))
        {
            //gameObject.transform.position = gameObject.transform.position + new Vector3(-0.01f, 0, 0);
            rigidbody.AddForce(-force, 0, 0, ForceMode.Force);
        }

        // right
        if (Input.GetKey(KeyCode.D))
        {
            //gameObject.transform.position = gameObject.transform.position + new Vector3(0.01f, 0, 0);
            rigidbody.AddForce(force, 0, 0, ForceMode.Force);
        }
        
    }
}
