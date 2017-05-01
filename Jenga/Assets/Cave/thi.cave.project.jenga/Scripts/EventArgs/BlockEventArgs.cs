using UnityEngine;
using System.Collections;
using System;

public delegate void BlockEventHandler(object sender,BlockEventArgs e);

public class BlockEventArgs
{
    public BlockManager Manager { get; private set; }
    public GameObject gameObject
    {
        get { return Manager.gameObject; }
    }
    public Transform transform
    {
        get { return Manager.transform; }
    }

    public BlockEventArgs(BlockManager blockManager)
    {
        Manager = blockManager;
    }
}
