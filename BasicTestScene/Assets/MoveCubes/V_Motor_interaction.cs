using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Motor_interaction : interactionTrigger {
	public float explosionForce = 1;

	private double timer = 0;
	private int exploded = 1;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (timer > 0.0) {
			timer -= Time.deltaTime;
		}
	}

	override public void trigger(){
		if (timer <= 0.0) {
			explode (this.transform);
			exploded *= -1;
			timer = 1.0;
		}
	}

	private void explode(Transform m){
		Vector3 direction, myPosition, parentPosition;
		foreach (Transform o in m.transform) {
			if (m.tag != "IgnoreExplod") {
				myPosition = o.transform.position;
				parentPosition = this.transform.position; // o.gameObject.transform.parent.transform.position;
				direction = getDirection (parentPosition, myPosition);
				o.transform.position += direction * explosionForce * exploded;
				explode (o);
			}
		}
	}

	private Vector3 getDirection(Vector3 fromV, Vector3 toV){
		Vector3 v = new Vector3(0,0,0);
		v = toV - fromV;
		return v.normalized;
	}
}
