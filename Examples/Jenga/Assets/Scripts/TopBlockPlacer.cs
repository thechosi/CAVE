using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cave;

public class TopBlockPlacer : CollisionSynchronization
{
    private static bool playerChangeable = false;

	public TopBlockPlacer()
		: base(new[] { Cave.EventType.OnCollisionEnter })
	{
		
	}

    public static bool PlayerChangeable
    {
        get
        {
            return playerChangeable;
        }

        set
        {
            playerChangeable = value;
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

	public override void OnSynchronizedCollisionEnter(GameObject other)
    {
        if (InfoScreenManager.IsPlaying && InfoScreenManager.Time > 3 && other.GetComponent<Renderer>().material.color == Color.green)
        {
			if (Mathf.Abs(other.transform.eulerAngles.y - transform.eulerAngles.y) <= 120 && Mathf.Abs(other.transform.eulerAngles.y - transform.eulerAngles.y) >= 60 ||
				Mathf.Abs(other.transform.eulerAngles.y - transform.eulerAngles.y) <= 300 && Mathf.Abs(other.transform.eulerAngles.y - transform.eulerAngles.y) >= 240)
            {
                PlayerChangeable = true;
            }
            else
            {
                PlayerChangeable = false;
            }
        }

    }
}
