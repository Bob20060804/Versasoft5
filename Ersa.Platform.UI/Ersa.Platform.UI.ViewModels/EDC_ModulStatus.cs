using Ersa.Global.Mvvm;
using Ersa.Platform.Infrastructure;

namespace Ersa.Platform.UI.ViewModels
{
	public class EDC_ModulStatus : BindableBase
	{
		private int m_i32Zaehler;

		private string m_strMeldungKey;

		private ENUM_ModulStatus m_enmModulStatus;

		public string PRO_strModulNameKey
		{
			get;
			set;
		}

		public int PRO_i32Zaehler
		{
			get
			{
				return m_i32Zaehler;
			}
			set
			{
				SetProperty(ref m_i32Zaehler, value, "PRO_i32Zaehler");
			}
		}

		public string PRO_strMeldungKey
		{
			get
			{
				return m_strMeldungKey;
			}
			set
			{
				SetProperty(ref m_strMeldungKey, value, "PRO_strMeldungKey");
			}
		}

		public ENUM_ModulStatus PRO_enmModulStatus
		{
			get
			{
				return m_enmModulStatus;
			}
			set
			{
				SetProperty(ref m_enmModulStatus, value, "PRO_enmModulStatus");
			}
		}
	}
}
