using UnityEngine;
using UnityClusterPackage;
using UnityEngine.Networking;

public class TeapotRotate : MonoBehaviour {

    // Use this for initialization
    void Start() {
        GetComponent<NetworkTransform>().sendInterval = 0.01f;
        if (NodeInformation.type.Equals("slave"))
        {
            enabled = false;
        }
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * 30.0f);
    }
}


