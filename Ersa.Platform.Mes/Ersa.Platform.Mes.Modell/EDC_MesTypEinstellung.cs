using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.Mes.Modell
{
    /// <summary>
    /// MES Type Setting
    /// </summary>
	[Serializable]
	public abstract class EDC_MesTypEinstellung : BindableBase
	{

        /// <summary>
        /// Code separator
        /// </summary>
		private string m_strCodeTrennzeichen;
        /// <summary>
        /// Ping Interval
        /// </summary>
		private double m_dblPingIntervall;

		public int PRO_i32Version
		{
			get;
			set;
		}

        private string m_strBibliothekProgrammTrennzeichen;
        /// <summary>
        /// Library program separator
        /// </summary>
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
