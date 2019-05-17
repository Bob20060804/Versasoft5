using Ersa.Global.Common.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ersa.Global.Controls.Validation
{
	public class EDC_ValidationMinMax : ValidationRule
	{
		private const int mC_i32DefaultNachkommastelle = 1;

		public EDC_Range PRO_edcRange
		{
			get;
			set;
		}

		public int PRO_i32NachkommaStellen
		{
			get;
			set;
		}

		public bool PRO_blnMinRangeVerletzung
		{
			get;
			set;
		}

		public bool PRO_blnMaxRangeVerletzung
		{
			get;
			set;
		}

		public string PRO_strMsgIllegaleZeichen
		{
			get;
			set;
		}

		public string PRO_strMsgRangeFehler
		{
			get;
			set;
		}

		public string PRO_strMsgParamIsNull
		{
			get;
			set;
		}

		public string PRO_strMsgParamIsEmpty
		{
			get;
			set;
		}

		public EDC_ValidationMinMax()
		{
			PRO_strMsgParamIsNull = "Parameter is null";
			PRO_strMsgParamIsEmpty = "Parameter is empty";
			PRO_strMsgRangeFehler = "Value = {0} not in range: min = {1} max = {2} ";
			PRO_strMsgIllegaleZeichen = "Illegal characters #{0}#";
			PRO_i32NachkommaStellen = 1;
		}

		public override ValidationResult Validate(object i_objValue, CultureInfo i_fdcCultureInfo)
		{
			PRO_blnMinRangeVerletzung = false;
			PRO_blnMaxRangeVerletzung = false;
			if (i_objValue == null)
			{
				return new ValidationResult(isValid: false, PRO_strMsgParamIsNull);
			}
			if (i_objValue is string)
			{
				if (string.IsNullOrWhiteSpace(i_objValue as string))
				{
					return new ValidationResult(isValid: false, PRO_strMsgParamIsEmpty);
				}
				if (i_objValue as string== "-")
				{
					return new ValidationResult(isValid: false, PRO_strMsgParamIsEmpty);
				}
			}
			double num;
			try
			{
				num = Convert.ToDouble(i_objValue, i_fdcCultureInfo);
			}
			catch (Exception)
			{
				return new ValidationResult(isValid: false, string.Format(PRO_strMsgIllegaleZeichen, i_objValue));
			}
			if (PRO_edcRange != null)
			{
				SUB_RangeBindingAktualisieren(EDC_Range.PRO_dblMaxProperty);
				SUB_RangeBindingAktualisieren(EDC_Range.PRO_dblMinProperty);
				if (num < PRO_edcRange.PRO_dblMin)
				{
					PRO_blnMinRangeVerletzung = true;
					return new ValidationResult(isValid: false, string.Format(PRO_strMsgRangeFehler, EDC_ZahlenFormatHelfer.FUN_strWert(num, PRO_i32NachkommaStellen), EDC_ZahlenFormatHelfer.FUN_strWert(PRO_edcRange.PRO_dblMin, PRO_i32NachkommaStellen), EDC_ZahlenFormatHelfer.FUN_strWert(PRO_edcRange.PRO_dblMax, PRO_i32NachkommaStellen)));
				}
				if (num > PRO_edcRange.PRO_dblMax)
				{
					PRO_blnMaxRangeVerletzung = true;
					return new ValidationResult(isValid: false, string.Format(PRO_strMsgRangeFehler, EDC_ZahlenFormatHelfer.FUN_strWert(num, PRO_i32NachkommaStellen), EDC_ZahlenFormatHelfer.FUN_strWert(PRO_edcRange.PRO_dblMin, PRO_i32NachkommaStellen), EDC_ZahlenFormatHelfer.FUN_strWert(PRO_edcRange.PRO_dblMax, PRO_i32NachkommaStellen)));
				}
			}
			return ValidationResult.ValidResult;
		}

		private void SUB_RangeBindingAktualisieren(DependencyProperty i_fdcDependencyProperty)
		{
			BindingOperations.GetBindingExpression(PRO_edcRange, i_fdcDependencyProperty)?.UpdateTarget();
		}
	}
}
