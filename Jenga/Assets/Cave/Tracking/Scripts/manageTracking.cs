using UnityEngine;
using System.Collections;

public class manageTracking : MonoBehaviour {
	private TrackerSettings trackingSettingScript;
	// Use this for initialization
	void Start () {
		trackingSettingScript = gameObject.GetComponent<TrackerSettings> ();
		if (trackingSettingScript == null) {
			print ("A tracking Settings Scipt must be attached to this Game Object");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//start tracking of cube
		if (Input.GetKeyDown (KeyCode.A) && trackingSettingScript != null) {
			trackingSettingScript.TrackPosition = true;
			trackingSettingScript.TrackRotation = true;
			print("Cubetracking started");
		}
		//Stop tracking of cube
		if (Input.GetKeyDown (KeyCode.S) && trackingSettingScript != null) {
			trackingSettingScript.TrackPosition = false;
			trackingSettingScript.TrackRotation = false;
			print ("Cubetracking stopped");
		}
	}
}
