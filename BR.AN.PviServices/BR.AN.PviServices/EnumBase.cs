namespace BR.AN.PviServices
{
	public abstract class EnumBase
	{
		protected string propName;

		protected object propValue;

		public string Name => propName;

		public virtual object Value => propValue;

		internal EnumBase(string name, object value)
		{
			propName = name;
			propValue = value;
		}

		internal abstract void SetEnumValue(object value);

		public virtual string ToPviString()
		{
			return "VS=e," + propValue.ToString() + "," + propName;
		}
	}
}
