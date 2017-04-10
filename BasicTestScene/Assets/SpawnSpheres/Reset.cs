using System.Collections;
using System.Collections.Generic;
using UnityClusterPackage;
using UnityEngine;
using UnityEngine.Networking;

public class Reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.R))
        {
            //Destroy(gameObject);
            if (NodeInformation.type.Equals("master"))
            {
                NetworkServer.Destroy(gameObject);
            }

        }
		
	}
}
