using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TowerInteractivity : MonoBehaviour
{

    private GameObject selectedObj = null;

    private GameObject firstSelected = null;

    private int rows;

    private Material wood;

    public GameObject brick;

    public float diffBetweenBlocks;

    public int nrOfRows;

    public int nrOfPlayers;
    private List<Player> players = new List<Player>();

    private AudioSource destroyAudioSource;
    private AudioSource buttonSelectAudioSource;
    private static int maxRow;

    private Vector3 blockSize;

    public static int MaxRow
    {
        get
        {
            return maxRow;
        }
    }

    public List<Player> Players
    {
        get
        {
            return players;
        }

        set
        {
            players = value;
        }
    }

    public int NrOfPlayers
    {
        get
        {
            return nrOfPlayers;
        }

        set
        {
            nrOfPlayers = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < NrOfPlayers; i++)
        {
            Players.Add(ScriptableObject.CreateInstance<Player>());
            Players[i].Score = 0;
            Players[i].PlayerNumber = i;
        }
        Players[0].IsActive = true;

        if (diffBetweenBlocks < 0)
        {
            diffBetweenBlocks = 0;
        }
        createTower();

        //destroyAudioSource = GetComponent<AudioSource>()[0];
        //buttonSelectAudioSource = GetComponent<AudioSource>()[1];

    }

    private void setSizes()
    {
        GameObject newBrick;
        newBrick = Instantiate(brick) as GameObject;
        wood = newBrick.GetComponent<Renderer>().material;

        blockSize = newBrick.GetComponent<Renderer>().bounds.size;
        Destroy(newBrick);

        GameObject plane = GameObject.Find("Plane");
        plane.transform.position = new Vector3(0, -blockSize.y, 0);
    }

    public void createTower()
    {
        Debug.Log("creating Tower");

        setSizes();


        for (int i = 0; i < nrOfRows; i++)
        {
            addRow();
        }

        Debug.Log("Tower created");
    }

    private void addRow()
    {
        maxRow = transform.childCount;
        GameObject row = new GameObject();
        row.name = "Row#" + (maxRow + 1);
        row.transform.parent = this.transform;

        float absolutDiff = (blockSize.y + diffBetweenBlocks) * maxRow;
        GameObject newBrick;
        if ((maxRow + 1) % 2 == 1)
        {
            newBrick = Instantiate(brick, new Vector3(blockSize.z, absolutDiff, 0), Quaternion.identity) as GameObject;
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block1";
            newBrick = Instantiate(brick, new Vector3(blockSize.z, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block2";
            newBrick = Instantiate(brick, new Vector3(blockSize.z, absolutDiff, 2 * blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block3";
        }
        else
        {
            newBrick = Instantiate(brick, new Vector3(0, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.Rotate(new Vector3(0, 90, 0));
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block1";
            newBrick = Instantiate(brick, new Vector3(blockSize.z, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.Rotate(new Vector3(0, 90, 0));
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block2";
            newBrick = Instantiate(brick, new Vector3(2 * blockSize.z, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.Rotate(new Vector3(0, 90, 0));
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block3";
        }
    }

    private void await(int v, Func<object> p)
    {
        throw new NotImplementedException();
    }

    public void destroyTower()
    {
        buttonSelectAudioSource = GetComponent<AudioSource>();
        buttonSelectAudioSource.Play();

        Debug.Log("Destroying Tower");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void resetTower()
    {
        destroyTower();
        createTower();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.O))
        {
            if (TopBlockPlacer.PlayerChangeable == true)
            {
                TopBlockPlacer.PlayerChangeable = false;
                deselect();
                Player.changeActivePlayer();
                Debug.Log("Spieler " + (Player.ActivePlayer + 1) + " ist jetzt am Zug und hat " + Players[Player.ActivePlayer].Score + " Punkte");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            addRow();
        }

        maxRow = transform.childCount;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {

                if (hitInfo.transform.gameObject.tag == "Brick")
                {
                    rows = hitInfo.transform.parent.parent.childCount;
                    if (hitInfo.transform.gameObject != selectedObj && hitInfo.transform.parent.name != "Row#" + rows)
                    {
                        select(hitInfo.transform.gameObject);
                    }
                    else
                    {
                        deselect();
                    }

                }
            }
        }
    }

    /*void FixedUpdate()
    {

        if (selectedObj != null)
        {
            Rigidbody rigidbody = selectedObj.GetComponent<Rigidbody>();

            // as of now, only the cubes with the "+" material have this script, so only they move!
            Vector3 moveVector = new Vector3();
            // up
            if (Input.GetKey(KeyCode.Y))
            {
                //rigidbody.velocity = new Vector3(0, force, 0);
                moveVector.y = force;
            }

            // down
            if (Input.GetKey(KeyCode.X))
            {
                //rigidbody.velocity = new Vector3(0, -force, 0);
                moveVector.y = -force;
            }

            // forward
            if (Input.GetKey(KeyCode.W))
            {
                //rigidbody.velocity = new Vector3(0, 0, force);
                moveVector.z = force;
            }

            // backward
            if (Input.GetKey(KeyCode.S))
            {
                //rigidbody.velocity = new Vector3(0, 0, -force);
                moveVector.z = -force;
            }

            // left
            if (Input.GetKey(KeyCode.A))
            {
                //rigidbody.velocity = new Vector3(-force, 0, 0);
                moveVector.x = -force;
            }

            // right
            if (Input.GetKey(KeyCode.D))
            {
                //rigidbody.velocity = new Vector3(force, 0, 0);
                moveVector.x = force;
            }

            // if no force is applied, dont change velocity (so gravity can work)
            if (moveVector.sqrMagnitude > 0)
            {
                rigidbody.velocity = moveVector;
            }
        }
    }*/

    private void deselect()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Renderer>().material = wood;
            selectedObj.GetComponent<Renderer>().material.color = Color.white;
            selectedObj = null;
            firstSelected = null;
        }
    }



    public void select(GameObject gameObject)
    {

        // Sound effect tower destroyed
        destroyAudioSource = GetComponent<AudioSource>();
        destroyAudioSource.Play();


        if (selectedObj == null && firstSelected == null)
        {
            firstSelected = gameObject;
            selectedObj = gameObject;
            selectedObj.GetComponent<Renderer>().material.color = Color.green;
            changeRow(gameObject);
        }
        else if (gameObject == firstSelected)
        {
            deselect();
            selectedObj = gameObject;
            selectedObj.GetComponent<Renderer>().material.color = Color.green;
        }

    }

    public void changeRow(GameObject gameObject)
    {
        if (transform.GetChild(maxRow - 1).childCount == 3)
        {

            GameObject row = new GameObject();
            row.name = "Row#" + (maxRow + 1);
            row.transform.parent = this.transform;
            gameObject.transform.parent = row.transform;
            gameObject.name = "Row#" + (maxRow + 1) + "Block1";


        }
        else if (transform.GetChild(maxRow - 1).childCount == 1)
        {

            gameObject.transform.parent = transform.GetChild(maxRow - 1).transform;
            gameObject.name = "Row#" + (maxRow) + "Block2";

        }
        else if (transform.GetChild(maxRow - 1).childCount == 2)
        {

            gameObject.transform.parent = transform.GetChild(maxRow - 1).transform;
            gameObject.name = "Row#" + (maxRow) + "Block3";

        }
        else
        {
            Debug.Log("else");
        }
    }
}
