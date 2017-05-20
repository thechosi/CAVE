using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour {

	public GameObject parent;

	//void OnPreCull(){
	void Update(){
		transform.rotation = parent.transform.rotation;
	}
}
