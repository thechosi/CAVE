using UnityEngine;
using System.Collections;
using System;

public delegate void TowerStateChangedEventHandler(object sender,TowerStateChangedEventArgs e);

public class TowerStateChangedEventArgs : EventArgs
{
    public TowerManager.TowerState state { get; private set; }
    public bool isWaiting { get; private set; }
    public TowerStateChangedEventArgs(TowerManager.TowerState state, bool isWaiting)
    {
        this.state = state;
        this.isWaiting = isWaiting;
    }
}