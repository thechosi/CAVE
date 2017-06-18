using UnityEngine;
using AwesomeSockets.Buffers;

namespace Cave
{

    public class InputTrackingMessage : ISynchroMessage
    {
        public Vector3 cameraPosition = new Vector3();
        public Vector3 flyStickPosition = new Vector3();
        public Vector3 flyStickRotation = new Vector3();

        private void SerializeVector3(Buffer buffer, Vector3 vector)
        {
            Buffer.Add(buffer, vector.x);
            Buffer.Add(buffer, vector.y);
            Buffer.Add(buffer, vector.z);
        }

        public void Serialize(Buffer buffer)
        {
            SerializeVector3(buffer, cameraPosition);
            SerializeVector3(buffer, flyStickPosition);
            SerializeVector3(buffer, flyStickRotation);
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
            cameraPosition = DeserializeVector3(buffer);
            flyStickPosition = DeserializeVector3(buffer);
            flyStickRotation = DeserializeVector3(buffer);
        }

        public int GetLength()
        {
            return sizeof(float) * 3 * 3;
        }
    }

    class TrackingSynchronizer
    {
        public static void ProcessMessage(InputTrackingMessage message)
        {
            GameObject origin = GameObject.FindGameObjectWithTag("MainCamera");
            if (origin != null)
            {
                origin.transform.Find("CameraHolder").transform.localPosition = message.cameraPosition;
                origin.transform.Find("Flystick").transform.localEulerAngles = message.flyStickRotation;
                origin.transform.Find("Flystick").transform.localPosition = message.flyStickPosition;
            }
        }

        public static void BuildMessage(InputTrackingMessage message)
        {
            GameObject origin = GameObject.FindGameObjectWithTag("MainCamera");
            message.cameraPosition = origin.transform.Find("CameraHolder").transform.localPosition;
            message.flyStickRotation = origin.transform.Find("Flystick").transform.localEulerAngles;
            message.flyStickPosition = origin.transform.Find("Flystick").transform.localPosition;
        }

    }
}
