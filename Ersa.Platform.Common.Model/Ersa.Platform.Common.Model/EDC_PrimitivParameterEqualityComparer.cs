using System.Collections.Generic;

namespace Ersa.Platform.Common.Model
{
	public class EDC_PrimitivParameterEqualityComparer : IEqualityComparer<EDC_PrimitivParameter>
	{
		public bool Equals(EDC_PrimitivParameter i_edcErsterParameter, EDC_PrimitivParameter i_edcZweiterParameter)
		{
			if (i_edcErsterParameter == null || i_edcZweiterParameter == null)
			{
				return false;
			}
			if (!i_edcErsterParameter.GetType().IsInstanceOfType(i_edcZweiterParameter))
			{
				return false;
			}
			if (i_edcErsterParameter.PRO_strAdresse == i_edcZweiterParameter.PRO_strAdresse)
			{
				return i_edcErsterParameter.PRO_edcParameterBeschreibung.Equals(i_edcZweiterParameter.PRO_edcParameterBeschreibung);
			}
			return false;
		}

		public int GetHashCode(EDC_PrimitivParameter i_edcPrimitivParameter)
		{
			return i_edcPrimitivParameter.PRO_strAdresse.GetHashCode() ^ i_edcPrimitivParameter.PRO_edcParameterBeschreibung.GetHashCode();
		}
	}
}
