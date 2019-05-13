using Ersa.Global.Mvvm;
using Ersa.Global.Mvvm.Commands;
using Ersa.Platform.UI.Common.Interfaces;
using Prism.Commands;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common.ViewModels
{
	[Export]
	public class EDC_BasisCommandsViewModel : BindableBase
	{
		public DelegateCommand<INF_BasisCommandSource> PRO_cmdWertGeaendert
		{
			get;
		}

		public AsyncCommand<INF_SpeichernCommandSource> PRO_cmdSpeichern
		{
			get;
		}

		public AsyncCommand<INF_VerwerfenCommandSource> PRO_cmdVerwerfen
		{
			get;
		}

		public DelegateCommand<INF_VerlassenCommandSource> PRO_cmdVerlassen
		{
			get;
		}

		public EDC_BasisCommandsViewModel()
		{
			PRO_cmdWertGeaendert = new DelegateCommand<INF_BasisCommandSource>(SUB_OnWertGeaendert);
			PRO_cmdSpeichern = new AsyncCommand<INF_SpeichernCommandSource>(FUN_fdcSpeichernAsync);
			PRO_cmdVerwerfen = new AsyncCommand<INF_VerwerfenCommandSource>(FUN_fdcVerwerfenAsync);
			PRO_cmdVerlassen = new DelegateCommand<INF_VerlassenCommandSource>(SUB_Verlassen);
		}

		private void SUB_OnWertGeaendert(INF_BasisCommandSource i_edcCommandSource)
		{
			i_edcCommandSource.SUB_AktualisiereZustand();
		}

		private async Task FUN_fdcSpeichernAsync(INF_SpeichernCommandSource i_edcCommandSource)
		{
			if (i_edcCommandSource.PRO_blnHatAenderung)
			{
				await i_edcCommandSource.FUN_fdcAenderungenSpeichernAsync().ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		private async Task FUN_fdcVerwerfenAsync(INF_VerwerfenCommandSource i_edcCommandSource)
		{
			if (i_edcCommandSource.PRO_blnHatAenderung)
			{
				await i_edcCommandSource.FUN_fdcAenderungenVerwerfenAsync().ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		private void SUB_Verlassen(INF_VerlassenCommandSource i_edcCommandSource)
		{
			i_edcCommandSource.SUB_Verlassen();
		}
	}
}
