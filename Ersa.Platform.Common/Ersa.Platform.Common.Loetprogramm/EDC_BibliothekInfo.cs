using Ersa.Global.Common;
using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.Common.Loetprogramm
{
	[Serializable]
	public class EDC_BibliothekInfo : BindableBase, IEquatable<EDC_BibliothekInfo>
	{
		private string m_strBibliotheksName;

		public EDC_SmartObservableCollection<EDC_ProgrammInfo> PRO_lstProgramme
		{
			get;
		}

		public long PRO_i64BibliotheksId
		{
			get;
			set;
		}

		public string PRO_strBibliotheksName
		{
			get
			{
				return m_strBibliotheksName;
			}
			set
			{
				SetProperty(ref m_strBibliotheksName, value, "PRO_strBibliotheksName");
			}
		}

		public EDC_BibliothekInfo(string i_strBibliotheksName, long i_i64BibliotheksId = 0L)
		{
			PRO_lstProgramme = new EDC_SmartObservableCollection<EDC_ProgrammInfo>();
			m_strBibliotheksName = i_strBibliotheksName;
			PRO_i64BibliotheksId = i_i64BibliotheksId;
		}

		public bool Equals(EDC_BibliothekInfo i_edcBibliothek)
		{
			return PRO_i64BibliotheksId == i_edcBibliothek?.PRO_i64BibliotheksId;
		}

		public override int GetHashCode()
		{
			return PRO_i64BibliotheksId.GetHashCode();
		}
	}
}
