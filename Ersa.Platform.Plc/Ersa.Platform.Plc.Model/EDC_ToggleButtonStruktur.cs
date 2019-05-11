using Ersa.Platform.Common.Model;
using Ersa.Platform.Plc.KommunikationsDienst;
using System.ComponentModel;

namespace Ersa.Platform.Plc.Model
{
	public class EDC_ToggleButtonStruktur : EDC_StructParameter
	{
		public EDC_BooleanParameter PRO_edcToggle => (EDC_BooleanParameter)base.PRO_edcKomposition[EDC_AdressTeile.ENUM_LevelEndeAnfo.enmBlnAkToggle];

		public EDC_BooleanParameter PRO_edcToggleQuit => (EDC_BooleanParameter)base.PRO_edcKomposition[EDC_AdressTeile.ENUM_LevelEndeStat.enmBlnAkToggleQuitt];

		public EDC_IntegerParameter PRO_edcButtonZustand => (EDC_IntegerParameter)base.PRO_edcKomposition[EDC_AdressTeile.ENUM_LevelEndeStat.enmEnmZustand];

		public bool PRO_blnEingabeGesperrt => ((ENUM_ButtonZustand)PRO_edcButtonZustand.PRO_intWert.GetValueOrDefault()).HasFlag(ENUM_ButtonZustand.enmEingabeGesperrt);

		public bool PRO_blnTeilablaufAktiv => ((ENUM_ButtonZustand)PRO_edcButtonZustand.PRO_intWert.GetValueOrDefault()).HasFlag(ENUM_ButtonZustand.enmTeilablaufAktiv);

		public bool PRO_blnGesamtablaufAktiv => ((ENUM_ButtonZustand)PRO_edcButtonZustand.PRO_intWert.GetValueOrDefault()).HasFlag(ENUM_ButtonZustand.enmGesamtablaufAktiv);

		public EDC_ToggleButtonStruktur()
		{
			base.PRO_edcKomposition.SUB_Hinzufuegen(this, EDC_AdressTeile.ENUM_LevelEndeAnfo.enmBlnAkToggle, new EDC_BooleanParameter
			{
				PRO_edcParameterBeschreibung = EDC_ParameterAufbauHelfer.FUN_edcAnfoParameterBeschreibungErstellen()
			});
			base.PRO_edcKomposition.SUB_Hinzufuegen(this, EDC_AdressTeile.ENUM_LevelEndeStat.enmBlnAkToggleQuitt, new EDC_BooleanParameter
			{
				PRO_edcParameterBeschreibung = EDC_ParameterAufbauHelfer.FUN_edcStatParameterBeschreibungErstellen()
			});
			base.PRO_edcKomposition.SUB_Hinzufuegen(this, EDC_AdressTeile.ENUM_LevelEndeStat.enmEnmZustand, new EDC_IntegerParameter
			{
				PRO_edcParameterBeschreibung = EDC_ParameterAufbauHelfer.FUN_edcStatParameterBeschreibungErstellen(),
				PRO_intWert = -1
			});
			SUB_ZustandsVariableUeberwachen();
		}

		private void SUB_ZustandsVariableUeberwachen()
		{
			PropertyChangedEventManager.AddHandler(PRO_edcButtonZustand, delegate
			{
				SUB_ZustandAktualisieren();
			}, "PRO_intWert");
		}

		private void SUB_ZustandAktualisieren()
		{
			RaisePropertyChanged("PRO_blnEingabeGesperrt");
			RaisePropertyChanged("PRO_blnTeilablaufAktiv");
			RaisePropertyChanged("PRO_blnGesamtablaufAktiv");
		}
	}
}
