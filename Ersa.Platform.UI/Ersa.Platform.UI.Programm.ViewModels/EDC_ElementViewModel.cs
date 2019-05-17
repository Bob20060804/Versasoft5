using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.UI.Programm.ViewModels
{
	public abstract class EDC_ElementViewModel : BindableBase, IEquatable<EDC_ElementViewModel>
	{
		private string m_strName;

		private bool m_blnIstAusgewaehlt;

		private bool m_blnIstFehlerhaft;

		public long PRO_i64Id
		{
			get;
		}

		public string PRO_strName
		{
			get
			{
				return m_strName;
			}
			set
			{
				SetProperty(ref m_strName, value, "PRO_strName");
			}
		}

		public bool PRO_blnIstAusgewaehlt
		{
			get
			{
				return m_blnIstAusgewaehlt;
			}
			set
			{
				SetProperty(ref m_blnIstAusgewaehlt, value, "PRO_blnIstAusgewaehlt");
			}
		}

		public bool PRO_blnIstFehlerhaft
		{
			get
			{
				return m_blnIstFehlerhaft;
			}
			set
			{
				SetProperty(ref m_blnIstFehlerhaft, value, "PRO_blnIstFehlerhaft");
			}
		}

		protected EDC_ElementViewModel(long i_i64Id, string i_strName)
		{
			PRO_i64Id = i_i64Id;
			m_strName = i_strName;
		}

		public override bool Equals(object i_objVergleichsObjekt)
		{
			return Equals(i_objVergleichsObjekt as EDC_ElementViewModel);
		}

		public bool Equals(EDC_ElementViewModel i_edcElement)
		{
			return PRO_i64Id == i_edcElement?.PRO_i64Id;
		}

		public override int GetHashCode()
		{
			return PRO_i64Id.GetHashCode();
		}
	}
}
