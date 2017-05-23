using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBlockPlacer : MonoBehaviour
{
    private static bool playerChangeable = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.realtimeSinceStartup > 3 && collision.collider.transform.parent != null && collision.collider.transform.parent.parent.childCount - 1 == TowerInteractivity.MaxRow - 1)
        {
            if (Mathf.Abs(collision.collider.transform.eulerAngles.y - transform.eulerAngles.y) <= 120 && Mathf.Abs(collision.collider.transform.eulerAngles.y - transform.eulerAngles.y) >= 60 ||
                Mathf.Abs(collision.collider.transform.eulerAngles.y - transform.eulerAngles.y) <= 300 && Mathf.Abs(collision.collider.transform.eulerAngles.y - transform.eulerAngles.y) >= 240)
            {
                Debug.Log("Stein im richtigen Winkel platziert. ");
                PlayerChangeable = true;
            }
            else
            {
                Debug.Log("Stein nicht richtig Platziert. ");
                PlayerChangeable = false;
            }
        }

    }
}
