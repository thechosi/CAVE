using System;
using UnityEngine;

namespace Cave
{
    class InputSynchronizer
    {
        private static float axisHorizontal;
        private static float axisVertical;

        public static void ProcessMessage(InputMessage message)
        {
            axisHorizontal = message.axisHorizontal;
            axisVertical = message.axisVertical;
        }

        public static void BuildMessage(InputMessage message)
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
