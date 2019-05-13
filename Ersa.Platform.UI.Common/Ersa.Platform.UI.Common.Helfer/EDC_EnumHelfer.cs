using Ersa.Global.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Ersa.Platform.UI.Common.Helfer
{
	public static class EDC_EnumHelfer
	{
		private const string mC_strStandardFarbe = "#012f50";

		public static IEnumerable<EDC_EnumMember> FUN_enmBeschreibungenErmitteln(Type i_fdcEnumType)
		{
			return from object fdcEnumValue in Enum.GetValues(i_fdcEnumType)
			select new EDC_EnumMember
			{
				PRO_enmValue = fdcEnumValue,
				PRO_i32Value = Convert.ToInt32(fdcEnumValue),
				PRO_strDescription = EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(i_fdcEnumType, fdcEnumValue),
				PRO_fdcFarbe = FUN_fdcEnumFarbeErmitteln(i_fdcEnumType, fdcEnumValue)
			} into i_edcMember
			where !string.IsNullOrEmpty(i_edcMember.PRO_strDescription)
			select i_edcMember;
		}

		private static Brush FUN_fdcEnumFarbeErmitteln(Type i_fdcEnumTyp, object i_objEnumValue)
		{
			string value = (i_fdcEnumTyp.GetField(i_objEnumValue.ToString()).GetCustomAttributes(typeof(EDC_FarbeAttribute), inherit: false).FirstOrDefault() as EDC_FarbeAttribute)?.PRO_strFarbe;
			if (string.IsNullOrEmpty(value))
			{
				value = "#012f50";
			}
			return new BrushConverter().ConvertFrom(value) as SolidColorBrush;
		}
	}
}
