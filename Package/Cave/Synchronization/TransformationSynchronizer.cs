using System.Collections.Generic;
using UnityEngine;
using AwesomeSockets.Buffers;
using UnityEngine.Networking;

namespace Cave
{
    public struct StoredTransform
    {
        public uint networkId;
        public Vector3 localPosition;
        public Vector3 localEulerAngles;
        public Vector3 localScale;
    }

    public class InputTransformationMessage : ISynchroMessage
    {
        public List<StoredTransform> transforms = new List<StoredTransform>();

        private void SerializeVector3(Buffer buffer, Vector3 vector)
        {
            Buffer.Add(buffer, vector.x);
            Buffer.Add(buffer, vector.y);
            Buffer.Add(buffer, vector.z);
        }

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, transforms.Count);
            foreach (StoredTransform transform in transforms)
            {
                Buffer.Add(buffer, transform.networkId);
                SerializeVector3(buffer, transform.localPosition);
                SerializeVector3(buffer, transform.localEulerAngles);
                SerializeVector3(buffer, transform.localScale);
            }
        }

        private Vector3 DeserializeVector3(Buffer buffer)
        {
            Vector3 vector = new Vector3();
            vector.x = Buffer.Get<float>(buffer);
            vector.y = Buffer.Get<float>(buffer);
            vector.z = Buffer.Get<float>(buffer);
            return vector;
        }

        public void Deserialize(Buffer buffer)
        {
            int length = Buffer.Get<int>(buffer);
            for (int i = 0; i < length; i++)
            {
                StoredTransform transform = new StoredTransform();
                transform.networkId = Buffer.Get<uint>(buffer);
                transform.localPosition = DeserializeVector3(buffer);
                transform.localEulerAngles = DeserializeVector3(buffer);
                transform.localScale = DeserializeVector3(buffer);
                transforms.Add(transform);
            }
        }

        public int GetLength()
        {
            return (sizeof(float) * 3 * 3 + sizeof(int)) * transforms.Count + sizeof(uint);
        }
    }

    class TransformationSynchronizer
    {
        public static void ProcessMessage(InputTransformationMessage message)
        {
            Dictionary<NetworkInstanceId, NetworkIdentity> networkIdentities = ClientScene.objects;

            foreach (StoredTransform transform in message.transforms)
            {
                NetworkInstanceId networkInstanceId = new NetworkInstanceId(transform.networkId);
                if (networkIdentities.ContainsKey(networkInstanceId))
                {
                    networkIdentities[networkInstanceId].transform.localPosition = transform.localPosition;
                    networkIdentities[networkInstanceId].transform.localEulerAngles = transform.localEulerAngles;
                    networkIdentities[networkInstanceId].transform.localScale = transform.localScale;
                }
            }
        }

        public static void BuildMessage(InputTransformationMessage message)
        {
            foreach (KeyValuePair<NetworkInstanceId, NetworkIdentity> networkIdentity in NetworkServer.objects)
            {
				if (networkIdentity.Value.gameObject.GetComponent<Rigidbody>() != null || networkIdentity.Value.gameObject.GetComponent<ForceSynchronization>() != null)
                {
                    StoredTransform transform = new StoredTransform();
                    transform.networkId = networkIdentity.Key.Value;
                    transform.localPosition = networkIdentity.Value.gameObject.transform.localPosition;
                    transform.localEulerAngles = networkIdentity.Value.gameObject.transform.localEulerAngles;
                    transform.localScale = networkIdentity.Value.gameObject.transform.localScale;
                    message.transforms.Add(transform);
                }
            }
        }

    }
}
