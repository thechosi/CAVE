using UnityEngine;
using System.Collections;
using System;

public class SetupBlockBehaviour : MonoBehaviour
{
    public GameObject blockModel, targetRow;

    public event BlockEventHandler BlockPlaced;

    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        if (GameManager.instance.State == GameState.PLAYING)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == GetComponent<Collider>())
                {
                    GetComponent<Renderer>().enabled = true;
                    if (Input.GetMouseButtonDown(0))
                    {
                        var block = (GameObject)Instantiate(blockModel);
                        block.transform.parent = targetRow.transform;
                        var z = transform.localPosition.x;
                        if (transform.parent.parent.name.EndsWith("(90)"))
                        {
                            z = -z;
                        }
                        block.transform.localPosition = new Vector3(0, 0, z);
                        block.transform.localRotation = Quaternion.identity;
                        if (BlockPlaced != null)
                            BlockPlaced(this, new BlockEventArgs(block.GetComponent<BlockManager>()));
                        Destroy(gameObject);
                    }
                } else
                {
                    GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }
}
