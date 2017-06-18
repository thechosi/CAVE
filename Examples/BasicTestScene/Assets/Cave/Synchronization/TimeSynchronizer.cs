using UnityEngine;
using AwesomeSockets.Buffers;

namespace Cave
{
    public class InputTimeMessage : ISynchroMessage
    {
        public float deltaTime;
        public float time;

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, deltaTime);
            Buffer.Add(buffer, time);
        }

        public void Deserialize(Buffer buffer)
        {
            deltaTime = Buffer.Get<float>(buffer);
            time = Buffer.Get<float>(buffer);
        }

        public int GetLength()
        {
            return sizeof(float) * 2;
        }
    }

    class TimeSynchronizer
    {

        public static float deltaTime;
        public static float time;

        public static void BuildMessage(InputTimeMessage message)
        {
            deltaTime = Time.deltaTime;
            time = Time.time;
            message.deltaTime = deltaTime;
            message.time = time;
        }

        public static void ProcessMessage(InputTimeMessage message)
        {
            deltaTime = message.deltaTime;
            time = message.time;
        }
    }

}
