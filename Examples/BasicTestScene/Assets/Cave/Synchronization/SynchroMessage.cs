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
        public InputTimeMessage inputTimeMessage = new InputTimeMessage();
        public InputParticleMessage inputParticleMessage = new InputParticleMessage();
        public InputInputMessage inputInputMessage = new InputInputMessage();
        public InputAnimatorMessage inputAnimatorMessage = new InputAnimatorMessage();
        public InputTrackingMessage inputTrackingMessage = new InputTrackingMessage();
        public InputRigidBodyMessage inputRigidBodyMessage = new InputRigidBodyMessage();
        public InputTransformationMessage inputTransformationMessage = new InputTransformationMessage();
        public InputCollisionMessage inputCollisionMessage = new InputCollisionMessage();

        public void Serialize(Buffer buffer)
        {
            inputTimeMessage.Serialize(buffer);
            inputParticleMessage.Serialize(buffer);
            inputInputMessage.Serialize(buffer);
            inputAnimatorMessage.Serialize(buffer);
            inputTrackingMessage.Serialize(buffer);
            inputRigidBodyMessage.Serialize(buffer);
            inputTransformationMessage.Serialize(buffer);
            inputCollisionMessage.Serialize(buffer);
        }

        public void Deserialize(Buffer buffer)
        {
            inputTimeMessage.Deserialize(buffer);
            inputParticleMessage.Deserialize(buffer);
            inputInputMessage.Deserialize(buffer);
            inputAnimatorMessage.Deserialize(buffer);
            inputTrackingMessage.Deserialize(buffer);
            inputRigidBodyMessage.Deserialize(buffer);
            inputTransformationMessage.Deserialize(buffer);
            inputCollisionMessage.Deserialize(buffer);
        }

        public int GetLength()
        {
            return inputTimeMessage.GetLength() + inputParticleMessage.GetLength() + inputInputMessage.GetLength() + inputAnimatorMessage.GetLength() + inputTrackingMessage.GetLength() + inputRigidBodyMessage.GetLength() + inputTransformationMessage.GetLength() + inputCollisionMessage.GetLength();
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
