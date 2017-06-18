using UnityEngine;
using System.Collections;
using System;
using Cave;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuGUI : MonoBehaviour
{
    public enum MenuState
    {
        Main,
        Credits,
        Options,
        Hidden,
        Player
    }

    public MenuState state = MenuState.Main;
    private bool musicOn = true;

    public GameObject playBtn;
    public GameObject creditsBtn;
    public GameObject exitBtn;
    public GameObject settingsBtn;
    public GameObject musicBtn;
    public GameObject backBtn;
    public GameObject header;
    private GameObject nrOfPlayers;
    public GameObject twoPlayers;
    public GameObject threePlayers;
    public GameObject fourPlayers;
    public GameObject creditText;

    private Button selectedButton;

    private bool settingsSpawned = false;
	private bool playSpawned = false;
    private bool creditSpawned = false;

    void Start()
    {

        if (NodeInformation.type.Equals("master"))
        {
            state = MenuState.Main;
            refresh();
        }
    }

    void Update()
    {
        if (state != MenuState.Hidden)
        {
            GameObject flystickSim = GameObject.Find("FlystickSim");
            GameObject flystick = GameObject.Find("Flystick");

            RaycastHit hit;

            if (Physics.Raycast(flystickSim.transform.position + flystickSim.transform.up, flystickSim.transform.up, out hit, 10) || flystick && Physics.Raycast(flystick.transform.position, flystick.transform.forward, out hit))
            {
                if (hit.collider.name == "Plane")
                {
                    return;
                }

                GameObject hitObj = hit.collider.gameObject;
                Button hitButton = hitObj.GetComponent<Button>();

                if (selectedButton != null && selectedButton != hitButton)
                {
                    selectedButton.GetComponent<Image>().color = Color.white;
                }

                selectedButton = hitButton;

                selectedButton.GetComponent<Image>().color = Color.green;

            }
            else if (selectedButton != null)
            {
                selectedButton.GetComponent<Image>().color = Color.white;
                selectedButton = null;
            }

            if (selectedButton != null && (selectedButton.name == "Btn_2Player" || selectedButton.name == "Btn_3Player" || selectedButton.name == "Btn_4Player"))
            {
                nrOfPlayers = selectedButton.gameObject;
            }

            if (selectedButton != null && (InputSynchronizer.GetFlyStickButtonDown(0) || Input.GetButtonDown("Submit")))
            {
                selectedButton.onClick.Invoke();
            }

        }
    }

    public void playButton()
    {
        state = MenuState.Player;
        refresh();
		if (!playSpawned && NodeInformation.type.Equals("master"))
		{
			NetworkServer.Spawn(twoPlayers);
			NetworkServer.Spawn(threePlayers);
			NetworkServer.Spawn(fourPlayers);
            NetworkServer.Spawn(backBtn);
            playSpawned = true;
		}
    }

    public void createPlayers()
    {
        TowerInteractivity tower = FindObjectOfType<TowerInteractivity>();

        tower.NrOfPlayers = Int32.Parse(nrOfPlayers.transform.FindChild("Text").GetComponent<Text>().text.Substring(0, 1));

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
		if (!settingsSpawned && NodeInformation.type.Equals("master"))
        {
            NetworkServer.Spawn(musicBtn);
            NetworkServer.Spawn(backBtn);
			settingsSpawned = true;
        }
    }

    public void creditsButton()
    {
        state = MenuState.Credits;
        refresh();
        if (!creditSpawned && NodeInformation.type.Equals("master"))
        {
            NetworkServer.Spawn(creditText);
            NetworkServer.Spawn(backBtn);
            creditSpawned = true;
        }
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
        creditText.SetActive(state == MenuState.Credits);

        twoPlayers.SetActive(state == MenuState.Player);
        threePlayers.SetActive(state == MenuState.Player);
        fourPlayers.SetActive(state == MenuState.Player);

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
