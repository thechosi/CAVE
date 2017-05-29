using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RadialMenuSpawner : MonoBehaviour {

	public static RadialMenuSpawner ins;
	public RadialMenu menuPrefab;
	public float distanceFromFlystick;

	void Awake(){
		ins = this;
	}

	public void SpawnMenu(Interactable obj){
		RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;
		newMenu.transform.SetParent(transform, false);

		GameObject Flystick = GameObject.Find ("Flystick");

		if (Flystick != null) {
			newMenu.transform.position = Flystick.transform.position;
			newMenu.transform.rotation = Flystick.transform.rotation;

			//newMenu.transform.Rotate (270f,0f,0f);
			newMenu.transform.Translate (Flystick.transform.forward * distanceFromFlystick, Space.World);

			newMenu.SpawnButtons (obj);
			NetworkServer.Spawn (newMenu.gameObject);
		} else {
			Debug.Log ("Flystick not found");
		}
	}
}
