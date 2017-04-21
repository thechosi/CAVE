using UnityEngine;
using System;
using System.IO;
using System.Xml;

namespace UnityClusterPackage
{

    static class NodeInformation
    {
        private static XmlDocument xmlDocument;

        public static string name, type, serverIp;
        public static Vector3 cameraRoation;
        public static String cameraEye;
        public static int id, nodes, serverPort;
        public static bool stereo;
        public static string eye;
        public static float paX, paY, paZ, pbX, pbY, pbZ, pcX, pcY, pcZ, peX, peY, peZ;

        static NodeInformation()
        {
            xmlDocument = new XmlDocument();

            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "node-config.xml");
            string contentXml = "";

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
            int x, y, z;
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

        static Vector3 getXYZOfNode(XmlNode node)
        {
            int x, y, z;
            Vector3 v3 = new Vector3();

            x = Convert.ToInt32(node.Attributes["x"].Value);
            y = Convert.ToInt32(node.Attributes["y"].Value);
            z = Convert.ToInt32(node.Attributes["z"].Value);

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
            Vector3 tempVec;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "pa":
                        tempVec = getXYZOfNode(childNode);
                        paX = tempVec.x;
                        paY = tempVec.y;
                        paZ = tempVec.z;
                        break;
                    case "pb":
                        tempVec = getXYZOfNode(childNode);
                        pbX = tempVec.x;
                        pbY = tempVec.y;
                        pbZ = tempVec.z;
                        break;
                    case "pc":
                        tempVec = getXYZOfNode(childNode);
                        pcX = tempVec.x;
                        pcY = tempVec.y;
                        pcZ = tempVec.z;
                        break;
                    case "pe":
                        tempVec = getXYZOfNode(childNode);
                        peX = tempVec.x;
                        peY = tempVec.y;
                        peZ = tempVec.z;
                        break;
                }
            }
        }

        static void ReadNodeInformation()
        {

            XmlNode node_Config = xmlDocument.GetElementsByTagName("config")[0];

            foreach (XmlNode node in node_Config.ChildNodes)
            {
                switch (node.Name)
                {
                    case "master":
                        if (Network.player.ipAddress == node.Attributes["ip"].Value || node.Attributes["ip"].Value == "localhost") { 
                            type = "master";
                        Debug.Log("Wir sind master");
                        getServerInfosOfNode(node);


                        foreach (XmlNode master_node in node.ChildNodes)
                        {
                            switch (master_node.Name)
                            {
                                case "relationToOrigin":
                                    cameraRoation = getRotationOfNode(master_node);
                                    break;

                                case "screenplane":
                                    getScreenplane(master_node);
                                    break;
                            }
                        }
                    }
                    break;

                    case "slave":
                        if (Network.player.ipAddress == node.Attributes["ip"].Value)
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