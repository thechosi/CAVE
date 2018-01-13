using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour {

	public GameObject parent;

	//void OnPreCull(){
	void LateUpdate(){
		transform.rotation = parent.transform.rotation * Quaternion.Euler(Cave.NodeInformation.own.cameraRoation);
	}
}
