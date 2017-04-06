using System.Collections;
using System.Collections.Generic;
using UnityClusterPackage;
using UnityEngine;
using UnityEngine.Networking;

public class CircularMovement : MonoBehaviour
{

	float timeCounter = 0;
    
    [Tooltip("Multiplicator for the movement speed.")]
	public float speed = 1f;
    [Tooltip("Set radius.")]
    public float distance = 10f;

    // Initial position values
    float x, y, z;

    // Use this for initialization
    void Start () {

        // Get initial position
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }
	
	
	// Update is called once per frame
	void Update () {
        
        timeCounter += (Synchronizer.deltaTime * speed);
		float xt = Mathf.Cos (timeCounter) * distance;
		float zt = Mathf.Sin (timeCounter) * distance;
		transform.position = new Vector3 (x + xt, y, z + zt);

	}
}
