using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TowerInteractivity : MonoBehaviour
{

    private GameObject selectedObj = null;

    public float force;

    private int rows;

    private Material wood;

    public GameObject brick;

    public int diffBetweenBlocks;

    public int nrOfRows;



    // Use this for initialization
    void Start()
    {
        if (diffBetweenBlocks <0)
        {
            diffBetweenBlocks = 0;
        }
        if (nrOfRows < 1)
        {
            nrOfRows = 1;
        }
        createTower();
        destroyTower();
        wood = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
    }

    public void createTower()
    {
        GameObject newBrick;


        newBrick = Instantiate(brick) as GameObject;
        float height = newBrick.GetComponent<Renderer>().bounds.size.y;
        Destroy(newBrick);


        for (int i = 0; i < nrOfRows; i++)
        {
            GameObject row = new GameObject();
            row.name = "Row#" + (i + 1);
            row.transform.parent = this.transform;

            float absolutDiff = (height + diffBetweenBlocks) * i;
            
            if (i % 2 == 1)
            {
                newBrick = Instantiate(brick, new Vector3(1, absolutDiff, 0), Quaternion.identity) as GameObject;
                newBrick.transform.parent = row.transform;
                newBrick = Instantiate(brick, new Vector3(1, absolutDiff, 1), Quaternion.identity) as GameObject;
                newBrick.transform.parent = row.transform;
                newBrick = Instantiate(brick, new Vector3(1, absolutDiff, 2), Quaternion.identity) as GameObject;
                newBrick.transform.parent = row.transform;
            }
            else
            {
                newBrick = Instantiate(brick, new Vector3(0, absolutDiff, 1), Quaternion.identity) as GameObject;
                newBrick.transform.Rotate(new Vector3(0, 90, 0));
                newBrick.transform.parent = row.transform;
                newBrick = Instantiate(brick, new Vector3(1, absolutDiff, 1), Quaternion.identity) as GameObject;
                newBrick.transform.Rotate(new Vector3(0, 90, 0));
                newBrick.transform.parent = row.transform;
                newBrick = Instantiate(brick, new Vector3(2, absolutDiff, 1), Quaternion.identity) as GameObject;
                newBrick.transform.Rotate(new Vector3(0, 90, 0));
                newBrick.transform.parent = row.transform;
            }
        }
    }


    public void destroyTower()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

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

    void FixedUpdate()
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


    }




    private void deselect()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Renderer>().material = wood;
            selectedObj = null;
        }
    }

    private void select(GameObject gameObject)
    {
        deselect();
        selectedObj = gameObject;
        selectedObj.GetComponent<Renderer>().material.color = Color.green;
    }
}
