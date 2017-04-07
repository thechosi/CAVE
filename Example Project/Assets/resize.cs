using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resize : MonoBehaviour
{

    Vector3 max = new Vector3(5.0f, 5.0f, 5.0f);
    Vector3 min = new Vector3(0.1f, 0.1f, 0.1f);
    float inc = 0.05f;

    bool increase = true;

    // Use this for initialization
    void Start()
    {
    }



    // Update is called once per frame
    void Update()
    {
        if (increase)
        {
            transform.localScale += new Vector3(inc, inc, inc);
            if (Vector3.SqrMagnitude(transform.localScale - max) < 0.01)
            {
                increase = false;
            }
        }
        else
        {
            transform.localScale -= new Vector3(inc, inc, inc);
            if (Vector3.SqrMagnitude(transform.localScale - min) < 0.01)
            {
                increase = true;
            }
        }
    }
}
