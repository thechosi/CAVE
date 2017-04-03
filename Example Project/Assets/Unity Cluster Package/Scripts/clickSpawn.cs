using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickSpawn : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => OnClicked());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClicked()
    {

    }

}