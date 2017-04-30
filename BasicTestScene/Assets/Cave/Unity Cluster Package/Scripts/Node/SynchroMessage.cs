using AwesomeSockets.Buffers;
using UnityEngine;

namespace UnityClusterPackage
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

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, deltaTime);
            Buffer.Add(buffer, particleDeltaTime);
            Buffer.Add(buffer, time);
            Buffer.Add(buffer, axisHorizontal);
            Buffer.Add(buffer, axisVertical);
            Buffer.Add(buffer, animatorTime);
        }

        public void Deserialize(Buffer buffer)
        {
            deltaTime = Buffer.Get<float>(buffer);
            particleDeltaTime = Buffer.Get<float>(buffer);
            time = Buffer.Get<float>(buffer);
            axisHorizontal = Buffer.Get<float>(buffer);
            axisVertical = Buffer.Get<float>(buffer);
            animatorTime = Buffer.Get<float>(buffer);
        }

        public int GetLength()
        {
            return sizeof(float) * 6;
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
