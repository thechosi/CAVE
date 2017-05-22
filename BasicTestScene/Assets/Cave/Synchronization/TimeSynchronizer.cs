using System;
using UnityEngine;

namespace Cave
{
    class TimeSynchronizer
    {

        public static float deltaTime;
        public static float time;

        public static void BuildMessage(InputMessage message)
        {
            deltaTime = Time.deltaTime;
            time = Time.time;
            message.deltaTime = deltaTime;
            message.time = time;
        }

        public static void ProcessMessage(InputMessage message)
        {
            deltaTime = message.deltaTime;
            time = message.time;
        }
    }
}
