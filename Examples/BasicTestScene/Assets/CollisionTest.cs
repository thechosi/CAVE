using System.Collections;
using System.Collections.Generic;
using Cave;
using UnityEngine;
using UnityEngine.Networking;

public class CollisionTest : CollisionSynchronization {
    
    public CollisionTest()
        : base(new[] { Cave.EventType.OnCollisionEnter })
    {

    }
    
    public override void OnSynchronizedCollisionEnter(GameObject other)
    {
        Debug.Log(TimeSynchronizer.time + " " + GetComponent<NetworkIdentity>().netId + " - " + other.gameObject.name);
    }
}
