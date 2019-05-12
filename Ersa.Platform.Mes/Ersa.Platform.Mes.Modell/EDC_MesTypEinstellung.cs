using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.Mes.Modell
{
	[Serializable]
	public abstract class EDC_MesTypEinstellung : BindableBase
	{
		private string m_strBibliothekProgrammTrennzeichen;

		private string m_strCodeTrennzeichen;

		private double m_dblPingIntervall;

		public int PRO_i32Version
		{
			get;
			set;
		}

		public string PRO_strBibliothekProgrammTrennzeichen
		{
			get
			{
				return m_strBibliothekProgrammTrennzeichen;
			}
			set
			{
				SetProperty(ref m_strBibliothekProgrammTrennzeichen, value, "PRO_strBibliothekProgrammTrennzeichen");
			}
		}

		public string PRO_strCodeTrennzeichen
		{
			get
			{
				return m_strCodeTrennzeichen;
			}
			set
			{
				SetProperty(ref m_strCodeTrennzeichen, value, "PRO_strCodeTrennzeichen");
			}
		}

		public double PRO_dblPingIntervall
		{
			get
			{
				return m_dblPingIntervall;
			}
			set
			{
				SetProperty(ref m_dblPingIntervall, value, "PRO_dblPingIntervall");
			}
		}

		public abstract void SUB_VerionskompatiblitaetHerstellen();
	}
}
