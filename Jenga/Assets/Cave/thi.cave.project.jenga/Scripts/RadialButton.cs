using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RadialButton : MonoBehaviour
{

    public Image circle;
    public Image icon;
    public Text title;
    public RadialMenu myMenu;

    public void ButtonPressed()
    {
        Debug.Log("button " + title.text + " selected");

        if (title.text == "Zurücksetzen")
        {
            GameObject towerObject = GameObject.Find("DynamicTower");
            if (towerObject != null)
            {
                TowerInteractivity tower = towerObject.GetComponent<TowerInteractivity>();
                tower.destroyTower();
                tower.createTower();
            }
        }
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
