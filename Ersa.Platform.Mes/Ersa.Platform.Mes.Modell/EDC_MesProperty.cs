using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.Mes.Modell
{
	/// <summary>
	/// MES Ù–‘
	/// Mes Property
	/// </summary>
	[Serializable]
	public class EDC_MesProperty : BindableBase
	{
		private string m_strId;

		private string m_strName;

		private string m_strValue;

		public string PRO_strId
		{
			get
			{
				return m_strId;
			}
			set
			{
				SetProperty(ref m_strId, value, "PRO_strId");
			}
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

		public string PRO_strValue
		{
			get
			{
				return m_strValue;
			}
			set
			{
				SetProperty(ref m_strValue, value, "PRO_strValue");
			}
		}
	}
}
