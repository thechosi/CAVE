using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationSync : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        NetworkAnimator netAnim = GetComponent<NetworkAnimator>();

        netAnim.SetParameterAutoSend(0, true);
    }
}
