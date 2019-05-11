using System;
using System.Xml;

namespace BR.AN.PviServices
{
	[CLSCompliant(false)]
	public class ScalingPoint
	{
		private Value xValue;

		private Value yValue;

		public Value XValue => xValue;

		public Value YValue => yValue;

		public ScalingPoint(Value xValue, Value yValue)
		{
			this.xValue = xValue;
			this.yValue = yValue;
		}

		internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			writer.WriteAttributeString("XValue", xValue);
			writer.WriteAttributeString("YValue", yValue);
			return 0;
		}

		public int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, ScalingPoint point)
		{
			if (point == null)
			{
				return -1;
			}
			double result = 0.0;
			string attribute = reader.GetAttribute("XValue");
			if (attribute != null && attribute.Length > 0 && PviParse.TryParseDouble(attribute, out result))
			{
				point.xValue = result;
			}
			attribute = reader.GetAttribute("YValue");
			if (attribute != null && attribute.Length > 0 && PviParse.TryParseDouble(attribute, out result))
			{
				point.yValue = result;
			}
			return 0;
		}
	}
}
