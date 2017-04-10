using UnityClusterPackage;
using UnityEngine;
using UnityEngine.Networking;

public class SyncParticle : MonoBehaviour
{
    NetworkManager nm;
    NetworkClient client;

    public void Start()
    {
        GameObject go = GameObject.Find("NodeManager");
        nm = go.GetComponent<NetworkManager>();

        if (NodeInformation.type.Equals("slave"))
        {
            client = nm.client;
            client.RegisterHandler(MyMsgType.ParticleTime, OnParticleTime);
        }
    }

    public void Update()
    {
        if (NodeInformation.type.Equals("master"))
        {
            if (Input.GetKeyDown("o"))
            {
                SendParticleInfo(GetSeed(), GetTime());
                ParticleSystem p = GetParticleSystem();
            }
        }
    }

    private ParticleSystem GetParticleSystem()
    {
        return GetComponent<ParticleSystem>();
    }

    private float GetTime()
    {
        ParticleSystem p = GetParticleSystem();
        //Debug.Log(p.time);
        return p.time;
    }

    private uint GetSeed()
    {
        ParticleSystem p = GetParticleSystem();
        //Debug.Log(p.randomSeed);
        return p.randomSeed;
    }

    public class MyMsgType
    {
        public static short ParticleTime = MsgType.Highest + 1;
    };

    public class ParticleTimeMessage : MessageBase
    {
        public float time;
        public uint seed;
    }

    public void SendParticleInfo(uint seed, float time)
    {
        ParticleTimeMessage msg = new ParticleTimeMessage();
        msg.time = time;
        msg.seed = seed;

        NetworkServer.SendToAll(MyMsgType.ParticleTime, msg);
    }

    public void OnParticleTime(NetworkMessage netMsg)
    {
        ParticleSystem p = GetComponent<ParticleSystem>();
        ParticleTimeMessage msg = netMsg.ReadMessage<ParticleTimeMessage>();
        Debug.Log("current playback time " + p.time);
        Debug.Log("received playback time " + msg.time);
        //p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //p.randomSeed = msg.seed;
        p.Simulate(msg.time);
        //p.time = msg.time;
        //p.Clear();
        p.Play();
    }
}