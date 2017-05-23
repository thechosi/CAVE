using UnityEngine;
using UnityEngine.UI;

public class InfoScreenManager : MonoBehaviour {
	public Text timer;
	public Text playerInfo;
	public Text loseInfo;

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		int time = (int) Cave.TimeSynchronizer.time;
		timer.text = CreateTimerText (time);
		playerInfo.text = CreatePlayerText ();
	}

	string CreateTimerText(int time){
		int minutes = time / 60;
		int seconds = time % 60;

		string timerText = "";
		if (minutes < 10){
			timerText += "0";
		}
		timerText += minutes;

		timerText += ":";

		if (seconds < 10){
			timerText += "0";
		}
		timerText += seconds;

		return timerText;
	}

	int FindActivePlayer(){
		TowerInteractivity tower = FindObjectOfType<TowerInteractivity>();
		foreach (Player p in tower.Players)
		{
			if (p.IsActive){
				return p.PlayerNumber + 1;
			}
		}

		return 0;
	}

	string CreatePlayerText(){
		return "Spieler " + FindActivePlayer() + " ist dran!";
	}

	public void LoserView(int playerNumber){
		loseInfo.text = "Spieler " + playerNumber + "\nhat verloren!";
		anim.CrossFade ("LoserView", 0);
	}

	public void InfoView(){
		anim.CrossFade ("InfoView", 0);
	}
}
