using Ersa.Global.Common.Data.Cad;
using Ersa.Platform.Common;
using Ersa.Platform.Common.IoT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung
{
	public interface INF_MaschinenBasisDatenCapability
	{
		IEnumerable<EDC_ElementVersion> FUN_enuMaschinenVersionenErmitteln();

		EDC_LokalisierungsKeyContainer FUN_edcMaschinenNamenErmitteln();

		string FUN_strMaschinenNummerErmitteln();

		bool FUN_blnIstInProduktion();

		bool FUN_blnIstOffline();

		bool FUN_blnIstInEinrichten();

		bool FUN_blnIstNichtAutomatik();

		IDisposable FUN_fdcBetriebsartAenderungsBenachrichtigungAbonnieren(Action i_delAenderung);

		string FUN_strDefaultMaschinenKonfigDateiNamenErmitteln();

		Task<long> FUN_fdcRegistriereMaschineAsync();

		Task<long> FUN_fdcHoleMaschinenIdAsync();

		Task<IEnumerable<long>> FUN_fdcHoleZugewieseneGruppenIdsAsync();

		Task<long> FUN_fdcHoleAktiveCodetabellenIdAsync();

		Task FUN_fdcSetzeAktiveCodetabellenIdAsync(long i_i64CodetabellenId);

		string FUN_strHoleMaschinenTyp();

		Task<long> FUN_fdcHoleDefaultMacheninGruppenIdAsync();

		IoTDeviceTyp FUN_edcHoleIotDeviceTyp();

		IEnumerable<ENUM_Blickrichtung> FUN_enuHoleVerfuegbareBlickwinkel();
	}
}
