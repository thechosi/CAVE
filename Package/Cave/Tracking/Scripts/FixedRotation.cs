using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour {

	public GameObject parent;

	//void OnPreCull(){
	void LateUpdate(){
		transform.eulerAngles = parent.transform.rotation.eulerAngles + Cave.NodeInformation.own.cameraRoation;
	}
}
