using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class InterpupillaryDistance : MonoBehaviour {
	
	public float interpupillaryDistance;
	public GameObject camLeft;
	public GameObject camRight;

	// Use this for initialization
	void Awake () {
		//changeInterpupillaryDistance (interpupillaryDistance);

		Vector3 newPos = Vector3.zero;
		newPos.x += interpupillaryDistance / 2.0f;
		camRight.transform.localPosition = newPos;
		newPos.x *= -1;
		camLeft.transform.localPosition  = newPos;
		print ("Interpupillary  Distance: " + getInterpupillaryDistance().ToString ("F6"));
	}

	void Update(){

	}


	// TODO: Warum werden die x koordinaten nicht resetet?
	void OnApplicationQuit(){
		Vector3 zero = new Vector3 (0f, 0f, 0f);
		camRight.transform.localPosition = zero;
		camLeft.transform.localPosition = zero;
	}

	// Chance the Interpupillary Distance deltaDistance
	void changeInterpupillaryDistance(float deltaDistance){
		Vector3 oldRight = camRight.transform.localPosition;
		Vector3 oldLeft = camLeft.transform.localPosition;
		oldRight.x += deltaDistance / 2.0f;
		oldLeft.x -= deltaDistance/2.0f;

		camRight.transform.localPosition = oldRight;
		camLeft.transform.localPosition = oldLeft;
		/*
		camRight.transform.Translate (Vector3.left * (deltaDistance / 2.0f));
		camLeft.transform.Translate (Vector3.right * (deltaDistance / 2.0f));

		*/
		print ("Interpupillary  Distance: " + getInterpupillaryDistance().ToString ("F6"));
	}

	// Chance the Z Distance deltaDistance
	void changeZDistance(float deltaDistance){
		Vector3 oldRight = camRight.transform.localPosition;
		Vector3 oldLeft = camLeft.transform.localPosition;
		oldRight.z += deltaDistance;
		oldLeft.z -= deltaDistance;

		camRight.transform.localPosition = oldRight;
		camLeft.transform.localPosition = oldLeft;
		/*
		camRight.transform.Translate (Vector3.left * (deltaDistance / 2.0f));
		camLeft.transform.Translate (Vector3.right * (deltaDistance / 2.0f));

		*/
		print ("Interpupillary  Distance: " + getInterpupillaryDistance().ToString ("F6"));
	}
	float getInterpupillaryDistance(){
		return Vector3.Distance (camLeft.gameObject.transform.localPosition, camRight.gameObject.transform.localPosition);
	}
}