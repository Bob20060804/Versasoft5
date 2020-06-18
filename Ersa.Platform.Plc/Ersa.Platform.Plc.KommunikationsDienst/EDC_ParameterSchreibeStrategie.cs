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
    /// <summary>
    /// 参数写入策略
    /// parameter Write strategy
    /// </summary>
	[Export]
	public class EDC_ParameterSchreibeStrategie : INF_ParameterBehandlungsStrategie<Action<EDC_PrimitivParameter>>
	{
		private readonly Lazy<INF_AdressenZusammenSetzenCapability> m_edcAdressenZusammensetzerCapability;

		private readonly EDC_AktionenFuerParameterTypen m_blnAktionenFuerParameterTypen;

		[ImportingConstructor]
		public EDC_ParameterSchreibeStrategie(INF_CapabilityProvider i_edcCapabilityProvider, EDC_AktionenFuerParameterTypen i_blnAktionenFuerParameterTypen)
		{
			m_edcAdressenZusammensetzerCapability = new Lazy<INF_AdressenZusammenSetzenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_AdressenZusammenSetzenCapability>);
			m_blnAktionenFuerParameterTypen = i_blnAktionenFuerParameterTypen;
		}

		public Action<EDC_PrimitivParameter> FUN_objBehandleAktuelleZeit(EDC_PrimitivParameter i_edcParameter)
		{
			throw new InvalidOperationException("Die Lesezeit darf nicht geschrieben werden!");
		}

		public Action<EDC_PrimitivParameter> FUN_objBehandleSollZeit(EDC_PrimitivParameter i_edcParameter)
		{
			m_blnAktionenFuerParameterTypen.PRO_dicSchreibeAktionen.TryGetValue(ENUM_SpsTyp.enmInt32, out Action<string, object> delM1Aktion);
			if (delM1Aktion == null)
			{
				throw new InvalidOperationException($"Keine Schreibe-Methode fuer den Typ {ENUM_SpsTyp.enmInt32} konnte gefunden werden!");
			}
			List<string> lstAdressen = m_edcAdressenZusammensetzerCapability.Value.FUN_enuBehandleSollZeit(i_edcParameter).ToList();
			return delegate(EDC_PrimitivParameter i_edcParam)
			{
				DateTime dateTime = (DateTime)i_edcParam.PRO_objValue;
				delM1Aktion(lstAdressen[0], dateTime.Year - 1900);
				delM1Aktion(lstAdressen[1], dateTime.Month);
				delM1Aktion(lstAdressen[2], dateTime.Day);
				delM1Aktion(lstAdressen[3], dateTime.Hour);
				delM1Aktion(lstAdressen[4], dateTime.Minute);
				delM1Aktion(lstAdressen[5], dateTime.Second);
			};
		}

		public Action<EDC_PrimitivParameter> FUN_objBehandleDefault(EDC_PrimitivParameter i_edcParameter)
		{
			string strPhysischeAdresse = m_edcAdressenZusammensetzerCapability.Value.FUN_strErstellePhysischeAdresse(i_edcParameter);
			ENUM_SpsTyp eNUM_SpsTyp = strPhysischeAdresse.FUN_enmErmittelParameterTyp();
			m_blnAktionenFuerParameterTypen.PRO_dicSchreibeAktionen.TryGetValue(eNUM_SpsTyp, out Action<string, object> delM1Aktion);

			if (delM1Aktion == null)
			{
				throw new InvalidOperationException($"Keine Schreibe-Methode fuer den Typ {eNUM_SpsTyp} konnte gefunden werden!");
			}
			float sngFaktor = EDC_KommunikationsHelfer.FUN_sngErmittelFaktor(i_edcParameter);
			if (i_edcParameter is EDC_IntegerParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					EDC_IntegerParameter eDC_IntegerParameter = (EDC_IntegerParameter)i_edcParam;
					delM1Aktion(strPhysischeAdresse, EDC_WertKonvertierung.FUN_intWertFaktorisieren(eDC_IntegerParameter.PRO_intWert, 1f / sngFaktor));
				};
			}
			if (i_edcParameter is EDC_UIntegerParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					EDC_UIntegerParameter eDC_UIntegerParameter = (EDC_UIntegerParameter)i_edcParam;
					delM1Aktion(strPhysischeAdresse, EDC_WertKonvertierung.FUN_u32WertFaktorisieren(eDC_UIntegerParameter.PRO_intWert, 1f / sngFaktor));
				};
			}
			if (i_edcParameter is EDC_NumerischerParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					EDC_NumerischerParameter eDC_NumerischerParameter = (EDC_NumerischerParameter)i_edcParam;
					delM1Aktion(strPhysischeAdresse, EDC_WertKonvertierung.FUN_sngWertFaktorisieren(eDC_NumerischerParameter.PRO_sngWert, 1f / sngFaktor, eDC_NumerischerParameter.PRO_int32Nachkommastellen));
				};
			}
			if (i_edcParameter is EDC_BooleanParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					EDC_BooleanParameter eDC_BooleanParameter = (EDC_BooleanParameter)i_edcParam;
					delM1Aktion(strPhysischeAdresse, eDC_BooleanParameter.PRO_blnWert);
				};
			}
			if (i_edcParameter is EDC_StringParameter)
			{
				return delegate(EDC_PrimitivParameter i_edcParam)
				{
					EDC_StringParameter eDC_StringParameter = (EDC_StringParameter)i_edcParam;
					delM1Aktion(strPhysischeAdresse, eDC_StringParameter.PRO_strWert);
				};
			}
			return delegate(EDC_PrimitivParameter i_edcParam)
			{
				delM1Aktion(strPhysischeAdresse, i_edcParam.PRO_objValue);
			};
		}
	}
}
