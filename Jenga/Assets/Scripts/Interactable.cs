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

	private RadialMenu menu = null;

    void Update(){
		if (menu == null && (Input.GetKeyDown(KeyCode.Escape) || InputSynchronizer.GetFlyStickButtonDown(3))){
			menu = RadialMenuSpawner.ins.SpawnMenu (this);

			// fade music if in menu modus
			GameObject BackgroundMusic = GameObject.Find ("BackgroundMusic");
			AudioSource backgroundMusicSource = BackgroundMusic.GetComponent<AudioSource>();
			backgroundMusicSource.volume = 0.5f;

			Debug.Log ("Radial Menu created");
		}else if (menu!=null && (Input.GetKeyDown(KeyCode.Escape) || InputSynchronizer.GetFlyStickButtonDown(3))){
			// music back to full volume
			GameObject BackgroundMusic = GameObject.Find ("BackgroundMusic");
			AudioSource backgroundMusicSource = BackgroundMusic.GetComponent<AudioSource>();
			backgroundMusicSource.volume = 1.0f;

			menu.Trigger ();
			Destroy (menu.gameObject);
			menu = null;

			Debug.Log ("Radial Menu destroyed");
		}
	}    
}
