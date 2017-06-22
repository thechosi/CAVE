using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using Cave;
using UnityEngine.Events;
using UnityEngine.Networking;

public class TowerInteractivity : MonoBehaviour
{
    public enum State
    {
        BlockPlacing,
        BlockCorrectlyPlaced,
        TowerCrashed
    }

    public State state;
    InteractableItem interactingItem;
    public float gravity = 10;
    private GameObject selectedObj;
    private GameObject firstSelected;
    private int rows;
    public GameObject brick;
    public float diffBetweenBlocks;
    public int nrOfRows;
    private int nrOfPlayers;
    private List<Player> players = new List<Player>();
    private AudioSource destroyAudioSource;
    private AudioSource buttonSelectAudioSource;
    private static int maxRow;
    private Vector3 blockSize;
    private Camera cam;
    private FlyStickInteraction flyStickInteraction;

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

    public GameObject FirstSelected
    {
        get
        {
            return firstSelected;
        }

        set
        {
            firstSelected = value;
        }
    }

    public GameObject SelectedObj
    {
        get
        {
            return selectedObj;
        }

        set
        {
            selectedObj = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Physics.gravity = new Vector3(0, -gravity, 0);



        /*for (int i = 0; i < NrOfPlayers; i++)
        {
            Players.Add(ScriptableObject.CreateInstance<Player>());
            Players[i].Score = 0;
            Players[i].PlayerNumber = i;
        }
        Players[0].IsActive = true;*/

        if (diffBetweenBlocks < 0)
        {
            diffBetweenBlocks = 0;
        }
        //createTower();

        //destroyAudioSource = GetComponent<AudioSource>()[0];
        //buttonSelectAudioSource = GetComponent<AudioSource>()[1];

        state = State.BlockPlacing;
    }

    private void setSizes()
    {
        GameObject newBrick;
        newBrick = Instantiate(brick);
        blockSize = newBrick.GetComponent<Renderer>().bounds.size;
        Destroy(newBrick);
    }

    public void createTower()
    {
        if (NodeInformation.type.Equals("master"))
        {
            setSizes();

            for (int i = 0; i < nrOfRows; i++)
            {
                addRow();
            }

            Debug.Log("Tower created");
        }
		state = State.BlockPlacing;
    }

    public float GetHeight()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        float height = 0;
        foreach (GameObject brick in bricks)
        {
            if (height < brick.transform.position.y)
                height = brick.transform.position.y;
        }
        return height;
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
            newBrick.GetComponent<InteractableItem>().rotationFactor = 400;
            NetworkServer.Spawn(newBrick);

            newBrick = Instantiate(brick, new Vector3(blockSize.z, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block2";
            NetworkServer.Spawn(newBrick);

            newBrick = Instantiate(brick, new Vector3(blockSize.z, absolutDiff, 2 * blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block3";
            NetworkServer.Spawn(newBrick);
        }
        else
        {
            newBrick = Instantiate(brick, new Vector3(0, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.Rotate(new Vector3(0, 90, 0));
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block1";
            NetworkServer.Spawn(newBrick);

            newBrick = Instantiate(brick, new Vector3(blockSize.z, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.Rotate(new Vector3(0, 90, 0));
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block2";
            NetworkServer.Spawn(newBrick);

            newBrick = Instantiate(brick, new Vector3(2 * blockSize.z, absolutDiff, blockSize.z), Quaternion.identity) as GameObject;
            newBrick.transform.Rotate(new Vector3(0, 90, 0));
            newBrick.transform.parent = row.transform;
            newBrick.name = row.name + "Block3";
            NetworkServer.Spawn(newBrick);
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

		GameObject interactionObject = GameObject.Find("InteractionObject");
		interactionObject.transform.parent = null;

		deselect();
        Debug.Log("Destroying Tower");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // children aren't destroyed this frame! (but will be destroyed next frame)
        // to get the right childcount, we detach all Children
        transform.DetachChildren();
    }

    public void resetTower()
    {
        state = State.BlockPlacing;
        destroyTower();
        createTower();
    }

    // Update is called once per frame
    void Update()
    {
        if (flyStickInteraction == null)
            initFlyStickEvents();

        if (InputSynchronizer.GetFlyStickButtonDown(0))
        {
            grabBrick();
        }

        if (InputSynchronizer.GetFlyStickButtonUp(0))
        {
            if (interactingItem)
            {
                interactingItem.EndInteraction(flyStickInteraction);
                interactingItem = null;
            }
        }

        if (InputSynchronizer.GetKey("o") || InputSynchronizer.GetFlyStickButtonDown(1))
        {
            if (state == State.BlockCorrectlyPlaced)
            {
                state = State.BlockPlacing;
                deselect();
                Player.changeActivePlayer();
                Debug.Log("Player " + (Player.ActivePlayer + 1) + " is on the move and has " + Players[Player.ActivePlayer].Score + " Points");
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
            cam = GameObject.Find("Camera").GetComponent<Camera>();
            bool hit = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {

                if (hitInfo.transform.gameObject.tag == "Brick")
                {
                    rows = hitInfo.transform.parent.parent.childCount;
                    if (hitInfo.transform.gameObject != SelectedObj && hitInfo.transform.parent.name != "Row#" + rows)
                    {
                        select(hitInfo.transform.gameObject);
                        Debug.Log("select");
                    }
                    else
                    {
                        deselect();
                        Debug.Log("deselect");
                    }

                }
            }
        }
    }

    private void deselect()
    {
        if (SelectedObj != null)
        {
            //SelectedObj.GetComponent<Renderer>().material = wood;
            SelectedObj.GetComponent<Renderer>().material.color = Color.white;
            SelectedObj = null;
            FirstSelected = null;
        }
    }

    public void select(GameObject gameObject)
    {

        // Sound effect tower destroyed
        destroyAudioSource = GetComponent<AudioSource>();
        destroyAudioSource.Play();


        if (SelectedObj == null && FirstSelected == null)
        {
            FirstSelected = gameObject;
            SelectedObj = gameObject;
            SelectedObj.GetComponent<Renderer>().material.color = Color.green;
            if (NodeInformation.type.Equals("master"))
                changeRow(gameObject);
        }
        else if (gameObject == FirstSelected)
        {
            deselect();
            SelectedObj = gameObject;
            SelectedObj.GetComponent<Renderer>().material.color = Color.green;
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
    }

    public void initFlyStickEvents()
    {
        GameObject flyStick = GameObject.Find("Flystick");
        if (flyStick)
        {
            FlyStickInteraction flyStickInteractionLocal = flyStick.GetComponent<FlyStickInteraction>();
            if (flyStickInteractionLocal)
            {
                flyStickInteraction = flyStickInteractionLocal;
            }
        }
    }

    private void grabBrick()
    {
        // Check if flystick is inside a brick
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        GameObject selectedItem = null;
        foreach (GameObject brick in bricks)
        {
            if (brick.GetComponent<Collider>().bounds.Contains(flyStickInteraction.transform.position))
            {
                selectedItem = brick;
                break;
            }
        }

        // Check if flystick points on a brick
        if (selectedItem == null)
        {
            flyStickInteraction.sendRayForBlocks();
            selectedItem = flyStickInteraction.SelectedPart;
        }

        if (selectedItem)
        {
            interactingItem = selectedItem.GetComponent<InteractableItem>();
            if (interactingItem)
            {
                GameObject towerObject = GameObject.Find("DynamicTower");
                if (towerObject)
                {
                    TowerInteractivity tower = towerObject.GetComponent<TowerInteractivity>();

                    if (interactingItem.name.Contains(TowerInteractivity.MaxRow.ToString()) && interactingItem.GetComponent<Renderer>().material.color != Color.green)
                    {
                        //return;
                    }
                    if (interactingItem.GetComponent<Renderer>().material.color != Color.green)
                    {
                        if (tower.FirstSelected == null)
                        {
                            tower.select(interactingItem.transform.gameObject);
                            Debug.Log("select: " + interactingItem.transform.gameObject.name);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                if (interactingItem.isInteracting())
                {
                    interactingItem.EndInteraction(flyStickInteraction);
                    interactingItem = null;
                    Debug.Log("endInteraction");
                }
                interactingItem.BeginInteraction(flyStickInteraction);
            }
        }
    }
}
