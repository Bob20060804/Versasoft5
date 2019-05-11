using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
	public class AppInfo
	{
		public string Version
		{
			get;
			set;
		}

		public string OsVersion
		{
			get;
			set;
		}

		public string ConfigurationId
		{
			get;
			set;
		}

		public string ProjectVersion
		{
			get;
			set;
		}

		public AppInfo(string xmlData)
		{
			XmlReader xmlReader = null;
			try
			{
				xmlReader = XmlReader.Create(new StringReader(xmlData));
				xmlReader.MoveToContent();
				do
				{
					string name;
					if (xmlReader.NodeType == XmlNodeType.Element && (name = xmlReader.Name) != null && name == "ApplicationInformation")
					{
						string attribute = xmlReader.GetAttribute("ProjectId");
						if (!string.IsNullOrEmpty(attribute))
						{
							Version = attribute;
						}
						attribute = xmlReader.GetAttribute("AutomationRuntime");
						if (!string.IsNullOrEmpty(attribute))
						{
							OsVersion = attribute;
						}
						attribute = xmlReader.GetAttribute("ProjectName");
						if (!string.IsNullOrEmpty(attribute))
						{
							ConfigurationId = attribute;
						}
						attribute = xmlReader.GetAttribute("ProjectVersion");
						if (!string.IsNullOrEmpty(attribute))
						{
							ProjectVersion = attribute;
						}
					}
				}
				while (xmlReader.Read());
			}
			catch
			{
			}
			finally
			{
				xmlReader?.Close();
			}
		}

		public override string ToString()
		{
			string str = "<ApplicationInformation";
			if (!string.IsNullOrEmpty(Version))
			{
				str = str + " Version=\"" + Version + "\"";
			}
			if (!string.IsNullOrEmpty(OsVersion))
			{
				str = str + " OsVersion=\"" + OsVersion + "\"";
			}
			if (!string.IsNullOrEmpty(ConfigurationId))
			{
				str = str + " ConfigurationId=\"" + ConfigurationId + "\"";
			}
			if (!string.IsNullOrEmpty(ProjectVersion))
			{
				str = str + " ProjectVersion=\"" + ProjectVersion + "\"";
			}
			return str + " />";
		}
	}
}
