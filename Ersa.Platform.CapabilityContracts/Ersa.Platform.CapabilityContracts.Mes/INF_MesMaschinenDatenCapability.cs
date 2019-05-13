using Ersa.Platform.Common.Mes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.Mes
{
	public interface INF_MesMaschinenDatenCapability
	{
		Task<EDC_MesMaschinenDaten> FUN_fdcDatenHolenAsync(ENUM_MesFunktionen i_enuFunktion);

		void SUB_MesAktivSetzen(bool i_blnIstMesAktiv);

		void SUB_MesTypSetzen(ENUM_MesTyp i_enmMesTyp);

		Task<IList<ENUM_ZusatzprotokollTyp>> FUN_fdcImplementierteZusatzProtokolleHolenAsync();

		ENUM_ZusatzprotokollTyp FUN_enmAktivesZusatzProtokolleHolen();

		void SUB_AktivesZusatzProtokolleSetzen(ENUM_ZusatzprotokollTyp i_enmZusatzprotokollTyp);

		void SUB_MesKonfigurationAnsichtVerlassen();
	}
}
