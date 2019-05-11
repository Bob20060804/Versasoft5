namespace BR.AN.PviServices
{
	public class DerivationBase
	{
		private string propName;

		private DataType propBasicType;

		protected DerivationBase propDerivedFrom;

		public string Name => propName;

		public DataType DataType => propBasicType;

		public DerivationBase DerivedFrom => propDerivedFrom;

		internal DerivationBase(string typeName, DataType basicType)
		{
			propName = typeName;
			propBasicType = basicType;
			propDerivedFrom = null;
		}

		internal void SetDerivation(DerivationBase derivation)
		{
			propDerivedFrom = derivation;
		}

		public virtual string ToPviString()
		{
			if (DerivationPath() != null && 0 < DerivationPath().Length)
			{
				return "VS=" + DerivationParameters() + " TN=" + DerivationPath();
			}
			return "VS=" + DerivationParameters();
		}

		public string DerivationPath()
		{
			if (propDerivedFrom == null)
			{
				return propName;
			}
			return propName + "," + propDerivedFrom.DerivationPath();
		}

		public virtual string DerivationParameters()
		{
			if (propDerivedFrom == null)
			{
				return "v";
			}
			return "v;" + propDerivedFrom.DerivationParameters();
		}

		public override string ToString()
		{
			return ToPviString();
		}
	}
}
