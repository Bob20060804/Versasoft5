using System.Xml;

namespace BR.AN.PviServices
{
	public class MemoryInfo
	{
		public string Name
		{
			get;
			set;
		}

		public int Type
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

		public long MaxBlockSize
		{
			get;
			set;
		}

		public MemoryInfo(XmlReader xmlTReader)
		{
			bool flag = true;
			while (xmlTReader.AttributeCount == 0 && flag)
			{
				flag = xmlTReader.Read();
			}
			Name = xmlTReader.Name;
			string attribute = xmlTReader.GetAttribute("Type");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					Type = int.Parse(attribute);
				}
				catch
				{
				}
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
			attribute = xmlTReader.GetAttribute("MaxBlockSize");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					MaxBlockSize = long.Parse(attribute);
				}
				catch
				{
				}
			}
		}
	}
}
