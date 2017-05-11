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
                origin.transform.Find("CameraHolder").transform.position = message.cameraPosition;
                origin.transform.Find("Flystick").transform.rotation = Quaternion.Euler(message.flyStickRotation);
                origin.transform.Find("Flystick").transform.position = message.flyStickPosition;
            }
        }

        public static void BuildMessage(InputMessage message)
        {
            GameObject origin = GameObject.FindGameObjectWithTag("MainCamera");
            message.cameraPosition = origin.transform.Find("CameraHolder").transform.position;
            message.flyStickRotation = origin.transform.Find("Flystick").transform.rotation.eulerAngles;
            message.flyStickPosition = origin.transform.Find("Flystick").transform.position;
        }

    }
}
