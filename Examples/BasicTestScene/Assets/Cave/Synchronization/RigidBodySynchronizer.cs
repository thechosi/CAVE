using AwesomeSockets.Domain.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using AwesomeSockets.Buffers;

namespace Cave
{
    public class InputRigidBodyMessage : ISynchroMessage
    {
        public bool objectsSpawned;

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, objectsSpawned);
        }

        public void Deserialize(Buffer buffer)
        {
            objectsSpawned = Buffer.Get<bool>(buffer);
        }

        public int GetLength()
        {
            return sizeof(bool) * 1;
        }
    }

    public class RigidBodySynchronizer : MonoBehaviour
    {
        private static bool objectsSpawned;
        private static int framesToSearchForSpawnedObjects;

        public static void Spawn(GameObject gameObject)
        {
            if (gameObject.GetComponent<NetworkTransform>() != null)
                InitializeNetworkTransform(gameObject.GetComponent<NetworkTransform>());
            NetworkServer.Spawn(gameObject);
            objectsSpawned = true;
        }

        public static void Prepare()
        {
            Rigidbody[] rigidbodies = Resources.FindObjectsOfTypeAll(typeof(Rigidbody)) as Rigidbody[];
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                if (rigidbody.gameObject.GetComponent<NetworkIdentity>() != null && rigidbody.gameObject.tag == "NetworkTransformSynchro")
                {
                    rigidbody.gameObject.AddComponent<NetworkTransform>();
                }
            }
        }

        private static void InitializeNetworkTransforms()
        {
            NetworkTransform[] networkTransforms = Resources.FindObjectsOfTypeAll(typeof(NetworkTransform)) as NetworkTransform[];
            foreach (NetworkTransform networkTransform in networkTransforms)
            {
                InitializeNetworkTransform(networkTransform);
            }
        }

        private static void InitializeNetworkTransform(NetworkTransform networkTransform)
        {
            networkTransform.interpolateMovement = 0;
            networkTransform.sendInterval = 0.01f;
            networkTransform.movementTheshold = 0.0001f;
            networkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
        }

        public static void InitializeFromServer(Server server, ISocket client)
        {
            InitializeNetworkTransforms();
        }

        public static void InitializeFromClient(Client client)
        {
            InitializeNetworkTransforms();
            Rigidbody[] rigidbodies = Resources.FindObjectsOfTypeAll(typeof(Rigidbody)) as Rigidbody[];
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                if (rigidbody.gameObject.GetComponent<NetworkTransform>() != null)
                    Destroy(rigidbody);
            }
        }

        public static void ProcessMessage(InputRigidBodyMessage message)
        {
            if (message.objectsSpawned)
                framesToSearchForSpawnedObjects = 60;

            if (framesToSearchForSpawnedObjects > 0)
            {
                Rigidbody[] rigidbodies = Resources.FindObjectsOfTypeAll(typeof(Rigidbody)) as Rigidbody[];
                foreach (Rigidbody rigidbody in rigidbodies)
                {
                    if (rigidbody.gameObject.GetComponent<NetworkTransform>() != null)
                        Destroy(rigidbody);
                }
                framesToSearchForSpawnedObjects--;
            }
        }

        public static void BuildMessage(InputRigidBodyMessage message)
        {
            message.objectsSpawned = objectsSpawned;
            objectsSpawned = false;
        }

    }

}