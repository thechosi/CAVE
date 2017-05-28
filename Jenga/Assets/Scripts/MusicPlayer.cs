﻿using System.Collections; using System.Collections.Generic; using UnityEngine;  public class MusicPlayer : MonoBehaviour {  	public AudioClip[] clips; 	private AudioSource audiosource;  	void Start () 	{ 		audiosource = GetComponent<AudioSource>(); 	}  	private AudioClip GetRandomClip(){  		return clips [Random.Range (0, clips.Length)]; 	}   	// Update is called once per frame 	void Update () 	{ 		audiosource = GetComponent<AudioSource>();  		if (!audiosource.isPlaying) 		{ 			audiosource.clip = GetRandomClip(); 			audiosource.Play(); 		} 	}  }  