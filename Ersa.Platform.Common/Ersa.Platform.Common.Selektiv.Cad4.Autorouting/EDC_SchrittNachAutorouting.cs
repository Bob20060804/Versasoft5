using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Selektiv.Cad4.Autorouting
{
	public class EDC_SchrittNachAutorouting
	{
		public ENUM_BahnTyp PRO_enmBahnTyp
		{
			get;
			set;
		}

		public ENUM_SchrittModus PRO_enmSchrittModus
		{
			get;
			set;
		}

		public ENUM_CadMaschinenModule PRO_enmModulTyp
		{
			get;
			set;
		}

		public int PRO_i32ModulNummer
		{
			get;
			set;
		}

		public int PRO_i32WerkzeugNummer
		{
			get;
			set;
		}

		public IList<EDC_AbstractCncSchritt> PRO_lstAblaufSchritte
		{
			get;
			set;
		}

		public ENUM_SyncpunktAttribut PRO_enmSynchronisationsModus
		{
			get;
			set;
		}

		public ENUM_SynchronisationsPosition PRO_enmSynchronisationsPosition
		{
			get;
			set;
		}

		public int PRO_i32SynchronisationsId
		{
			get;
			set;
		}

		public float PRO_sngEndpunktX
		{
			get;
			set;
		}

		public float PRO_sngEndpunktY
		{
			get;
			set;
		}

		public float PRO_sngStartpunktX
		{
			get;
			set;
		}

		public float PRO_sngStartpunktY
		{
			get;
			set;
		}

		public bool FUN_blnIstIdentisch(EDC_SchrittNachAutorouting i_edcOther)
		{
			if (PRO_enmBahnTyp == i_edcOther.PRO_enmBahnTyp && PRO_enmSchrittModus == i_edcOther.PRO_enmSchrittModus && PRO_enmModulTyp == i_edcOther.PRO_enmModulTyp && PRO_i32ModulNummer == i_edcOther.PRO_i32ModulNummer && PRO_i32WerkzeugNummer == i_edcOther.PRO_i32WerkzeugNummer && FUN_blnVergleicheAblaufSchritte(i_edcOther.PRO_lstAblaufSchritte) && PRO_enmSynchronisationsModus == i_edcOther.PRO_enmSynchronisationsModus && PRO_i32SynchronisationsId == i_edcOther.PRO_i32SynchronisationsId && PRO_sngEndpunktX.Equals(i_edcOther.PRO_sngEndpunktX) && PRO_sngEndpunktY.Equals(i_edcOther.PRO_sngEndpunktY) && PRO_sngStartpunktX.Equals(i_edcOther.PRO_sngStartpunktX))
			{
				return PRO_sngStartpunktY.Equals(i_edcOther.PRO_sngStartpunktY);
			}
			return false;
		}

		public EDC_SchrittNachAutorouting FUN_edcKopieErstellen()
		{
			return new EDC_SchrittNachAutorouting
			{
				PRO_i32WerkzeugNummer = PRO_i32WerkzeugNummer,
				PRO_enmModulTyp = PRO_enmModulTyp,
				PRO_enmBahnTyp = PRO_enmBahnTyp,
				PRO_enmSchrittModus = PRO_enmSchrittModus,
				PRO_enmSynchronisationsModus = PRO_enmSynchronisationsModus,
				PRO_enmSynchronisationsPosition = PRO_enmSynchronisationsPosition,
				PRO_i32ModulNummer = PRO_i32ModulNummer,
				PRO_i32SynchronisationsId = PRO_i32SynchronisationsId,
				PRO_lstAblaufSchritte = ((PRO_lstAblaufSchritte != null) ? new List<EDC_AbstractCncSchritt>(PRO_lstAblaufSchritte) : null),
				PRO_sngEndpunktX = PRO_sngEndpunktX,
				PRO_sngEndpunktY = PRO_sngEndpunktY,
				PRO_sngStartpunktX = PRO_sngStartpunktX,
				PRO_sngStartpunktY = PRO_sngStartpunktY
			};
		}

		private bool FUN_blnVergleicheAblaufSchritte(IList<EDC_AbstractCncSchritt> i_lstSchritte)
		{
			if (i_lstSchritte.Count != PRO_lstAblaufSchritte.Count)
			{
				return false;
			}
			return PRO_lstAblaufSchritte.Zip(i_lstSchritte, (EDC_AbstractCncSchritt i_edcSchritt, EDC_AbstractCncSchritt i_edcCncSchritt) => new Tuple<EDC_AbstractCncSchritt, EDC_AbstractCncSchritt>(i_edcSchritt, i_edcCncSchritt)).All((Tuple<EDC_AbstractCncSchritt, EDC_AbstractCncSchritt> i_edcTuple) => i_edcTuple.Item1.FUN_blnIstIdentisch(i_edcTuple.Item2));
		}
	}
}
