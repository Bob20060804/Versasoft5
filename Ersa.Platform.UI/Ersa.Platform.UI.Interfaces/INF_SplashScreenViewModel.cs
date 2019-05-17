using Ersa.Platform.UI.Common.Interfaces;
using Ersa.Platform.UI.Splash;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ersa.Platform.UI.Interfaces
{
	public interface INF_SplashScreenViewModel : INF_ModulStatusViewModel, INotifyPropertyChanged, IDisposable
	{
		string PRO_strMeldungsTextKey
		{
			get;
			set;
		}

		string PRO_strSoftwareVersion
		{
			get;
			set;
		}

		string PRO_strHerstellerInfo
		{
			get;
			set;
		}

		string PRO_strMaschinenTyp
		{
			get;
			set;
		}

		string PRO_strMaschinenIp
		{
			get;
			set;
		}

		string PRO_strVerbindungsModusKey
		{
			get;
			set;
		}

		bool PRO_blnIstInMeldungModus
		{
			get;
			set;
		}

		void SUB_InitialisiereDasModel(Dispatcher i_fdcDispatcher);

		void SUB_ZeigeMeldung(EDC_SplashMeldung i_edcSplahMeldung);

		Task<bool> FUN_fdcWarteAufAntwortAsync(EDC_SplashMeldung i_edcSplahMeldung);

		Task SUB_EntferneVorhandeneMeldungAsync();
	}
}
