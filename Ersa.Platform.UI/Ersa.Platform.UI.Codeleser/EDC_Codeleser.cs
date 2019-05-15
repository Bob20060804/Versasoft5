using Ersa.Platform.Common.LeseSchreibGeraete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.UI.Codeleser
{
	public class EDC_Codeleser : EDC_CodeleserBasis
	{
		private IReadOnlyList<EDC_CodeMitVerwendungUndBedeutung> m_lstGeleseneWerte;

		private string m_strGelesenerWert;

		public string PRO_strGelesenerWert
		{
			get
			{
				return m_strGelesenerWert;
			}
			private set
			{
				SetProperty(ref m_strGelesenerWert, value, "PRO_strGelesenerWert");
			}
		}

		public IReadOnlyList<EDC_CodeMitVerwendungUndBedeutung> PRO_lstGeleseneWerte
		{
			get
			{
				return m_lstGeleseneWerte;
			}
			private set
			{
				SetProperty(ref m_lstGeleseneWerte, value, "PRO_lstGeleseneWerte");
			}
		}

		public EDC_Codeleser(long i_i64ArrayIndex, string i_strBezeichnung)
			: base(i_i64ArrayIndex, i_strBezeichnung)
		{
			PRO_lstGeleseneWerte = new List<EDC_CodeMitVerwendungUndBedeutung>();
		}

		public void SUB_SetzeGeleseneWerte(IReadOnlyList<EDC_CodeMitVerwendungUndBedeutung> i_lstWerte)
		{
			PRO_lstGeleseneWerte = i_lstWerte;
			if (i_lstWerte == null)
			{
				PRO_strGelesenerWert = null;
			}
			else
			{
				PRO_strGelesenerWert = string.Join(Environment.NewLine, from i_edcWert in i_lstWerte
				select i_edcWert.PRO_strCode);
			}
		}

		public void SUB_SetzeFehler(string i_strFehler)
		{
			PRO_lstGeleseneWerte = new List<EDC_CodeMitVerwendungUndBedeutung>();
			PRO_strGelesenerWert = i_strFehler;
		}
	}
}
