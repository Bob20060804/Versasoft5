using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
	public class ServiceCollection : BaseCollection
	{
		private TraceWriter propTracer;

		public static Hashtable Services = new Hashtable();

		private LogicalCollection propLogicalObjects;

		private LogicalObjectsUsage propLogicalUsage;

		public TraceWriter Trace
		{
			get
			{
				if (propTracer == null)
				{
					propTracer = new TraceWriter();
				}
				return propTracer;
			}
		}

		public LogicalCollection LogicalObjects => propLogicalObjects;

		public LogicalObjectsUsage LogicalObjectsUsage
		{
			get
			{
				return propLogicalUsage;
			}
			set
			{
				propLogicalUsage = value;
			}
		}

		public Service this[string name]
		{
			get
			{
				if (propCollectionType == CollectionType.HashTable)
				{
					return (Service)base[name];
				}
				return null;
			}
		}

		public ServiceCollection()
		{
			propCollectionType = CollectionType.HashTable;
			propLogicalObjects = new LogicalCollection(this, "Logicals");
			LogicalObjectsUsage = LogicalObjectsUsage.FullName;
		}

		public int LoadConfiguration(string fileName)
		{
			ConfigurationFlags configurationFlags = ConfigurationFlags.None;
			configurationFlags |= ConfigurationFlags.ConnectionState;
			configurationFlags |= ConfigurationFlags.ActiveState;
			configurationFlags |= ConfigurationFlags.RefreshTime;
			configurationFlags |= ConfigurationFlags.VariableMembers;
			configurationFlags |= ConfigurationFlags.Values;
			configurationFlags |= ConfigurationFlags.Scope;
			configurationFlags |= ConfigurationFlags.IOAttributes;
			return LoadConfiguration(fileName, configurationFlags);
		}

		protected int LoadConfiguration(StreamReader stream, ConfigurationFlags flags)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(stream);
			xmlTextReader.MoveToContent();
			if (string.Compare(xmlTextReader.Name, "Services") == 0)
			{
				string attribute = xmlTextReader.GetAttribute("LogicalUsage");
				if (attribute != null)
				{
					if (attribute.Equals("None"))
					{
						propLogicalUsage = LogicalObjectsUsage.None;
					}
					if (attribute.Equals("FullName"))
					{
						propLogicalUsage = LogicalObjectsUsage.FullName;
					}
					if (attribute.Equals("ObjectName"))
					{
						propLogicalUsage = LogicalObjectsUsage.ObjectName;
					}
					if (attribute.Equals("ObjectNameWithType"))
					{
						propLogicalUsage = LogicalObjectsUsage.ObjectNameWithType;
					}
				}
			}
			xmlTextReader.Read();
			while (xmlTextReader.NodeType != XmlNodeType.EndElement)
			{
				if (string.Compare(xmlTextReader.Name, "Service") != 0)
				{
					continue;
				}
				string attribute2 = xmlTextReader.GetAttribute("Name");
				if (!ContainsKey(attribute2))
				{
					Service service = new Service(attribute2, this);
					service.LoadConfiguration(xmlTextReader, flags);
					if (Contains(service))
					{
						Add(service);
					}
					xmlTextReader.Read();
				}
				else
				{
					xmlTextReader.Skip();
				}
			}
			xmlTextReader.Close();
			return 0;
		}

		public int LoadConfiguration(StreamReader stream)
		{
			ConfigurationFlags configurationFlags = ConfigurationFlags.None;
			configurationFlags |= ConfigurationFlags.ConnectionState;
			configurationFlags |= ConfigurationFlags.ActiveState;
			configurationFlags |= ConfigurationFlags.RefreshTime;
			configurationFlags |= ConfigurationFlags.VariableMembers;
			configurationFlags |= ConfigurationFlags.Values;
			configurationFlags |= ConfigurationFlags.Scope;
			configurationFlags |= ConfigurationFlags.IOAttributes;
			return LoadConfiguration(stream, configurationFlags);
		}

		protected int LoadConfiguration(string fileName, ConfigurationFlags flags)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(fileName);
			xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
			xmlTextReader.MoveToContent();
			if (string.Compare(xmlTextReader.Name, "Services") == 0)
			{
				string attribute = xmlTextReader.GetAttribute("LogicalUsage");
				if (attribute != null)
				{
					if (attribute.Equals("None"))
					{
						propLogicalUsage = LogicalObjectsUsage.None;
					}
					if (attribute.Equals("FullName"))
					{
						propLogicalUsage = LogicalObjectsUsage.FullName;
					}
					if (attribute.Equals("ObjectName"))
					{
						propLogicalUsage = LogicalObjectsUsage.ObjectName;
					}
					if (attribute.Equals("ObjectNameWithType"))
					{
						propLogicalUsage = LogicalObjectsUsage.ObjectNameWithType;
					}
				}
			}
			xmlTextReader.Read();
			while (xmlTextReader.NodeType != XmlNodeType.EndElement)
			{
				if (string.Compare(xmlTextReader.Name, "Service") == 0)
				{
					string attribute2 = xmlTextReader.GetAttribute("Name");
					if (!ContainsKey(attribute2))
					{
						Service service = new Service(attribute2, this);
						service.LoadConfiguration(xmlTextReader, flags);
						if (Contains(service))
						{
							Add(service);
						}
						if (!xmlTextReader.Read())
						{
							break;
						}
					}
					else
					{
						xmlTextReader.Skip();
					}
				}
				else
				{
					xmlTextReader.Read();
				}
			}
			xmlTextReader.Close();
			return 0;
		}

		public int SaveConfiguration(string fileName)
		{
			ConfigurationFlags configurationFlags = ConfigurationFlags.None;
			configurationFlags |= ConfigurationFlags.ConnectionState;
			configurationFlags |= ConfigurationFlags.ActiveState;
			configurationFlags |= ConfigurationFlags.RefreshTime;
			configurationFlags |= ConfigurationFlags.VariableMembers;
			configurationFlags |= ConfigurationFlags.Values;
			configurationFlags |= ConfigurationFlags.Scope;
			configurationFlags |= ConfigurationFlags.IOAttributes;
			return SaveConfiguration(fileName, configurationFlags);
		}

		public int SaveConfiguration(string fileName, ConfigurationFlags flags)
		{
			int result = 0;
			FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			XmlTextWriter writer = new XmlTextWriter(fileStream, Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			writer.WriteRaw("<?PviServices Version=1.0?>");
			writer.WriteStartElement("Services");
			writer.WriteAttributeString("LogicalUsage", propLogicalUsage.ToString());
			if (0 < Count)
			{
				foreach (Service value in Values)
				{
					value.ToXMLTextWriter(ref writer, flags);
				}
			}
			writer.WriteEndElement();
			writer.Close();
			fileStream.Close();
			return result;
		}

		public void Add(Service service)
		{
			service.Services = this;
			service.LogicalObjectsUsage = LogicalObjectsUsage;
			base.Add(service.Name, service);
		}

		public override void Connect()
		{
			if (0 < Count)
			{
				foreach (Service value in Values)
				{
					value.Connected += ServiceConnected;
					value.Error += ServiceError;
					value.Connect();
				}
			}
		}

		public void Disconnect()
		{
			propCounter = 0;
			propErrorCount = 0;
			if (0 < Count)
			{
				foreach (Service value in Values)
				{
					value.Disconnect();
					value.Disconnected += ServiceDisconnected;
				}
			}
		}

		private void ServiceConnected(object sender, PviEventArgs e)
		{
			propCounter++;
			((Service)sender).Connected -= ServiceConnected;
			int propCounter = propCounter;
			int count = Count;
		}

		private void ServiceDisconnected(object sender, PviEventArgs e)
		{
			propCounter++;
			((Service)sender).Disconnected -= ServiceDisconnected;
			int propCounter = propCounter;
			int count = Count;
		}

		private void ServiceError(object sender, PviEventArgs e)
		{
			propErrorCount++;
			((Service)sender).Error -= ServiceError;
		}

		internal bool ContainsLogicalObject(string logical)
		{
			if (propLogicalObjects == null)
			{
				return false;
			}
			return propLogicalObjects.ContainsKey(logical);
		}

		internal int AddLogicalObject(string logical, object obj)
		{
			int result = 0;
			if (propLogicalObjects == null)
			{
				propLogicalObjects = new LogicalCollection(this, "Logicals");
			}
			propLogicalObjects.Add(logical, obj);
			return result;
		}

		internal void RemoveLogicalObject(string logical)
		{
			if (propLogicalObjects != null)
			{
				propLogicalObjects.Remove(logical);
			}
		}
	}
}
