using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStickSimInvisible : MonoBehaviour {

    public bool visible = false;

	// Use this for initialization
	void Start () {

        if (visible)
        {
            gameObject.active = true;
            GetComponentInChildren<Renderer>().enabled = true;
        }
        else
        {
            gameObject.active = false;
            GetComponentInChildren<Renderer>().enabled = false;
        }
	}
}
