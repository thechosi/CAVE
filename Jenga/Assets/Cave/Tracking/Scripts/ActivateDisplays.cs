using UnityEngine;
using System.Diagnostics;
using System.Collections;


public class ActivateDisplays : MonoBehaviour {
	//#if !UNITY_EDITOR
	// Use this for initialization
	/*
	static void Awake(){
		Process proc = new Process();
		proc.StartInfo.FileName = "C:\\Windows\\System32\\DisplaySwitch.exe";
		proc.StartInfo.Arguments = "/extend";

		proc.Start();
		proc.WaitForExit();
	}*/

	void Start () {
		UnityEngine.Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.

		if (Display.displays.Length > 1) {
			Display.displays [1].Activate ();
		}
		if (Display.displays.Length > 2) {
			Display.displays [2].Activate ();
		}

	}
	/*
	void OnApplicationQuit(){
		Process proc = new Process();
		proc.StartInfo.FileName = "C:\\Windows\\System32\\DisplaySwitch.exe";
		proc.StartInfo.Arguments = "/clone";

		proc.Start();
		proc.WaitForExit(); 
	}*/
	//#endif
}
