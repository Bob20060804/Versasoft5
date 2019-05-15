using Ersa.Global.Common;
using Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle;
using Ersa.Platform.UI.Codeleser;
using Prism.Commands;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Interfaces
{
	public interface INF_RuestkontrolleLeserViewModel
	{
		DelegateCommand<EDC_RuestkontrolleLeser> PRO_cmdDatenLesen
		{
			get;
		}

		EDC_SmartObservableCollection<EDC_RuestkontrolleLeser> PRO_lstRuestkontrolleLeser
		{
			get;
		}

		bool PRO_blnHatRuestkontrolleLeser
		{
			get;
		}

		void SUB_RuestkontrolleLesenAenderungAbonieren();

		Task FUN_fdcInitialisierenAsync(params ENUM_RuestWerkzeug[] ia_enmWerkzeuge);

		Task FUN_fdcInitialisierenMitGruppenNamenAsync(params ENUM_RuestWerkzeug[] ia_enmWerkzeuge);

		void SUB_Aufraeumen();
	}
}
