using UnityEngine;
using System.Collections;
using System;

public class TopBlockPlacer : MonoBehaviour
{
    public GameObject blockModel, setupBlockModel, targetRow;

    public event BlockEventHandler BlockPlaced;
    public event EventHandler Destroyed;
    private int count;

    void Start()
    {
        for (int x = -1; x <= 1; x++)
        {
            var block = (GameObject)GameObject.Instantiate(setupBlockModel);
            block.transform.parent = transform;
            block.name = "SetupBlock";
            block.transform.localRotation = Quaternion.Euler(0, 90, 0);
            block.transform.localPosition = new Vector3(x * setupBlockModel.GetComponent<Renderer>().bounds.size.z, setupBlockModel.GetComponent<Renderer>().bounds.size.y, 0);
            var component = block.AddComponent<SetupBlockBehaviour>();
            component.BlockPlaced += HandleBlockPlaced;
            component.blockModel = blockModel;
            component.targetRow = targetRow;
        }
        targetRow.transform.position = new Vector3(transform.position.x, transform.position.y + GetComponent<Renderer>().bounds.size.y, transform.position.z);
        targetRow.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRow.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        count = GetComponentsInChildren<SetupBlockBehaviour>().Length;
    }

    void OnEnable()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    void HandleBlockPlaced(object sender, BlockEventArgs e)
    {
        if (BlockPlaced != null)
            BlockPlaced(this, e);
        count--;

        if (count == 0)
        {
            if (Destroyed != null)
                Destroyed(this, new EventArgs());
            Destroy(GetComponent<TopBlockPlacer>());
        }
    }
}
