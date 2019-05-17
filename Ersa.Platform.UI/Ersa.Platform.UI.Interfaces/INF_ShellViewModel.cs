using Ersa.Platform.Common;
using Ersa.Platform.UI.Common.Interfaces;
using System;
using System.ComponentModel;

namespace Ersa.Platform.UI.Interfaces
{
	public interface INF_ShellViewModel : INF_HauptmenuViewModel, INF_FortschrittsanzeigeViewModel, INF_StartupAktionViewModel, INotifyPropertyChanged
	{
		ENUM_MaschinenZustand PRO_enmMaschinenZustand
		{
			get;
			set;
		}

		string PRO_strMaschinenNameKey
		{
			get;
			set;
		}

		bool? PRO_blnVisuIstPrimaer
		{
			get;
			set;
		}

		bool PRO_blnIstMaschineInProduktion
		{
			get;
		}

		void SUB_AnwendungBeenden(bool i_blnOhneNachfrageBeenden);

		void SUB_OnAnwendungBeendenAktionRegistrieren(Action i_delAktion);

		void SUB_AnwendungsBildschirmeAktualisieren();
	}
}
