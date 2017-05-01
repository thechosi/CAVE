using UnityEngine;
using System.Collections;
using System;

public delegate void GameStateChangedEventHandler(object sender,GameStateChangedEventArgs e);

public class GameManager
{
    public static Version gameVersion = new Version(0, 1);
    public static GameManager instance = new GameManager();

    public event GameStateChangedEventHandler StateChanged;
    public GameState State
    {
        get { return _state; }
        set
        {
            if (_state == value)
                return;
            PreviousState = _state;
            _state = value;

            if (StateChanged != null)
                StateChanged(this, new GameStateChangedEventArgs(_state, PreviousState));
        }
    }
    public GameState PreviousState { get; protected set; }

    private GameState _state;

    private GameManager()
    {
        StateChanged += OnStateChanged;

        State = GameState.MENU;
    }

    void OnStateChanged(object sender, GameStateChangedEventArgs e)
    {
        switch (State)
        {
            case GameState.PAUSEMENU:
                Time.timeScale = 0;
                break;
            case GameState.MENU:
                if (e.PreviousState == GameState.PAUSEMENU || 
                    e.PreviousState == GameState.OVER || 
                    e.PreviousState == GameState.INTRO)
                {
                    Application.LoadLevel("MenuScene");
                }
                break;
            case GameState.PLAYING:
                if (e.PreviousState != GameState.PAUSEMENU)
                    Application.LoadLevel("GameScene");
                break;
        }

        if (State == GameState.PLAYING || State == GameState.MENU)
            Time.timeScale = 1;
    }
}

public class GameStateChangedEventArgs
{
    public GameState CurrentState { get; private set; }
    public GameState PreviousState { get; private set; }

    public GameStateChangedEventArgs(GameState newState, GameState previousState)
    {
        CurrentState = newState;
        PreviousState = previousState;
    }
}

public enum GameState
{
    INTRO,
    MENU,
    EXIT,
    PLAYING,
    PAUSEMENU,
    OVER
}