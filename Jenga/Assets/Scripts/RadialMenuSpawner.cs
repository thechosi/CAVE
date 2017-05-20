using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuSpawner : MonoBehaviour {

	public static RadialMenuSpawner ins;
	public RadialMenu menuPrefab;

	void Awake(){
		ins = this;
	}

	public void SpawnMenu(Interactable obj){
		RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;
		newMenu.transform.SetParent(transform, false);

		GameObject Flystick = GameObject.Find ("FlystickSim");

		newMenu.transform.position = Flystick.transform.position;
		newMenu.transform.rotation = Flystick.transform.rotation;

		newMenu.transform.Rotate (270f,0f,0f);
		newMenu.transform.Translate (Flystick.transform.up*2, Space.World);

		newMenu.SpawnButtons (obj);
	}
}
