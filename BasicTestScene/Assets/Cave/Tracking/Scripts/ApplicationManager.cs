using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ApplicationManager : MonoBehaviour {
	public string modelScene;
	// Use this for initialization
	void Start () {
		if (modelScene!=null) {
			//SceneManager.LoadScene (modelScene, LoadSceneMode.Additive);
		} else {
			print ("No Valid Scene!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape") || Input.GetKey("q"))
		{
			Application.Quit();
		}
	}
}
