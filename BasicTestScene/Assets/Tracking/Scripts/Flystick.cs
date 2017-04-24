using UnityEngine;
using System.Collections;

public class Flystick : MonoBehaviour {
	public CharacterController characterController;
	public float gravity = 5f;
	public enum NavigationEnum{None,Joystick,Elastic,GrabWorld};
	public NavigationEnum navigationMode;
	public bool manipulation;
	public GameObject characterGameObject;
	public float rotationSpeed = 1;
	public float moveSpeed = 0.1f;	
	public Transform forward;
	public bool showDebugRay = true;
	public float rayLength = 10f;
	public bool fly = true;
	public LayerMask interactableObjectLayer;

	private int nrOfButtons = 6;
	private bool[] buttons;
	private float joystickX;
	private float joystickY;
	private FlystickNavigation navigation;
	private Vector3 forwardVector;
	private VRInteractiveItem currentInteractable;
	private VRInteractiveItem lastInteractable;

	// Use this for initialization
	void Awake () {
		buttons = new bool[nrOfButtons];
		switch (navigationMode) {
		case NavigationEnum.None:
			navigation = null;
			break;
		case NavigationEnum.Joystick:
			navigation = new FlystickNavigationJoystick();
			break;
		case NavigationEnum.Elastic:
			navigation = new FlystickNavigationElastic();
			break;
		case NavigationEnum.GrabWorld:
			navigation = new FlystickNavigationGrabWorld();
			break;
		}	
	}
	
	// Update is called once per frame
	void Update () {
		// get normalized forward vector of flystick
		forwardVector = forward.position - transform.position;
		Raycast ();
	}

	private void Raycast()
	{
		// Show the debug ray if required
		if (showDebugRay) 
		{
			Debug.DrawRay(transform.position, forwardVector * rayLength, Color.blue);
		}
		// Create a ray that points forwards from the camera.
		Ray ray = new Ray(transform.position, forwardVector);
		RaycastHit hit;

		// Do the raycast forweards to see if we hit an interactive item
		if (Physics.Raycast(ray, out hit, rayLength,interactableObjectLayer)) //TODO exclode unused layers (~usedlayer)
		{
			VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
			currentInteractable = interactible;

			// If we hit an interactive item and it's not the same as the last interactive item, then call Over
			if (interactible && interactible != lastInteractable)
				interactible.Over(); 

			// Deactive the last interactive item 
			if (interactible != lastInteractable)
				DeactiveLastInteractible();

			lastInteractable = interactible;

			// Something was hit, set at the hit position.
			//TODO more functions like click and so on
		}
		else
		{
			// Nothing was hit, deactive the last interactive item.
			DeactiveLastInteractible();
			currentInteractable = null;
		}
	}

	// Update Physics and movement
	void FixedUpdate () {
		// TODO move in seperate file -> different methods -> maybe StatePattern
		characterGameObject.transform.Rotate (0,joystickX * rotationSpeed,0);

		float speed = joystickY * moveSpeed;
		Vector3 transformVector = forwardVector * speed;
		if (!fly) {
			transformVector.y -= gravity;
		}
		if (transformVector.x != 0 || transformVector.y != 0 || transformVector.z != 0) {
			characterController.Move (transformVector);
		}
	}

	public void setButtons (bool[] newButtons) {
		for (int i = 0; i < nrOfButtons; i++) {
			buttons [i] = newButtons [i];
		}
	}

	public void setJoystick (float newJoystickX, float newJoystickY) {
		joystickX = newJoystickX;
		joystickY = newJoystickY;
	}	

	private void DeactiveLastInteractible(){
		if (lastInteractable == null) {
			return;
		}
		lastInteractable.Out();
		lastInteractable = null;
	}
}