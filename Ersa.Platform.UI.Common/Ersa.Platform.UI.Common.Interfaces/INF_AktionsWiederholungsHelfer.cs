using System;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_AktionsWiederholungsHelfer
	{
		bool FUN_blnFuehreWiederholebareDateiOperationAus(Action i_delAction, string i_strDialogTitel);

		Task<bool> FUN_fdcFuehreWiederholebareDateiOperationAusAsync(Func<Task> i_delAktionsErsteller, string i_strDialogTitel);
	}
}
