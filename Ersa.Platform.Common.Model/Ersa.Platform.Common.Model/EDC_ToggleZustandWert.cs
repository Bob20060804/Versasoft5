using Ersa.Global.Mvvm;
using System.ComponentModel;

namespace Ersa.Platform.Common.Model
{
	public class EDC_ToggleZustandWert : BindableBase
	{
		private bool m_blnToggleQuittAenderung;

		private bool m_blnEin;

		private bool m_blnToggleQuit;

		public EDC_BooleanParameter PRO_edcToggle
		{
			get;
			private set;
		}

		public EDC_IntegerParameter PRO_edcZustand
		{
			get;
		}

		public bool PRO_blnToggleQuittVorgangAktiv
		{
			get;
			set;
		}

		public bool PRO_blnEin
		{
			get
			{
				return m_blnEin;
			}
			private set
			{
				SetProperty(ref m_blnEin, value, "PRO_blnEin");
			}
		}

		public bool PRO_blnToggleQuit
		{
			get
			{
				return m_blnToggleQuit;
			}
			private set
			{
				SetProperty(ref m_blnToggleQuit, value, "PRO_blnToggleQuit");
			}
		}

		public bool PRO_blnToggleQuittAenderung
		{
			get
			{
				return m_blnToggleQuittAenderung;
			}
			private set
			{
				m_blnToggleQuittAenderung = value;
				RaisePropertyChanged("PRO_blnToggleQuittAenderung");
			}
		}

		public EDC_ToggleZustandWert(EDC_BooleanParameter i_edcToggle, EDC_IntegerParameter i_edcZustand)
		{
			PRO_edcToggle = i_edcToggle;
			PRO_edcZustand = i_edcZustand;
			if (i_edcZustand != null)
			{
				SUB_ZustaendeAktualisieren();
				PropertyChangedEventManager.AddHandler(i_edcZustand, SUB_PropertyChanged, "PRO_intWert");
			}
		}

		private void SUB_PropertyChanged(object i_objSender, PropertyChangedEventArgs i_fdcPropertyChangedEventArgs)
		{
			SUB_ZustaendeAktualisieren();
			PRO_blnToggleQuittAenderung = true;
		}

		private void SUB_ZustaendeAktualisieren()
		{
			PRO_blnEin = ((ENUM_ButtonZustand)PRO_edcZustand.PRO_intWert.GetValueOrDefault()).HasFlag(ENUM_ButtonZustand.enmTeilablaufAktiv);
			PRO_blnToggleQuit = ((ENUM_ButtonZustand)PRO_edcZustand.PRO_intWert.GetValueOrDefault()).HasFlag(ENUM_ButtonZustand.enmToggleQuit);
		}
	}
}
