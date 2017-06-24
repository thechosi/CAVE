using UnityEngine;
using System.Collections;

public abstract class FlystickNavigation : MonoBehaviour {

	// Use this for initialization
	void Start (){
	}
	
	// Update is called once per frame
	void Update (){
	}
}

public class FlystickNavigationJoystick : FlystickNavigation {
}
public class FlystickNavigationElastic : FlystickNavigation {
}
public class FlystickNavigationGrabWorld : FlystickNavigation {
}
