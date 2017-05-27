using UnityEngine;
using AwesomeSockets.Buffers;

namespace Cave
{
    public class InputInputMessage : ISynchroMessage
    {
        public float axisHorizontal;
        public float axisVertical;

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, axisHorizontal);
            Buffer.Add(buffer, axisVertical);
        }

        public void Deserialize(Buffer buffer)
        {
            axisHorizontal = Buffer.Get<float>(buffer);
            axisVertical = Buffer.Get<float>(buffer);
        }

        public int GetLength()
        {
            return sizeof(float) * 2;
        }
    }

    class InputSynchronizer
    {
        private static float axisHorizontal;
        private static float axisVertical;

        public static void ProcessMessage(InputInputMessage message)
        {
            axisHorizontal = message.axisHorizontal;
            axisVertical = message.axisVertical;
        }

        public static void BuildMessage(InputInputMessage message)
        {
            axisHorizontal = Input.GetAxis("Horizontal");
            axisVertical = Input.GetAxis("Vertical");
            message.axisHorizontal = axisHorizontal;
            message.axisVertical = axisVertical;
        }
        

        public static float GetAxis(string orientation)
        {
            if (orientation == "Vertical")
                return axisVertical;
            else
                return axisHorizontal;
        }

    }
}
