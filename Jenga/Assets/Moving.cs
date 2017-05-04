using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {

    public float speed;
    public float rotationSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        // as of now, only the cubes with the "+" material have this script, so only they move!
        Vector3 moveVector = new Vector3();
        // up
        if (Input.GetKey(KeyCode.Y))
        {
            //rigidbody.velocity = new Vector3(0, force, 0);
            moveVector.y += speed;
        }

        // down
        if (Input.GetKey(KeyCode.X))
        {
            //rigidbody.velocity = new Vector3(0, -force, 0);
            moveVector.y -= speed;
        }

        // forward
        if (Input.GetKey(KeyCode.W))
        {
            //rigidbody.velocity = new Vector3(0, 0, force);
            moveVector.z += speed;
        }

        // backward
        if (Input.GetKey(KeyCode.S))
        {
            //rigidbody.velocity = new Vector3(0, 0, -force);
            moveVector.z -= speed;
        }

        // left
        if (Input.GetKey(KeyCode.A))
        {
            //rigidbody.velocity = new Vector3(-force, 0, 0);
            moveVector.x -= speed;
        }

        // right
        if (Input.GetKey(KeyCode.D))
        {
            //rigidbody.velocity = new Vector3(force, 0, 0);
            moveVector.x += speed;
        }




        Vector3 rotationVector = new Vector3();

        if (Input.GetKey(KeyCode.U))
        {
            //rigidbody.velocity = new Vector3(force, 0, 0);
            rotationVector.x += rotationSpeed;
        }

        if (Input.GetKey(KeyCode.J))
        {
            //rigidbody.velocity = new Vector3(force, 0, 0);
            rotationVector.x -= rotationSpeed;
        }

        if (Input.GetKey(KeyCode.H))
        {
            //rigidbody.velocity = new Vector3(force, 0, 0);
            rotationVector.z += rotationSpeed;
        }

        if (Input.GetKey(KeyCode.K))
        {
            //rigidbody.velocity = new Vector3(force, 0, 0);
            rotationVector.z -= rotationSpeed;
        }
        gameObject.transform.Translate(moveVector);
        gameObject.transform.Rotate(rotationVector);
    }
}
