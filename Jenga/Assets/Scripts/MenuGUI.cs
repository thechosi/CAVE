using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MenuGUI : MonoBehaviour
{

    private Boolean showMenu = true;
    private Boolean musicOn = true;

    GameObject b1;
    GameObject b2;
    GameObject b3;
    GameObject b4;
    GameObject b5;
    GameObject btnBack;
    GameObject txt;

    void Start()
    {
        b1 = GameObject.Find("Btn_Play");
        b2 = GameObject.Find("Btn_Credits");
        b3 = GameObject.Find("Btn_Exit");
        b4 = GameObject.Find("Btn_Settings");
        b5 = GameObject.Find("Btn_Music");


        btnBack = GameObject.Find("Btn_Back");
        txt = GameObject.Find("MenuText");

        btnBack.SetActive(false);
        b5.SetActive(false);
    }

    public void playButton()
    {
        toggleMenu();
        btnBack.SetActive(false);
        
    }

    public void settingsButton()
    {
        toggleMenu();
        b5.SetActive(true);
    }

    public void creditsButton()
    {
        toggleMenu();
    }

    public void exitButton()
    {
        Application.Quit();
    }

    public void backButton()
    {
        toggleMenu();
    }

    public void showMenuFunc()
    {
        showMenu = false;
        toggleMenu();
    }


    public void toggleMenu()
    {
        showMenu = !showMenu;
        b1.SetActive(showMenu);
        b2.SetActive(showMenu);
        b3.SetActive(showMenu);
        b4.SetActive(showMenu);
        b5.SetActive(showMenu);
        txt.SetActive(showMenu);

        btnBack.SetActive(!showMenu);
    }

    public void toggleMusic()
    {
        var music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        if (musicOn)
        {
            music.Pause();
            b5.GetComponent<Button>().GetComponentInChildren<Text>().text = "Music ON";
        } else
        {
            music.Play();
            b5.GetComponent<Button>().GetComponentInChildren<Text>().text = "Music OFF";
        }
        musicOn = !musicOn;
    }

}
