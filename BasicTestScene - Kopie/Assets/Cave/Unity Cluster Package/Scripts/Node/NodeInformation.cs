using UnityEngine;
using System;
using System.IO;
using System.Xml;

namespace UnityClusterPackage {

	static class NodeInformation
	{
		private static XmlDocument xmlDocument;
		
		public static string name, type, serverIp;
		public static int id, nodes, serverPort;
		public static bool stereo;
		public static string eye;		
		public static float paX, paY, paZ, pbX, pbY, pbZ, pcX, pcY, pcZ, peX, peY, peZ;
		
		static NodeInformation()
		{
			xmlDocument = new XmlDocument();

			string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "node-config.xml");
			string contentXml = "";

			if ( Application.platform == RuntimePlatform.Android ) {

				WWW fileReaderAndroid = new WWW(filePath);
				while ( !fileReaderAndroid.isDone ) {}
				
				contentXml = fileReaderAndroid.text;

			} else {
				contentXml = System.IO.File.ReadAllText(filePath);
			}
			
			xmlDocument.LoadXml(contentXml);

			ReadNodeInformation();            
		}

		static void ReadNodeInformation()
		{
			
			XmlNodeReader xmlNodeReader = new XmlNodeReader(xmlDocument);
			
			while (xmlNodeReader.Read())
			{
				if (xmlNodeReader.NodeType == XmlNodeType.Element)
				{

					try {

						switch (xmlNodeReader.Name)
						{
							
							case "node":
								type = xmlNodeReader.GetAttribute("type");
								name = xmlNodeReader.GetAttribute("name");
								id = Convert.ToInt32(xmlNodeReader.GetAttribute("id"));
								
								if (type.Equals("master"))
								{
									nodes = Convert.ToInt32(xmlNodeReader.GetAttribute("nodes"));
								}
								
								break;
								
							case "server":
								serverIp = xmlNodeReader.GetAttribute("ip");
								serverPort = Convert.ToInt32(xmlNodeReader.GetAttribute("port"));
								break;
								
							case "screen":
								if (xmlNodeReader.GetAttribute("stereo").Equals("true"))
								{
									stereo = true;
									eye = xmlNodeReader.GetAttribute("eye");
								}
								else
								{
									stereo = false;
									eye = "stereo false";
								}
								break;
								
							case "pa":
								Single.TryParse(xmlNodeReader.GetAttribute("x"), out paX);
								Single.TryParse(xmlNodeReader.GetAttribute("y"), out paY);
								Single.TryParse(xmlNodeReader.GetAttribute("z"), out paZ);
								break;
								
							case "pb":
								Single.TryParse(xmlNodeReader.GetAttribute("x"), out pbX);
								Single.TryParse(xmlNodeReader.GetAttribute("y"), out pbY);
								Single.TryParse(xmlNodeReader.GetAttribute("z"), out pbZ);
								break;
								
							case "pc":
								Single.TryParse(xmlNodeReader.GetAttribute("x"), out pcX);
								Single.TryParse(xmlNodeReader.GetAttribute("y"), out pcY);
								Single.TryParse(xmlNodeReader.GetAttribute("z"), out pcZ);
								break;
								
							case "pe":
								Single.TryParse(xmlNodeReader.GetAttribute("x"), out peX);
								Single.TryParse(xmlNodeReader.GetAttribute("y"), out peY);
								Single.TryParse(xmlNodeReader.GetAttribute("z"), out peZ);
								break;
						}

					} catch ( Exception ex ) {
						Debug.Log( "Configuration file <node-config> parsing error: " + ex );
					}
				}
			}
		}
    }
}