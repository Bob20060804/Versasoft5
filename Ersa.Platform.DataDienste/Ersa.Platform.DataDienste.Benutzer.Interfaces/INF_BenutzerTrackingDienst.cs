using Ersa.Platform.Common.Data.Benutzer;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Benutzer.Interfaces
{
	public interface INF_BenutzerTrackingDienst
	{
		void SUB_Initialisieren();

		Task FUN_fdcTrackHinzufuegenAsync(string i_strActivityKey, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcParameterTrackHinzufuegenAsync(IEnumerable<EDC_ParameterTrack> i_enuParameter);
	}
}
