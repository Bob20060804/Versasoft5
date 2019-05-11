using Ersa.Global.Common;
using Ersa.Platform.CapabilityContracts.KommunikationsDienst;
using Ersa.Platform.Common;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Plc.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
	[Export]
	public class EDC_ParameterLeseStrategie : INF_ParameterBehandlungsStrategie<Action<EDC_PrimitivParameter>>
	{
		private readonly Lazy<INF_AdressenZusammenSetzenCapability> m_edcAdressenZusammensetzerCapability;

		private readonly EDC_AktionenFuerParameterTypen m_blnAktionenFuerParameterTypen;

		[ImportingConstructor]
		public EDC_ParameterLeseStrategie(INF_CapabilityProvider i_edcCapabilityProvider, EDC_AktionenFuerParameterTypen i_blnAktionenFuerParameterTypen)
		{
			m_edcAdressenZusammensetzerCapability = new Lazy<INF_AdressenZusammenSetzenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_AdressenZusammenSetzenCapability>);
			m_blnAktionenFuerParameterTypen = i_blnAktionenFuerParameterTypen;
		}

		public Action<EDC_PrimitivParameter> FUN_objBehandleAktuelleZeit(EDC_PrimitivParameter i_edcParameter)
		{
			m_blnAktionenFuerParameterTypen.PRO_dicLeseAktionen.TryGetValue(ENUM_SpsTyp.enmInt32, out Func<string, object> delM1Aktion);
			if (delM1Aktion == null)
			{
				throw new InvalidOperationException($"Keine Lese-Methode fuer den Typ {ENUM_SpsTyp.enmInt32} konnte gefunden werden!");
			}
			List<string> lstAdressen = m_edcAdressenZusammensetzerCapability.Value.FUN_enuBehandleSollZeit(i_edcParameter).ToList();
			return delegate(EDC_PrimitivParameter i_edcParam)
			{
				int num = (int)delM1Aktion(lstAdressen[0]);
				int num2 = (int)delM1Aktion(lstAdressen[1]);
				int num3 = (int)delM1Aktion(lstAdressen[2]);
				int num4 = (int)delM1Aktion(lstAdressen[3]);
				int num5 = (int)delM1Aktion(lstAdressen[4]);
				int num6 = (int)delM1Aktion(lstAdressen[5]);
				i_edcParam.PRO_objValue = (EDC_Utility.FUN_blnKannValidesDateTimeErstellen(num, num2, num3, num4, num5, num6) ? new DateTime(num, num2, num3, num4, num5, num6) : default(DateTime));
			};
		}

		public Action<EDC_PrimitivParameter> FUN_objBehandleSollZeit(EDC_PrimitivParameter i_edcParameter)
		{
			return delegate
			{
			};
		}

		public Action<EDC_PrimitivParameter> FUN_objBehandleDefault(EDC_PrimitivParameter i_edcParameter)
		{
			string strPhysischeAdresse = m_edcAdressenZusammensetzerCapability.Value.FUN_strErstellePhysischeAdresse(i_edcParameter);
			ENUM_SpsTyp eNUM_SpsTyp = strPhysischeAdresse.FUN_enmErmittelParameterTyp();
			m_blnAktionenFuerParameterTypen.PRO_dicLeseAktionen.TryGetValue(eNUM_SpsTyp, out Func<string, object> delM1Aktion);
			if (delM1Aktion == null)
			{
				throw new InvalidOperationException($"Keine Lese-Methode fuer den Typ {eNUM_SpsTyp} konnte gefunden werden!");
			}
			float sngFaktor = EDC_KommunikationsHelfer.FUN_sngErmittelFaktor(i_edcParameter);
			if (i_edcParameter is EDC_IntegerParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					((EDC_IntegerParameter)i_edcParam).PRO_intWert = EDC_WertKonvertierung.FUN_intWertUmwandeln(delM1Aktion(strPhysischeAdresse), sngFaktor);
				};
			}
			if (i_edcParameter is EDC_UIntegerParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					((EDC_UIntegerParameter)i_edcParam).PRO_intWert = EDC_WertKonvertierung.FUN_u32WertUmwandeln(delM1Aktion(strPhysischeAdresse), sngFaktor);
				};
			}
			if (i_edcParameter is EDC_NumerischerParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					EDC_NumerischerParameter eDC_NumerischerParameter = (EDC_NumerischerParameter)i_edcParam;
					eDC_NumerischerParameter.PRO_sngWert = EDC_WertKonvertierung.FUN_sngWertUmwandeln(delM1Aktion(strPhysischeAdresse), sngFaktor, eDC_NumerischerParameter.PRO_int32Nachkommastellen);
				};
			}
			if (i_edcParameter is EDC_BooleanParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					((EDC_BooleanParameter)i_edcParam).PRO_blnWert = (bool)delM1Aktion(strPhysischeAdresse);
				};
			}
			if (i_edcParameter is EDC_StringParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					((EDC_StringParameter)i_edcParam).PRO_strWert = (string)delM1Aktion(strPhysischeAdresse);
				};
			}
			return delegate(EDC_PrimitivParameter i_edcParam)
			{
				i_edcParam.PRO_objValue = delM1Aktion(strPhysischeAdresse);
			};
		}
	}
}
