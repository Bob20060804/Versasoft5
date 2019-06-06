namespace Ersa.Global.Controls.Converters.UniversalConverter
{
	public abstract class EDC_UniversalToBoolConverter<TValue> : EDC_UniversalConverter<TValue, bool>
	{
		protected EDC_UniversalToBoolConverter()
		{
			base.PRO_objTrueWert = true;
			base.PRO_objFalseWert = false;
		}
	}
}
