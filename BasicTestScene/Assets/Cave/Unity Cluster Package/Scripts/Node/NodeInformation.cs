using UnityEngine;
using System;
using System.Xml;

namespace UnityClusterPackage
{

    static class NodeInformation
    {
        private static XmlDocument xmlDocument;

        public static string name, type, serverIp;
        public static Vector3 originPosition;
        public static Vector3 cameraRoation;
        public static Vector3 screenplanePosition;
        public static Vector3 screenplaneRotation;
        public static Vector3 screenplaneScale;
        public static string cameraEye;
        public static int nodes, serverPort;
        public static bool stereo;
        public static int numberOfSlaves;

        static NodeInformation()
        {
            xmlDocument = new XmlDocument();

            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "node-config.xml");
            string contentXml = "";
            numberOfSlaves = 0;

            if (Application.platform == RuntimePlatform.Android)
            {

                WWW fileReaderAndroid = new WWW(filePath);
                while (!fileReaderAndroid.isDone) { }

                contentXml = fileReaderAndroid.text;

            }
            else
            {
                contentXml = System.IO.File.ReadAllText(filePath);
            }

            xmlDocument.LoadXml(contentXml);

            ReadNodeInformation();
        }

        static Vector3 getRotationOfNode(XmlNode node)
        {
            Vector3 rotation = new Vector3();

            foreach (XmlNode rotation_node in node.ChildNodes)
            {

                if (rotation_node.Name == "rotation")
                {

                    rotation = getXYZOfNode(rotation_node);
                }
            }

            return rotation;
            
        }

        static Vector3 getPositionOfNode(XmlNode node)
        {
            Vector3 position = new Vector3();

            foreach (XmlNode position_node in node.ChildNodes)
            {

                if (position_node.Name == "position")
                {

                    position = getXYZOfNode(position_node);
                }
            }

            return position;

        }

        static Vector3 getScaleOfNode(XmlNode node)
        {
            Vector3 scale = new Vector3();

            foreach (XmlNode scale_node in node.ChildNodes)
            {

                if (scale_node.Name == "scale")
                {

                    scale = getXYZOfNode(scale_node);
                }
            }

            return scale;

        }

        static Vector3 getXYZOfNode(XmlNode node)
        {
            float x, y, z;
            Vector3 v3 = new Vector3();

            float.TryParse(node.Attributes["x"].Value, out x);
            float.TryParse(node.Attributes["y"].Value, out y);
            float.TryParse(node.Attributes["z"].Value, out z);

            v3 = new Vector3(x, y, z);

            return v3;


        }

        static void getServerInfosOfNode(XmlNode node)
        {
            Debug.Log(node.Name);
            if (node.Name == "master")
            {
                serverIp = node.Attributes["ip"].Value;
                serverPort = Convert.ToInt32(node.Attributes["port"].Value);
            }

        }

        static void getScreenplane(XmlNode node)
        {
            screenplanePosition = getPositionOfNode(node);
            screenplaneRotation = getRotationOfNode(node);
            screenplaneScale= getScaleOfNode(node);
        }

        static void ReadNodeInformation()
        {

            XmlNode node_Config = xmlDocument.GetElementsByTagName("config")[0];

            foreach (XmlNode node in node_Config.ChildNodes)
            {
                switch (node.Name)
                {
                    case "master":
                        getServerInfosOfNode(node);
                        if (Network.player.ipAddress == node.Attributes["ip"].Value || node.Attributes["ip"].Value == "localhost")
                        {
                            type = "master";
                            Debug.Log("Wir sind master");


                            foreach (XmlNode master_node in node.ChildNodes)
                            {
                                switch (master_node.Name)
                                {
                                    case "origin":
                                        originPosition = getPositionOfNode(master_node);
                                        break;

                                    case "screenplane":
                                        getScreenplane(master_node);
                                        break;
                                }
                            }
                        }
                        break;

                    case "slave":
                        numberOfSlaves = numberOfSlaves + 1;
                        if (Network.player.ipAddress == node.Attributes["ip"].Value || node.Attributes["ip"].Value == "localhost")
                        {

                            type = "slave";

                            foreach (XmlNode slave_node in node.ChildNodes)
                            {
                                switch (slave_node.Name)
                                {
                                    case "camera":
                                        cameraRoation = getRotationOfNode(slave_node);
                                        cameraEye = slave_node.Attributes["eye"].Value;
                                        break;

                                    case "screenplane":
                                        getScreenplane(slave_node);
                                        break;
                                }
                            }
                        }
                        break;
                }

            }


        }
    }
}