using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cave;

public class FlystickSphereInteraction : MonoBehaviour {
    
    public GameObject FlystickSphere;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (this.FlystickSphere == null)
        {
            this.FlystickSphere = GameObject.Find("Flystick/FlystickSphere");
            Debug.Log(FlystickSphere);
        }
        if (this.FlystickSphere != null)
        {
            if (Input.GetKey(KeyCode.I) || InputSynchronizer.GetFlyStickButton(2))
            {
                FlystickSphere.SetActive(true);
            }
            else
            {
                FlystickSphere.SetActive(false);
            }
        }

    }
}
