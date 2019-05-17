using Ersa.Platform.Infrastructure;
using System;

namespace Ersa.Platform.UI.BreadCrumb
{
	public abstract class EDC_BreadCrumbElement : EDC_NotificationObjectMitSprachUmschaltung
	{
		private object m_objTag;

		private Func<string> m_delLokalisierungsAktion;

		public Func<string> PRO_delLokalisierungsAktion
		{
			private get
			{
				return m_delLokalisierungsAktion ?? ((Func<string>)(() => string.Empty));
			}
			set
			{
				m_delLokalisierungsAktion = value;
				RaisePropertyChanged("PRO_strText");
			}
		}

		public string PRO_strText => PRO_delLokalisierungsAktion();

		public object PRO_objTag
		{
			get
			{
				return m_objTag;
			}
			set
			{
				SetProperty(ref m_objTag, value, "PRO_objTag");
			}
		}

		public bool PRO_blnIstFehlerhaft
		{
			get;
			set;
		}

		public override string ToString()
		{
			return PRO_strText;
		}
	}
}
