using Ersa.Platform.UI.Common.Helfer;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ersa.Platform.UI.Common.Converters
{
	[ValueConversion(typeof(object), typeof(EDC_EnumMember))]
	public class EDC_EnumBeschreibungConverter : IValueConverter
	{
		public object Convert(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			EDC_EnumMember eDC_EnumMember = FUN_edcEnumMemberErmitteln(i_objValue);
			if (eDC_EnumMember == null)
			{
				return DependencyProperty.UnsetValue;
			}
			return eDC_EnumMember.PRO_strDescription;
		}

		public object ConvertBack(object i_objValue, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}

		private static EDC_EnumMember FUN_edcEnumMemberErmitteln(object i_objValue)
		{
			if (i_objValue == null)
			{
				return null;
			}
			Type type = i_objValue.GetType();
			Type type2 = Nullable.GetUnderlyingType(type) ?? type;
			if (!type2.IsEnum)
			{
				return null;
			}
			return EDC_EnumHelfer.FUN_enmBeschreibungenErmitteln(type2).SingleOrDefault((EDC_EnumMember i_edcEnumMember) => i_edcEnumMember.PRO_i32Value == (int)i_objValue);
		}
	}
}
