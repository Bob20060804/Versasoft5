using System;

namespace BR.AN.PviServices
{
	public class Int32MinMaxDerivation : DerivationBase
	{
		private int propMinimum;

		private int propMaximum;

		public int Minimum => propMinimum;

		public int Maximum => propMaximum;

		internal Int32MinMaxDerivation(string typeName, DataType basicType, params string[] values)
			: base(typeName, basicType)
		{
			propMinimum = int.MinValue;
			propMaximum = int.MaxValue;
			if (2 < values.Length)
			{
				propMinimum = Convert.ToInt32(values.GetValue(1));
				propMaximum = Convert.ToInt32(values.GetValue(2));
			}
		}

		public override string DerivationParameters()
		{
			if (propDerivedFrom == null)
			{
				return "v," + propMinimum.ToString() + "," + propMaximum.ToString();
			}
			return "v," + propMinimum.ToString() + "," + propMaximum.ToString() + ";" + propDerivedFrom.DerivationParameters();
		}
	}
}
