using System;
using System.Xml;

namespace BR.AN.PviServices
{
	[CLSCompliant(false)]
	public class Scaling
	{
		internal ScalingType propScalingType;

		private ScalingPointCollection propScalingPoints;

		internal Value propFactor;

		internal Value propMinValue;

		internal Value propMaxValue;

		internal ScalingType ScalingType => propScalingType;

		public ScalingPointCollection ScalingPoints
		{
			get
			{
				return propScalingPoints;
			}
			set
			{
				propScalingType = ScalingType.ScalingPoints;
				propScalingPoints = value;
			}
		}

		public Value Factor
		{
			get
			{
				return propFactor;
			}
			set
			{
				propFactor = value;
				if (propScalingType == ScalingType.LimitValues)
				{
					propScalingType = ScalingType.LimitValuesAndFactor;
				}
				else if (propScalingType == ScalingType.LimitValuesAndFactor)
				{
					propScalingType = ScalingType.LimitValuesAndFactor;
				}
				else
				{
					propScalingType = ScalingType.Factor;
				}
				ScalingPointCollection scalingPointCollection = new ScalingPointCollection();
				scalingPointCollection.Add(new ScalingPoint(propMinValue.ToDouble(null), propMinValue.ToDouble(null) * propFactor));
				scalingPointCollection.Add(new ScalingPoint(propMaxValue.ToDouble(null), propMaxValue.ToDouble(null) * propFactor));
				propScalingPoints = scalingPointCollection;
			}
		}

		public Value MinValue
		{
			get
			{
				return propMinValue;
			}
			set
			{
				propMinValue = value;
				if (propScalingType == ScalingType.Factor)
				{
					propScalingType = ScalingType.LimitValuesAndFactor;
				}
				else if (propScalingType == ScalingType.LimitValuesAndFactor)
				{
					propScalingType = ScalingType.LimitValuesAndFactor;
				}
				else
				{
					propScalingType = ScalingType.LimitValues;
				}
			}
		}

		public Value MaxValue
		{
			get
			{
				return propMaxValue;
			}
			set
			{
				propMaxValue = value;
				if (propScalingType == ScalingType.Factor)
				{
					propScalingType = ScalingType.LimitValuesAndFactor;
				}
				else if (propScalingType == ScalingType.LimitValuesAndFactor)
				{
					propScalingType = ScalingType.LimitValuesAndFactor;
				}
				else
				{
					propScalingType = ScalingType.LimitValues;
				}
			}
		}

		public Scaling()
		{
			Init();
			Factor = 1;
			propScalingType = ScalingType.Factor;
		}

		public Scaling(Value factor)
		{
			Init();
			Factor = factor;
			propScalingType = ScalingType.Factor;
		}

		public Scaling(Value minValue, Value maxValue)
		{
			propMinValue = minValue;
			propMaxValue = maxValue;
			Factor = 1;
			propScalingType = ScalingType.LimitValues;
		}

		public Scaling(Value minValue, Value maxValue, Value factor)
		{
			propMinValue = minValue;
			propMaxValue = maxValue;
			Factor = factor;
			propScalingType = ScalingType.LimitValuesAndFactor;
		}

		public Scaling(ScalingPointCollection scalingPoints)
		{
			Init();
			propScalingPoints = scalingPoints;
			propScalingType = ScalingType.ScalingPoints;
		}

		internal void Init()
		{
			propMaxValue = int.MaxValue;
			propMinValue = int.MinValue;
		}

		internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = 0;
			int num = 0;
			if (propFactor != 1)
			{
				writer.WriteAttributeString("Factor", propFactor.ToString());
			}
			if (propMinValue != int.MinValue)
			{
				writer.WriteAttributeString("MinValue", propMinValue.ToString());
			}
			if (propMaxValue != int.MaxValue)
			{
				writer.WriteAttributeString("MaxValue", propMaxValue.ToString());
			}
			if (propScalingType != ScalingType.ScalingPoints)
			{
				writer.WriteAttributeString("ScalingType", propScalingType.ToString());
			}
			num = propScalingPoints.ToXMLTextWriter(ref writer, flags);
			if (num != 0)
			{
				result = num;
			}
			return result;
		}

		public int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Scaling tmpScaling)
		{
			string text = "";
			text = reader.GetAttribute("Factor");
			if (text != null && text.Length > 0)
			{
				tmpScaling.propFactor = text;
			}
			text = "";
			text = reader.GetAttribute("MinValue");
			if (text != null && text.Length > 0)
			{
				tmpScaling.propMinValue = text;
			}
			text = "";
			text = reader.GetAttribute("MaxValue");
			if (text != null && text.Length > 0)
			{
				tmpScaling.propMaxValue = text;
			}
			text = "";
			text = reader.GetAttribute("ScalingType");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "callback":
					tmpScaling.propScalingType = ScalingType.Callback;
					break;
				case "factor":
					tmpScaling.propScalingType = ScalingType.Factor;
					break;
				case "limitvalues":
					tmpScaling.propScalingType = ScalingType.LimitValues;
					break;
				case "limitvaluesandfactor":
					tmpScaling.propScalingType = ScalingType.LimitValuesAndFactor;
					break;
				case "scalingpoints":
					tmpScaling.propScalingType = ScalingType.ScalingPoints;
					break;
				}
			}
			reader.Read();
			if (reader.Name == "ScalingPoints")
			{
				if (tmpScaling.ScalingPoints == null)
				{
					tmpScaling.ScalingPoints = new ScalingPointCollection();
				}
				tmpScaling.ScalingPoints.FromXmlTextReader(ref reader, flags, tmpScaling.ScalingPoints);
			}
			return 0;
		}
	}
}
