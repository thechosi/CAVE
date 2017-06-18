using UnityEngine;
using System.Collections;

public class interactionTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void trigger(){
		print ("No function to execute! Make sure your function is call \"public override void trigger\"");
	}
}
