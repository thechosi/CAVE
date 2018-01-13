using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Cave
{
    public class ConfigurationNode
    {
        public Vector3 originPosition;
        public Vector3 cameraRoation;
        public Vector3 screenplanePosition;
        public Vector3 screenplaneRotation;
        public Vector3 screenplaneScale;
        public Transform screenPlane;
        public string cameraEye;
        public string ip;
        public int port;
    }

    static class NodeInformation
    {
        private static XmlDocument xmlDocument;
        
        public static ConfigurationNode master;
        public static List<ConfigurationNode> slaves;
        public static ConfigurationNode own;
        public static bool developmentMode;


        public static void Load()
        {
            slaves = new List<ConfigurationNode>();

            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "node-config.xml");
            string contentXml = System.IO.File.ReadAllText(filePath);

            xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(contentXml);

            readNodeInformation();
        }

        private static void readNodeInformation()
        {
            XmlNode node_Config = xmlDocument.GetElementsByTagName("config")[0];
            foreach (XmlNode node in node_Config.ChildNodes)
            {
                if (node.Name.Equals("slave") || node.Name.Equals("master"))
                {
                    ConfigurationNode configurationNode = new ConfigurationNode();
                    readConnectionInfos(node, configurationNode);

                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case "origin":
                                readOrigin(childNode, configurationNode);
                                break;

                            case "camera":
                                readCamera(childNode, configurationNode);
                                break;

                            case "screenplane":
                                readScreenplane(childNode, configurationNode);
                                break;
                        }
                    }

                    switch (node.Name)
                    {
                        case "master":
                            master = configurationNode;
                            break;
                        case "slave":
                            slaves.Add(configurationNode);
                            break;
                    }
                }
            }

            if (developmentMode || isOwnIP(master.ip))
            {
                own = master;
            }
            else
            {
                foreach (ConfigurationNode slave in slaves)
                {
                    if (isOwnIP(slave.ip))
                        own = slave;
                }
            }

            if (own == null)
                throw new Exception("Own configuration node could not be found.");
            else
                Debug.Log("Config loaded (" + (master != null ? "1" : "0") + " master, " + slaves.Count + " slaves, own type: " + (isMaster() ? "master" : "slave") + (developmentMode ? ", Development mode!" : "") + ")");
        }

        private static void readConnectionInfos(XmlNode node, ConfigurationNode configurationNode)
        {
            configurationNode.ip = node.Attributes["ip"].Value;
            if (node.Attributes["port"] != null)
                configurationNode.port = Convert.ToInt32(node.Attributes["port"].Value);
        }

        private static void readScreenplane(XmlNode node, ConfigurationNode configurationNode)
        {
            configurationNode.screenplanePosition = readVector(node, "position");
            configurationNode.screenplaneRotation = readVector(node, "rotation");
            configurationNode.screenplaneScale = readVector(node, "scale");
        }

        private static void readOrigin(XmlNode node, ConfigurationNode configurationNode)
        {
            configurationNode.originPosition = readVector(node, "position");
        }

        private static void readCamera(XmlNode node, ConfigurationNode configurationNode)
        {
            configurationNode.cameraRoation = readVector(node, "rotation");
            configurationNode.cameraEye = node.Attributes["eye"].Value;
        }

        private static Vector3 readVector(XmlNode node, string childNodeName)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == childNodeName)
                {
                    float x, y, z;
                    Vector3 vec = new Vector3();

                    float.TryParse(childNode.Attributes["x"].Value, out x);
                    float.TryParse(childNode.Attributes["y"].Value, out y);
                    float.TryParse(childNode.Attributes["z"].Value, out z);

                    vec = new Vector3(x, y, z);

                    return vec;
                }
            }
            throw new Exception("No child node with name " + childNodeName + " found.");
        }

        private static bool isOwnIP(String ip)
        {
            if (ip.Equals("localhost"))
                return true;

            string dnsName = System.Net.Dns.GetHostName();
            if (dnsName == "")
                dnsName = "localhost";
            var host = System.Net.Dns.GetHostEntry(dnsName);
			foreach (var ownIp in host.AddressList)
			{
				if (ownIp.ToString().Equals(ip))
				{
					return true;
				}
			}
			return false;
		}

        public static bool isMaster()
        {
            return master == own;
        }

        private static void SpawnScreenplane(Transform parent, ConfigurationNode configurationNode)
        {
            GameObject screenPlane = new GameObject(configurationNode.ip);
            screenPlane.transform.parent = parent;
            screenPlane.transform.localScale = configurationNode.screenplaneScale;
            screenPlane.transform.eulerAngles = configurationNode.screenplaneRotation;
            screenPlane.transform.localPosition = configurationNode.screenplanePosition;
            configurationNode.screenPlane = screenPlane.transform;
        }

        public static void SpawnScreenplanes(Transform parent)
        {
            SpawnScreenplane(parent, master);
            foreach (ConfigurationNode slave in slaves)
            {
                SpawnScreenplane(parent, slave);
            }
        }
    }
}
