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
		//TODO Set position and rotation to flystick
		newMenu.transform.position = new Vector3(0f,2f,-5f);
		newMenu.transform.eulerAngles = new Vector3 (20f,0f,0f);
		newMenu.SpawnButtons (obj);
	}
}
