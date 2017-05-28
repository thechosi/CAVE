using System.Collections.Generic;
using UnityEngine;
using AwesomeSockets.Buffers;
using UnityEngine.Networking;

namespace Cave
{
    public enum CollisionType
    {
        OnCollisionEnter = 0,
        OnCollisionStay = 1,
        OnCollisionExit = 2,
        OnTriggerEnter = 3,
        OnTriggerStay = 4,
        OnTriggerExit = 5
    }

    public struct StoredCollision
    {
        public uint networkId;
        public uint otherNetworkId;
        public short type;
    }

    public class InputCollisionMessage : ISynchroMessage
    {
        public List<StoredCollision> collisions = new List<StoredCollision>();
        
        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, collisions.Count);
            foreach (StoredCollision collision in collisions)
            {
                Buffer.Add(buffer, collision.networkId);
                Buffer.Add(buffer, collision.otherNetworkId);
                Buffer.Add(buffer, collision.type);
            }
        }

        public void Deserialize(Buffer buffer)
        {
            int length = Buffer.Get<int>(buffer);
            for (int i = 0; i < length; i++)
            {
                StoredCollision collision = new StoredCollision();
                collision.networkId = Buffer.Get<uint>(buffer);
                collision.otherNetworkId = Buffer.Get<uint>(buffer);
                collision.type = Buffer.Get<short>(buffer);
                collisions.Add(collision);
            }
        }

        public int GetLength()
        {
            return (sizeof(uint) * 2 + sizeof(short)) * collisions.Count + sizeof(uint);
        }
    }

    class CollisionSynchronizer
    {
        private static List<StoredCollision> collisions = new List<StoredCollision>();

        private static void CallScripts(List<StoredCollision> collisions)
        {
            Dictionary<NetworkInstanceId, NetworkIdentity> networkIdentities = NodeInformation.type.Equals("master") ? NetworkServer.objects : ClientScene.objects;

            foreach (StoredCollision collision in collisions)
            {
                NetworkInstanceId networkInstanceId = new NetworkInstanceId(collision.networkId);
                if (networkIdentities.ContainsKey(networkInstanceId))
                {
                    CollisionSynchronization collisionSynchronization = networkIdentities[networkInstanceId].GetComponent<CollisionSynchronization>();
                    NetworkInstanceId otherNetworkInstanceId = new NetworkInstanceId(collision.otherNetworkId);

                    if (collisionSynchronization != null && networkIdentities.ContainsKey(otherNetworkInstanceId))
                    {
                        if ((CollisionType)collision.type == CollisionType.OnCollisionEnter)
                            collisionSynchronization.OnSynchronizedCollisionEnter(networkIdentities[otherNetworkInstanceId].gameObject);
                        else if ((CollisionType)collision.type == CollisionType.OnCollisionStay)
                            collisionSynchronization.OnSynchronizedCollisionStay(networkIdentities[otherNetworkInstanceId].gameObject);
                        else if ((CollisionType)collision.type == CollisionType.OnCollisionExit)
                            collisionSynchronization.OnSynchronizedCollisionExit(networkIdentities[otherNetworkInstanceId].gameObject);
                        else if ((CollisionType)collision.type == CollisionType.OnTriggerEnter)
                            collisionSynchronization.OnSynchronizedTriggerEnter(networkIdentities[otherNetworkInstanceId].gameObject);
                        else if ((CollisionType)collision.type == CollisionType.OnTriggerStay)
                            collisionSynchronization.OnSynchronizedTriggerStay(networkIdentities[otherNetworkInstanceId].gameObject);
                        else if ((CollisionType)collision.type == CollisionType.OnTriggerExit)
                            collisionSynchronization.OnSynchronizedTriggerExit(networkIdentities[otherNetworkInstanceId].gameObject);
                    }
                }
            }
        }

        public static void ProcessMessage(InputCollisionMessage message)
        {
            CallScripts(message.collisions);
        }

        public static void BuildMessage(InputCollisionMessage message)
        {
            message.collisions = collisions;
            CallScripts(message.collisions);
            collisions = new List<StoredCollision>();
        }

        public static void LogCollision(CollisionType type, NetworkInstanceId netId, NetworkInstanceId otherNetId)
        {
            StoredCollision collision;
            collision.networkId = netId.Value;
            collision.otherNetworkId = otherNetId.Value;
            collision.type = (short)type;
            collisions.Add(collision);
        }
    }
}
