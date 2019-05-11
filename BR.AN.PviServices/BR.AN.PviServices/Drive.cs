using System.Xml;

namespace BR.AN.PviServices
{
	public class Drive
	{
		public string DriveNumber
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}

		public long Size
		{
			get;
			set;
		}

		public long UsedSize
		{
			get;
			set;
		}

		public Drive(XmlReader xmlTReader)
		{
			bool flag = true;
			while (xmlTReader.AttributeCount == 0 && flag)
			{
				flag = xmlTReader.Read();
			}
			DriveNumber = xmlTReader.Name;
			string attribute = xmlTReader.GetAttribute("ID");
			if (!string.IsNullOrEmpty(attribute))
			{
				Id = attribute;
			}
			attribute = xmlTReader.GetAttribute("Name");
			if (!string.IsNullOrEmpty(attribute))
			{
				Name = attribute;
			}
			attribute = xmlTReader.GetAttribute("Size");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					Size = long.Parse(attribute);
				}
				catch
				{
				}
			}
			attribute = xmlTReader.GetAttribute("UsedSize");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					UsedSize = long.Parse(attribute);
				}
				catch
				{
				}
			}
		}
	}
}
