using System;
using System.Collections;
using System.Collections.Generic;
using Cave;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

	[System.Serializable]
	public class Action
	{
		public Color color;
		public Sprite sprite;
		public string title;
		public UnityEvent action;
	}

	public Action[] options;


    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) || InputSynchronizer.GetFlyStickButtonDown(3)){
			RadialMenuSpawner.ins.SpawnMenu (this);
		}
	}
    
}
