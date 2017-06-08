using System.Collections;
using System.Collections.Generic;
using Cave;
using UnityEngine;

public class Moving : MonoBehaviour {

    public float speed;
    public float rotationSpeed;

    void FixedUpdate()
    {
        Vector3 moveVector = new Vector3();
        // up
        if (InputSynchronizer.GetKey("y"))
        {
            moveVector.y += speed;
        }

        // down
        if (InputSynchronizer.GetKey("x"))
        {
            moveVector.y -= speed;
        }

        // forward
        if (InputSynchronizer.GetKey("w"))
        {
            moveVector.z += speed;
        }

        // backward
        if (InputSynchronizer.GetKey("s"))
        {
            moveVector.z -= speed;
        }

        // left
        if (InputSynchronizer.GetKey("a"))
        {
            moveVector.x -= speed;
        }

        // right
        if (InputSynchronizer.GetKey("d"))
        {
            moveVector.x += speed;
        }



        // Rotation
        Vector3 rotationVector = new Vector3();

        if (InputSynchronizer.GetKey("u"))
        {
            rotationVector.x += rotationSpeed;
        }

        if (InputSynchronizer.GetKey("j"))
        {
            rotationVector.x -= rotationSpeed;
        }

        if (InputSynchronizer.GetKey("h"))
        {
            rotationVector.z += rotationSpeed;
        }

        if (InputSynchronizer.GetKey("k"))
        {
            rotationVector.z -= rotationSpeed;
        }

        if (InputSynchronizer.GetKey("n"))
        {
            rotationVector.y += rotationSpeed;
        }

        if (InputSynchronizer.GetKey("m"))
        {
            rotationVector.y -= rotationSpeed;
        }
        gameObject.transform.Translate(moveVector);
        gameObject.transform.Rotate(rotationVector);
    }
}
