using System;
using UnityEngine;

namespace UnityClusterPackage
{
    class TrackingSynchronizer
    {
        public static void ProcessMessage(InputMessage message)
        {
            GameObject origin = GameObject.FindGameObjectWithTag("MainCamera");
            if (origin != null)
            {
                origin.transform.Find("CameraHolder").transform.localPosition = message.cameraPosition;
                origin.transform.Find("Flystick").transform.localEulerAngles = message.flyStickRotation;
                origin.transform.Find("Flystick").transform.localPosition = message.flyStickPosition;
            }
        }

        public static void BuildMessage(InputMessage message)
        {
            GameObject origin = GameObject.FindGameObjectWithTag("MainCamera");
            message.cameraPosition = origin.transform.Find("CameraHolder").transform.localPosition;
            message.flyStickRotation = origin.transform.Find("Flystick").transform.localEulerAngles;
            message.flyStickPosition = origin.transform.Find("Flystick").transform.localPosition;
        }

    }
}
