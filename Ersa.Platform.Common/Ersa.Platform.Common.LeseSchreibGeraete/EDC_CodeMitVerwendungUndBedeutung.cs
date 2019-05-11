using Ersa.Platform.Common.Konstanten;
using System;

namespace Ersa.Platform.Common.LeseSchreibGeraete
{
	public class EDC_CodeMitVerwendungUndBedeutung : IEquatable<EDC_CodeMitVerwendungUndBedeutung>
	{
		public string PRO_strCode
		{
			get;
			private set;
		}

		public ENUM_CodeVerwendung PRO_enmVerwendung
		{
			get;
			private set;
		}

		public ENUM_CodeBedeutung PRO_enmBedeutung
		{
			get;
			private set;
		}

		public EDC_CodeMitVerwendungUndBedeutung(string i_strCode, ENUM_CodeVerwendung i_enmVerwendung, ENUM_CodeBedeutung i_enmBedeutung)
		{
			PRO_strCode = i_strCode;
			PRO_enmVerwendung = i_enmVerwendung;
			PRO_enmBedeutung = i_enmBedeutung;
		}

		public bool Equals(EDC_CodeMitVerwendungUndBedeutung i_edcCodeMitVerwendungUndBedeutung)
		{
			if (i_edcCodeMitVerwendungUndBedeutung == null)
			{
				return false;
			}
			if (!i_edcCodeMitVerwendungUndBedeutung.PRO_strCode.Equals(PRO_strCode))
			{
				return false;
			}
			if (!i_edcCodeMitVerwendungUndBedeutung.PRO_enmVerwendung.Equals(PRO_enmVerwendung))
			{
				return false;
			}
			if (!i_edcCodeMitVerwendungUndBedeutung.PRO_enmBedeutung.Equals(PRO_enmBedeutung))
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object i_objOther)
		{
			if (i_objOther == null)
			{
				return false;
			}
			EDC_CodeMitVerwendungUndBedeutung eDC_CodeMitVerwendungUndBedeutung = i_objOther as EDC_CodeMitVerwendungUndBedeutung;
			if (eDC_CodeMitVerwendungUndBedeutung == null)
			{
				return false;
			}
			return Equals(eDC_CodeMitVerwendungUndBedeutung);
		}

		public override int GetHashCode()
		{
			return PRO_strCode.GetHashCode() ^ PRO_enmVerwendung.GetHashCode() ^ PRO_enmBedeutung.GetHashCode();
		}

		public override string ToString()
		{
			return $"Code: {PRO_strCode}, Usage: {PRO_enmVerwendung}, Meaning: {PRO_enmBedeutung}";
		}
	}
}
