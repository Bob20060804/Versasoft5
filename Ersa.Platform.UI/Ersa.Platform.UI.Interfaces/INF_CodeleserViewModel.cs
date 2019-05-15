using Ersa.Global.Common;
using Ersa.Global.Mvvm.Commands;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.UI.Codeleser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Interfaces
{
	public interface INF_CodeleserViewModel
	{
		AsyncCommand<EDC_Codeleser> PRO_cmdCodeLesen
		{
			get;
		}

		EDC_SmartObservableCollection<EDC_Codeleser> PRO_lstCodeleser
		{
			get;
		}

		bool PRO_blnHatCodeleser
		{
			get;
		}

		IEnumerable<long> PRO_enuAktiveArrayIndizes
		{
			get;
		}

		void SUB_CodeLesenAenderungAbonieren();

		Task FUN_fdcInitialisierenAsync(ENUM_LsgOrt? i_enmOrt = default(ENUM_LsgOrt?), ENUM_LsgSpur? i_enmSpur = default(ENUM_LsgSpur?), ENUM_LsgVerwendung? i_enmVerwendung = default(ENUM_LsgVerwendung?));

		void SUB_Aufraeumen();
	}
}
