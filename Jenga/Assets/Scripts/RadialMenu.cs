using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class RadialMenu : MonoBehaviour {

	public RadialButton buttonPrefab;
	public RadialButton selected;
	public float radius;

	public void SpawnButtons (Interactable obj) {
		for (int i = 0; i< obj.options.Length; i++){
			RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
			newButton.transform.SetParent(transform, false);
			float theta = (2 * Mathf.PI / obj.options.Length) * i;
			float xPos = Mathf.Sin (theta);
			float yPos = Mathf.Cos (theta);
			newButton.transform.localPosition = new Vector3 (xPos, yPos, 0f) * radius;
			newButton.circle.color = obj.options [i].color;
			newButton.icon.sprite = obj.options [i].sprite;
			newButton.title.text = obj.options [i].title;
			newButton.action = obj.options [i].action;
			newButton.myMenu = this;
			NetworkServer.Spawn ( newButton.gameObject);
		}
	}

	public void Trigger(){
		if (selected) {				
			selected.ButtonPressed ();
		}
	}

	void Update(){
		GameObject flystick = GameObject.Find ("Flystick");

		RaycastHit hit;

		if (Physics.Raycast (flystick.transform.position, flystick.transform.up, out hit, 10)) {
			if (hit.collider.gameObject.name == "Button(Clone)") {
				GameObject hitObj = hit.collider.gameObject;
				RadialButton hitButton = hitObj.GetComponent<RadialButton> ();

				selected = hitButton;
			}
		}
		else {
			selected = null;
		}
	}
}
