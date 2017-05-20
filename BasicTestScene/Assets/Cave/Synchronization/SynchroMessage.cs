using AwesomeSockets.Buffers;
using UnityEngine;

namespace Cave
{

    public interface ISynchroMessage
    {
        void Serialize(Buffer buffer);
        void Deserialize(Buffer buffer);

        int GetLength();
    }

    public class InputMessage : ISynchroMessage
    {
        public float deltaTime;
        public float particleDeltaTime;
        public float time;
        public float axisHorizontal;
        public float axisVertical;
        public float animatorTime;
        public Vector3 cameraPosition = new Vector3();
        public Vector3 flyStickPosition = new Vector3();
        public Vector3 flyStickRotation = new Vector3();
        public bool objectsSpawned;

        private void SerializeVector3(Buffer buffer, Vector3 vector)
        {
            Buffer.Add(buffer, vector.x);
            Buffer.Add(buffer, vector.y);
            Buffer.Add(buffer, vector.z);
        }

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, deltaTime);
            Buffer.Add(buffer, particleDeltaTime);
            Buffer.Add(buffer, time);
            Buffer.Add(buffer, axisHorizontal);
            Buffer.Add(buffer, axisVertical);
            Buffer.Add(buffer, animatorTime);
            SerializeVector3(buffer, cameraPosition);
            SerializeVector3(buffer, flyStickPosition);
            SerializeVector3(buffer, flyStickRotation);
            Buffer.Add(buffer, objectsSpawned);
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
            deltaTime = Buffer.Get<float>(buffer);
            particleDeltaTime = Buffer.Get<float>(buffer);
            time = Buffer.Get<float>(buffer);
            axisHorizontal = Buffer.Get<float>(buffer);
            axisVertical = Buffer.Get<float>(buffer);
            animatorTime = Buffer.Get<float>(buffer);
            cameraPosition = DeserializeVector3(buffer);
            flyStickPosition = DeserializeVector3(buffer);
            flyStickRotation = DeserializeVector3(buffer);
            objectsSpawned = Buffer.Get<bool>(buffer);
        }

        public int GetLength()
        {
            return sizeof(float) * (6 + 3 * 3) + sizeof(bool);
        }
    }

    public class FinishMessage : ISynchroMessage
    {
        public bool finished = true;

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, finished);
        }

        public void Deserialize(Buffer buffer)
        {
            finished = Buffer.Get<bool>(buffer);
        }

        public int GetLength()
        {
            return sizeof(bool);
        }
    }

    public enum SynchroMessageType
    {
        SetParticleSeed
    }

    public class EventMessage : ISynchroMessage
    {
        public SynchroMessageType type;
        public uint data;

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, (int) type);
            Buffer.Add(buffer, data);
        }

        public void Deserialize(Buffer buffer)
        {
            type = (SynchroMessageType) Buffer.Get<int>(buffer);
            data = Buffer.Get<uint>(buffer);
        }

        public int GetLength()
        {
            return sizeof(int) + sizeof(uint);
        }
    }
}
