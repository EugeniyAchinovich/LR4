using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Text.Json;

namespace ConfigManager
{
	public class ConfigManager
	{
		public void ManageConfigs(string xmlFilePath = null, string jsonFilePath = null)
        {
            if (File.Exists(jsonFilePath))
            {
                dynamic jsonCFG = JsonSerializer.Deserialize<JsonConfig>(File.ReadAllText(jsonFilePath));

                sourceFolder = jsonCFG.sourceFolder;
                targetFolder = jsonCFG.targetFolder;
                archive = jsonCFG.archive;
                archiveDateOnly = jsonCFG.archiveDateOnly;
                needToArchive = jsonCFG.needToArchive;
                needToEncrypt = jsonCFG.needToEncrypt;
                cypherKey = jsonCFG.cypherKey;
				connectionString = connectionString;
            }
            if (File.Exists(xmlFilePath))
            {
                List<XMLConfig> xmlCFGs = new List<XMLConfig>();

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlFilePath);
                XmlElement xRoot = xDoc.DocumentElement;

                foreach (XmlElement xnode in xRoot)
                {
                    XMLConfig xmlCFG = new XMLConfig();
                    XmlNode attribute = xnode.Attributes.GetNamedItem("CONFIGS");

                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        switch (childnode.Name)
                        {
                            case "sourceFolder": sourceFolder = childnode.InnerText; break;
                            case "targetFolder": targetFolder = childnode.InnerText; break;
                            case "archive": archive = childnode.InnerText; break;
                            case "archiveDateOnly": archiveDateOnly = Boolean.Parse(childnode.InnerText); break;
                            case "needToArchive": needToArchive = Boolean.Parse(childnode.InnerText); break;
                            case "needToCompress": needToCompress = Boolean.Parse(childnode.InnerText); break;
                            case "needToEncrypt": needToEncrypt = Boolean.Parse(childnode.InnerText); break;
                            case "cypherKey": cypherKey = childnode.InnerText; break;
							case "connectionString": cypherKey = childnode.InnerText; break;
                        }
                    }
                }
            }
        }
		
		public ConfigManager() { }
	}
	
}