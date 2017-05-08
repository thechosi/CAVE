using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {

    public float speed;
    public float rotationSpeed;

    void FixedUpdate()
    {
        Vector3 moveVector = new Vector3();
        // up
        if (Input.GetKey(KeyCode.Y))
        {
            moveVector.y += speed;
        }

        // down
        if (Input.GetKey(KeyCode.X))
        {
            moveVector.y -= speed;
        }

        // forward
        if (Input.GetKey(KeyCode.W))
        {
            moveVector.z += speed;
        }

        // backward
        if (Input.GetKey(KeyCode.S))
        {
            moveVector.z -= speed;
        }

        // left
        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x -= speed;
        }

        // right
        if (Input.GetKey(KeyCode.D))
        {
            moveVector.x += speed;
        }



        // Rotation
        Vector3 rotationVector = new Vector3();

        if (Input.GetKey(KeyCode.U))
        {
            rotationVector.x += rotationSpeed;
        }

        if (Input.GetKey(KeyCode.J))
        {
            rotationVector.x -= rotationSpeed;
        }

        if (Input.GetKey(KeyCode.H))
        {
            rotationVector.z += rotationSpeed;
        }

        if (Input.GetKey(KeyCode.K))
        {
            rotationVector.z -= rotationSpeed;
        }

        if (Input.GetKey(KeyCode.N))
        {
            rotationVector.y += rotationSpeed;
        }

        if (Input.GetKey(KeyCode.M))
        {
            rotationVector.y -= rotationSpeed;
        }
        gameObject.transform.Translate(moveVector);
        gameObject.transform.Rotate(rotationVector);
    }
}
