using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public GUISkin menuSkin;
    public GUISkin gameSkin;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        
        switch (GameManager.instance.State)
        {
            case GameState.PLAYING:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GameManager.instance.State = GameState.PAUSEMENU;
                }
                break;
            case GameState.PAUSEMENU:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GameManager.instance.State = GameState.PLAYING;
                }
                break;
            case GameState.OVER:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.instance.State = GameState.MENU;
                }
                break;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.Label(string.Format(" Qurmaq {0}.{1} on {2}, Unity {3}", GameManager.gameVersion.Major, GameManager.gameVersion.Minor, Application.platform, Application.unityVersion));
        switch (GameManager.instance.State)
        {
            case GameState.PAUSEMENU:
            case GameState.MENU:
                GUI.skin = menuSkin;
                GUILayout.BeginVertical();
                GUILayout.Label("Jenga", new GUIStyle("title"));
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical("box");
                if (GameManager.instance.State == GameState.PAUSEMENU)
                {
                    if (GUILayout.Button("Return to game"))
                    {
                        GameManager.instance.State = GameState.PLAYING;
                    }
                    GUILayout.Space(18);
                }
                if (GUILayout.Button("Play"))
                {
                    if (GameManager.instance.State == GameState.PAUSEMENU)
                    {
                        SceneManager.LoadScene("GameScene");
                    }
                    GameManager.instance.State = GameState.PLAYING;
                }
                if (GameManager.instance.State == GameState.PAUSEMENU)
                {
                    if (GUILayout.Button("Back to menu"))
                    {
                        GameManager.instance.State = GameState.MENU;
                    }
                }
                if (GUILayout.Button("Exit"))
                {
                    GameManager.instance.State = GameState.EXIT;
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                break;
            case GameState.OVER:
                GUI.skin = gameSkin;
                GUILayout.Label("<size=28><b>Game over!</b></size>\n<size=20>Press space or left mouse button...</size>", new GUIStyle("label") {
                    alignment = TextAnchor.MiddleCenter,
                    stretchWidth = true,
                    stretchHeight = true
                });
                break;
            case GameState.EXIT:
                GUI.skin = menuSkin;
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical("box");
                GUILayout.Label("<b><size=24>Exit the game?</size></b>");
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Yes"))
                {
                    Application.Quit();
                }
                if (GUILayout.Button("No"))
                {
                    GameManager.instance.State = GameManager.instance.PreviousState;
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                break;
        }
        GUILayout.EndArea();
    }
}
