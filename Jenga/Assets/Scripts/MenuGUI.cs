using UnityEngine;
using System.Collections;
using System;
using Cave;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuGUI : MonoBehaviour
{
    enum MenuState
    {
        Main,
        Credits,
        Options,
        Hidden,
        Player
    }

    private MenuState state = MenuState.Main;
    private bool musicOn = true;

    public GameObject playBtn;
    public GameObject creditsBtn;
    public GameObject exitBtn;
    public GameObject settingsBtn;
    public GameObject musicBtn;
    public GameObject backBtn;
    public GameObject header;
    public GameObject nrOfPlayers;

    private bool allButtonsSpawned = false;

    void Start()
    {
        if (NodeInformation.type.Equals("master"))
        {
            state = MenuState.Main;
            refresh();
        }
    }

    public void playButton()
    {
        state = MenuState.Player;
        refresh();
    }

    public void createPlayers()
    {
        TowerInteractivity tower = FindObjectOfType<TowerInteractivity>();
        tower.NrOfPlayers = Int32.Parse(nrOfPlayers.transform.FindChild("Text").GetComponent<Text>().text);
        for (int i = 0; i < tower.NrOfPlayers; i++)
        {
            tower.Players.Add(ScriptableObject.CreateInstance<Player>());
            tower.Players[i].Score = 0;
            tower.Players[i].PlayerNumber = i;
        }
        tower.Players[0].IsActive = true;
    }

    public void startGame()
    {
        state = MenuState.Hidden;
        createPlayers();
        refresh();
    }

    public void settingsButton()
    {
        state = MenuState.Options;
        refresh();
        if (!allButtonsSpawned && NodeInformation.type.Equals("master"))
        {
            NetworkServer.Spawn(musicBtn);
            NetworkServer.Spawn(backBtn);
            allButtonsSpawned = true;
        }
    }

    public void creditsButton()
    {
        state = MenuState.Credits;
        refresh();
    }

    public void exitButton()
    {
        Application.Quit();
    }

    public void backButton()
    {
        state = MenuState.Main;
        refresh();
    }

    public void showMenuFunc()
    {
        state = MenuState.Main;
        refresh();
    }


    public void refresh()
    {
        playBtn.SetActive(state == MenuState.Main);
        creditsBtn.SetActive(state == MenuState.Main);
        exitBtn.SetActive(state == MenuState.Main);
        settingsBtn.SetActive(state == MenuState.Main);
        header.SetActive(state == MenuState.Main);

        nrOfPlayers.SetActive(state == MenuState.Player);

        musicBtn.SetActive(state == MenuState.Options);
        backBtn.SetActive(state != MenuState.Main && state != MenuState.Hidden);
    }

    public void toggleMusic()
    {
        var music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        if (musicOn)
        {
            music.mute = true;
            musicBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = "Music ON";
        }
        else
        {
            music.mute = false;
            musicBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = "Music OFF";
        }
        musicOn = !musicOn;
    }

}
