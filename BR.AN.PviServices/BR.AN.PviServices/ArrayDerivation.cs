namespace BR.AN.PviServices
{
	public class ArrayDerivation : DerivationBase
	{
		private int propMinIndex;

		private int propMaxIndex;

		public int MinIndex => propMinIndex;

		public int MaxIndex => propMaxIndex;

		internal ArrayDerivation(string typeName, DataType basicType, ArrayDimension arrayDim)
			: base(typeName, basicType)
		{
			propMinIndex = arrayDim.StartIndex;
			propMaxIndex = arrayDim.EndIndex;
		}

		public override string DerivationParameters()
		{
			if (propDerivedFrom == null)
			{
				return "v;a," + propMinIndex.ToString() + "," + propMaxIndex.ToString();
			}
			return "v;a," + propMinIndex.ToString() + "," + propMaxIndex.ToString() + ";" + propDerivedFrom.DerivationParameters();
		}
	}
}
