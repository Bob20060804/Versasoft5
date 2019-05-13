using System;
using System.Threading;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_FortschrittsanzeigeViewModel
	{
		IDisposable FUN_fdcAbbrechbareFortschrittsAnzeigeEinblenden(CancellationTokenSource i_fdcCancellationTokenSource, string i_strAnzeigeText = null);

		IDisposable FUN_fdcFortschrittsAnzeigeEinblenden(bool i_blnUeberdeckendeAnzeige = false, string i_strAnzeigeText = null);

		void SUB_FortschrittsAnzeigeEinblenden(bool i_blnUeberdeckendeAnzeige = false, string i_strAnzeigeText = null);

		void SUB_FortschrittsAnzeigeAusblenden();
	}
}
