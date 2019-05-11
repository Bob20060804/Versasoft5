using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
	[CLSCompliant(false)]
	public class ScalingPointCollection : ArrayList
	{
		internal DataType propUserDataType;

		internal int propUserTypeLength;

		internal Variable propParent;

		public ScalingPointCollection()
		{
			propUserDataType = DataType.Unknown;
		}

		public virtual void Add(ScalingPoint scalingPoint)
		{
			if (Count == 0)
			{
				if (scalingPoint.YValue.DataType == DataType.String || scalingPoint.YValue.DataType == DataType.Boolean || scalingPoint.YValue.DataType == DataType.DT || scalingPoint.YValue.DataType == DataType.DateTime || scalingPoint.YValue.DataType == DataType.Date || scalingPoint.YValue.DataType == DataType.TOD || scalingPoint.YValue.DataType == DataType.TimeOfDay || scalingPoint.YValue.DataType == DataType.TimeSpan)
				{
					throw new InvalidOperationException();
				}
				if (scalingPoint.YValue.DataType == DataType.Single)
				{
					propUserDataType = DataType.Double;
					propUserTypeLength = 8;
				}
				else
				{
					propUserDataType = scalingPoint.YValue.DataType;
					propUserTypeLength = scalingPoint.YValue.TypeLength;
				}
			}
			Add((object)scalingPoint);
		}

		internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int num = 0;
			if (Count > 0)
			{
				writer.WriteStartElement("ScalingPoints");
				for (int i = 0; i < Count; i++)
				{
					writer.WriteStartElement("ScalingPoint" + i.ToString());
					num = ((ScalingPoint)this[i]).ToXMLTextWriter(ref writer, flags);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			return 0;
		}

		public int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, ScalingPointCollection pointCollection)
		{
			ScalingPoint scalingPoint = new ScalingPoint(0, 0);
			while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
			{
				scalingPoint.FromXmlTextReader(ref reader, flags, scalingPoint);
				pointCollection.Add(scalingPoint);
			}
			reader.Read();
			return 0;
		}
	}
}
