using UnityEngine;
using UnityClusterPackage;

public class TeapotRotate : MonoBehaviour {

    // Use this for initialization
    void Start() {
        if ( NodeInformation.type.Equals("slave") ) {
            enabled = false;                                                                 
        }
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * 30.0f);
    }
}


