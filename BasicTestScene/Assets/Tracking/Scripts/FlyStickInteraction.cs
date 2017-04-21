using UnityEngine;
using System.Collections;


public class FlyStickInteraction : MonoBehaviour {
	private RaycastHit rHit;
	private LineRenderer lineRender;
	private GameObject model, selectedPart;
	private TrackerSettings trackerSettings;

	public Transform origin;
	public Transform dest;
	public float maxRayDist = 10f;

	// Use this for initialization
	void Start () {		
		lineRender = this.GetComponent<LineRenderer> ();
		model = GameObject.FindWithTag ("InteractiveModel");
		print (model.name);
		trackerSettings = this.GetComponent<TrackerSettings> ();
	}
	
	// Update is called once per frame
	void Update () {
		drawLaser();
		rotateSelectedObject ();
	}

	private void sendRay(){
		if (Physics.Raycast (this.transform.position, this.transform.up, out rHit, maxRayDist)) {
			selectedPart = rHit.collider.gameObject.transform.parent.gameObject;
		}
	}

	private void drawLaser(){
		lineRender.SetPosition (0, origin.position);
		lineRender.SetPosition (1, dest.position);
	}

	public void trigger(){
		model.GetComponent<interactionTrigger> ().trigger ();
		/*if (selectedPart != null) {
			if (selectedPart.GetComponent<interactionTrigger> () != null) {
				selectedPart.GetComponent<interactionTrigger> ().trigger ();
			} else {
				print ("No interactionTrigger Script found! Make sure that your skrict inherited the interaction script and the trigger function is overrided.\n");
			}
		} else {
			print ("Nothing is selected");
		}*/
	}

	private void rotateSelectedObject(){
		Vector2 vec = trackerSettings.getAnalog ();
		Vector3 rot = new Vector3 (0, vec.x, 0);
		model.transform.Rotate (rot);
	}
}

