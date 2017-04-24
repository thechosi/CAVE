using UnityEngine;
using System.Collections;

public class FixedMoveDirection : MonoBehaviour {

	public bool allowX = false;
	public bool allowY = false; 
	private Quaternion rotIni;
	private float xIni;
	private float yIni;
	private float zIni;

	// Use this for initialization
	void Awake () {
		rotIni  = transform.rotation;
		xIni = transform.position.x;
		yIni = transform.position.y;
		zIni = transform.position.z;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.rotation = rotIni;
		Vector3 newPos = transform.position;
		newPos.z = zIni;
		if (!allowX) {
			newPos.x = xIni;
		}
		if (!allowY) {
			newPos.y = yIni;
		}
		transform.position = newPos;
	}
}
