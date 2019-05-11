using System;

namespace BR.AN.PviServices
{
	public class Int32Enum : EnumBase
	{
		internal Int32Enum(params string[] values)
			: base(null, null)
		{
			if (2 < values.Length)
			{
				propName = values[2];
				propValue = Convert.ToInt32(values[1]);
			}
			else if (1 < values.Length)
			{
				propName = values[1];
			}
		}

		internal override void SetEnumValue(object value)
		{
			propValue = 1 + (int)value;
		}

		public override string ToString()
		{
			return "e," + propValue.ToString() + "," + propName;
		}
	}
}
