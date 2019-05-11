using Ersa.Global.Mvvm;

namespace Ersa.Platform.Common.Loetprogramm
{
	public class EDC_ProjektEinstellungenBasis : BindableBase
	{
		private double m_dblDrehWinkel;

		private ENUM_BearbeitungsSeite m_enmBearbeitungsSeite;

		public int PRO_i32Version
		{
			get;
			set;
		}

		public double PRO_dblDrehWinkel
		{
			get
			{
				return m_dblDrehWinkel;
			}
			set
			{
				SetProperty(ref m_dblDrehWinkel, value, "PRO_dblDrehWinkel");
			}
		}

		public ENUM_BearbeitungsSeite PRO_enmBearbeitungsSeite
		{
			get
			{
				return m_enmBearbeitungsSeite;
			}
			set
			{
				SetProperty(ref m_enmBearbeitungsSeite, value, "PRO_enmBearbeitungsSeite");
			}
		}
	}
}
