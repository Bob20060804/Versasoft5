using System;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters.UniversalConverter
{
	public abstract class EDC_UniversalConverter<TValue, TResult> : IValueConverter
	{
		public TResult PRO_objTrueWert
		{
			get;
			set;
		}

		public TResult PRO_objFalseWert
		{
			get;
			set;
		}

		public bool PRO_blnIstInvertiert
		{
			get;
			set;
		}

		protected abstract Func<TValue, bool> PRO_delPruefung
		{
			get;
		}

		protected virtual Func<bool, TValue> PRO_delBackPruefung
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (i_objValue is TValue)
			{
				TValue arg = (TValue)i_objValue;
				return (PRO_delPruefung(arg) ^ PRO_blnIstInvertiert) ? PRO_objTrueWert : PRO_objFalseWert;
			}
			if (PRO_blnIstInvertiert)
			{
				return PRO_objFalseWert;
			}
			return PRO_objTrueWert;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			if (i_objValue is bool)
			{
				bool arg = (bool)i_objValue ^ PRO_blnIstInvertiert;
				return PRO_delBackPruefung(arg);
			}
			return Binding.DoNothing;
		}
	}
}
