using Ersa.Global.Common;
using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Ersa.Global.Controls.Eingabe.ViewModels
{
	public abstract class EDC_TastaturViewModel : EDC_ViewModelBasis, IDataErrorInfo
	{
		protected bool m_blnAktuellenWertErsetzen;

		private string m_strWert;

		private string m_strError;

		private double m_dblSkalaWert;

		private bool m_blnSkalaWirdAktualisiert;

		public ICommand PRO_cmdTextEingabe
		{
			get;
			private set;
		}

		public ICommand PRO_cmdMaxSetzen
		{
			get;
			private set;
		}

		public ICommand PRO_cmdMinSetzen
		{
			get;
			private set;
		}

		public ICommand PRO_cmdAbbrechen
		{
			get;
			set;
		}

		public ICommand PRO_cmdUebernehmen
		{
			get;
			set;
		}

		public IValueConverter PRO_fdcLokalisierungsConverter
		{
			get;
			set;
		}

		public string PRO_strTitel
		{
			get;
			set;
		}

		public string PRO_strEinheit
		{
			get;
			set;
		}

		public string PRO_strMin
		{
			get;
			set;
		}

		public string PRO_strMax
		{
			get;
			set;
		}

		public string PRO_strTextWennKeinFehler
		{
			get;
			set;
		}

		public string PRO_strAbbrechenText
		{
			get;
			set;
		}

		public string PRO_strUebernehmenText
		{
			get;
			set;
		}

		public bool PRO_blnDialogResult
		{
			get;
			set;
		}

		public abstract bool PRO_blnKommaSichtbar
		{
			get;
		}

		public abstract bool PRO_blnNegationSichtbar
		{
			get;
		}

		public string PRO_strWert
		{
			get
			{
				return m_strWert;
			}
			set
			{
				if (!(m_strWert == value))
				{
					m_strWert = value;
					SUB_TextGeaendert(PRO_strWert);
					SUB_OnPropertyChanged("PRO_strWert");
					m_blnSkalaWirdAktualisiert = true;
					double? num = FUN_dblWertNachSkalaWertKonvertieren(m_strWert);
					if (num.HasValue)
					{
						PRO_dblSkalaWert = num.Value;
					}
					m_blnSkalaWirdAktualisiert = false;
				}
			}
		}

		public double PRO_dblSkalaWert
		{
			get
			{
				return m_dblSkalaWert;
			}
			set
			{
				if (Math.Abs(m_dblSkalaWert - value) < 1E-10)
				{
					return;
				}
				m_dblSkalaWert = value;
				if (!m_blnSkalaWirdAktualisiert)
				{
					string text = FUN_strSkalaWertNachWertKonvertieren(m_dblSkalaWert);
					if (text != null)
					{
						PRO_strWert = text;
					}
					m_blnAktuellenWertErsetzen = false;
				}
				SUB_OnPropertyChanged("PRO_dblSkalaWert");
			}
		}

		public string Error
		{
			get
			{
				return m_strError;
			}
			private set
			{
				if (!(m_strError == value))
				{
					m_strError = value;
					SUB_OnPropertyChanged("Error");
				}
			}
		}

		public string this[string i_strProperty]
		{
			get
			{
				if (i_strProperty == "PRO_strWert")
				{
					Error = FUN_strWertValidieren();
					return Error;
				}
				return string.Empty;
			}
		}

		protected EDC_TastaturViewModel()
		{
			m_blnAktuellenWertErsetzen = true;
			PRO_cmdTextEingabe = new EDC_DelegateCommand(delegate(object i_objParameter)
			{
				SUB_TextEingabe(i_objParameter as string);
			});
			PRO_cmdMaxSetzen = new EDC_DelegateCommand(delegate
			{
				SUB_MaxSetzen();
			});
			PRO_cmdMinSetzen = new EDC_DelegateCommand(delegate
			{
				SUB_MinSetzen();
			});
		}

		protected abstract void SUB_TextEingabe(string i_strText, bool i_blnAktuellenWertErsetzen);

		protected abstract string FUN_strWertValidieren();

		protected abstract void SUB_TextGeaendert(string i_strText);

		protected abstract void SUB_MaxSetzen();

		protected abstract void SUB_MinSetzen();

		protected abstract string FUN_strSkalaWertNachWertKonvertieren(double i_dblSkalaWert);

		protected abstract double? FUN_dblWertNachSkalaWertKonvertieren(string i_strWert);

		private void SUB_TextEingabe(string i_strText)
		{
			SUB_TextEingabe(i_strText, m_blnAktuellenWertErsetzen);
			m_blnAktuellenWertErsetzen = false;
		}
	}
}
