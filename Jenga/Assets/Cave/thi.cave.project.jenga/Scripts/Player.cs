using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ScriptableObject
{
    private int playerNumber;
    private int score = 0;
    private bool isActive = false;
    private static int activePlayer = 0;

    public int PlayerNumber
    {
        get
        {
            return playerNumber;
        }

        set
        {
            playerNumber = value;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
        }
    }

    public static int ActivePlayer
    {
        get
        {
            return activePlayer;
        }

        set
        {
            activePlayer = value;
        }
    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void resetStats()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            p.Score = 0;
        }
    }

    public static int changeActivePlayer()
    {
        TowerInteractivity tower = FindObjectOfType<TowerInteractivity>();
        foreach (Player p in tower.Players)
        {
            p.IsActive = false;
        }

        if (ActivePlayer < tower.NrOfPlayers - 1)
        {
            ActivePlayer++;
            tower.Players[ActivePlayer].IsActive = true;
        }
        else
        {
            ActivePlayer = 0;
            tower.Players[0].IsActive = true;
        }
        return ActivePlayer;
    }
}
