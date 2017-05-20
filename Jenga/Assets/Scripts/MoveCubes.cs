using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveCubes : MonoBehaviour
{

    public float force;


    // Use this for initialization
    void Start()
    {
        force = 5f;
        // initCubes();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

        // as of now, only the cubes with the "+" material have this script, so only they move!
        Vector3 moveVector = new Vector3();
        // up
        if (Input.GetKey(KeyCode.Y))
        {
            //rigidbody.velocity = new Vector3(0, force, 0);
            moveVector.y = force;
        }

        // down
        if (Input.GetKey(KeyCode.X))
        {
            //rigidbody.velocity = new Vector3(0, -force, 0);
            moveVector.y = -force;
        }

        // forward
        if (Input.GetKey(KeyCode.W))
        {
            //rigidbody.velocity = new Vector3(0, 0, force);
            moveVector.z = force;
        }

        // backward
        if (Input.GetKey(KeyCode.S))
        {
            //rigidbody.velocity = new Vector3(0, 0, -force);
            moveVector.z = -force;
        }

        // left
        if (Input.GetKey(KeyCode.A))
        {
            //rigidbody.velocity = new Vector3(-force, 0, 0);
            moveVector.x = -force;
        }

        // right
        if (Input.GetKey(KeyCode.D))
        {
            //rigidbody.velocity = new Vector3(force, 0, 0);
            moveVector.x = force;
        }

        // if no force is applied, dont change velocity (so gravity can work)
        if (moveVector.sqrMagnitude > 0)
        {
            rigidbody.velocity = moveVector;
        }

    }
}
