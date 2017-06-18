using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class RadialButton : MonoBehaviour
{

    public Image circle;
    public Image icon;
    public Text title;
    public RadialMenu myMenu;
	public UnityEvent action;

    public void ButtonPressed()
    {
		action.Invoke ();

		AudioSource pressedButtonSound = GetComponent<AudioSource>();
		pressedButtonSound.Play ();
    }

    public void Update()
    {
        if (myMenu.selected == this)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
