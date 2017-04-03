using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObject : MonoBehaviour
{

    public GameObject[] objects;
    public Vector3 spawnValues;
    public int objectnr = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateObject()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.y), Random.Range(-spawnValues.x, spawnValues.y), Random.Range(-spawnValues.x, spawnValues.y));
        objectnr = Random.Range(0, 3); 
        Instantiate(objects[objectnr], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
    }
}
