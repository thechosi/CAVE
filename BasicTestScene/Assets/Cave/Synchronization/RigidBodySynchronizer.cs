using AwesomeSockets.Domain.Sockets;
using UnityEngine;
using UnityEngine.Networking;

namespace Cave
{
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
                if (rigidbody.gameObject.GetComponent<NetworkIdentity>() != null)
                {
                    rigidbody.gameObject.AddComponent<NetworkTransform>();
                }
            }
        }

        private static void InitializeNetworkTransforms()
        {
            NetworkTransform[] networkTransforms =
                Resources.FindObjectsOfTypeAll(typeof(NetworkTransform)) as NetworkTransform[];
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
                Destroy(rigidbody);
            }
        }

        public static void ProcessMessage(InputMessage message)
        {
            if (message.objectsSpawned)
                framesToSearchForSpawnedObjects = 60;

            if (framesToSearchForSpawnedObjects > 0)
            {
                Rigidbody[] rigidbodies = Resources.FindObjectsOfTypeAll(typeof(Rigidbody)) as Rigidbody[];
                foreach (Rigidbody rigidbody in rigidbodies)
                {
                    Destroy(rigidbody);
                }
                framesToSearchForSpawnedObjects--;
            }
        }

        public static void BuildMessage(InputMessage message)
        {
            message.objectsSpawned = objectsSpawned;
            objectsSpawned = false;
        }

    }

}