using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreenManager : MonoBehaviour
{
    public Text timer;
    public Text playerInfo;
    public Text loseInfo;
    private static int time;
    private static Boolean isPlaying = false;
    private int startTime;

    private TopBlockPlacer topBlockPlacer;
    Animator anim;

    public static int Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }

    public int StartTime
    {
        get
        {
            return startTime;
        }

        set
        {
            startTime = value;
        }
    }

    public static bool IsPlaying
    {
        get
        {
            return isPlaying;
        }

        set
        {
            isPlaying = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ButtonStart()
    {
        IsPlaying = true;
        StartTime = (int)Cave.TimeSynchronizer.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
           time = (int)Cave.TimeSynchronizer.time - StartTime;
            timer.text = CreateTimerText(time);
           playerInfo.text = CreatePlayerText();
        }
    }

    string CreateTimerText(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;

        string timerText = "";
        if (minutes < 10)
        {
            timerText += "0";
        }
        timerText += minutes;

        timerText += ":";

        if (seconds < 10)
        {
            timerText += "0";
        }
        timerText += seconds;

        return timerText;
    }

    int FindActivePlayer()
    {
        TowerInteractivity tower = FindObjectOfType<TowerInteractivity>();
        /*foreach (Player p in tower.Players)
		{
			if (p.IsActive){
				return p.PlayerNumber + 1;
			}
		}

		return 0;*/
        return Player.ActivePlayer + 1;
    }

    string CreatePlayerText()
    {
        return "Turn Player " + FindActivePlayer() + "";
    }

    public void LoserView()
    {
		int playerNumber = Player.ActivePlayer + 1;
		loseInfo.text = "Player " + playerNumber + "lost";
		anim.SetBool("LoserView", true);
    }

    public void InfoView()
    {
		anim.SetBool("LoserView", false);
    }
}
