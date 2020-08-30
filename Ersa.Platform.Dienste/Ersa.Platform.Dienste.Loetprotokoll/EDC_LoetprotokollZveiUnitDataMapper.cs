using Ersa.Global.Common.Helper;
using Ersa.Platform.Common.Loetprotokoll;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	public class EDC_LoetprotokollZveiUnitDataMapper
	{
		private readonly EDC_ProtokollParameterNamenMapper m_edcNamenMapper;

		private EDC_LoetprotokollDateiEinstellungen m_edcEinstellungsParamter;

		[ImportingConstructor]
		public EDC_LoetprotokollZveiUnitDataMapper(EDC_ProtokollParameterNamenMapper i_edcNamenMapper)
		{
			m_edcNamenMapper = i_edcNamenMapper;
		}

		public EDC_LoetprotokollZveiUnitData FUN_edcVerarbeiteDaten(EDC_LoetprotokollDaten i_edcLoetProtokoll, EDC_LoetprotokollDateiEinstellungen i_edcEinstellungsParameter)
		{
			m_edcEinstellungsParamter = i_edcEinstellungsParameter;
			EDC_LoetprotokollZveiUnitData eDC_LoetprotokollZveiUnitData = new EDC_LoetprotokollZveiUnitData();
			if (i_edcLoetProtokoll == null)
			{
				throw new ArgumentNullException("Input parameter in EDC_LoetprotokollZveiUnitDataMapper.FUN_edcVerarbeiteDaten() is NULL");
			}
			SUB_KopfDaten(eDC_LoetprotokollZveiUnitData, i_edcLoetProtokoll.PRO_edcKopf);
			SUB_CodesHinzu(eDC_LoetprotokollZveiUnitData, i_edcLoetProtokoll.PRO_edcKopf);
			SUB_ElementeHinzu(eDC_LoetprotokollZveiUnitData, i_edcLoetProtokoll.PRO_lstModulElemente);
			return eDC_LoetprotokollZveiUnitData;
		}

		public EDC_LoetprotokollZveiUnitData FUN_edcVerarbeiteDaten(IEnumerable<EDC_LoetprotokollDaten> i_enmLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcEinstellungsParameter)
		{
			m_edcEinstellungsParamter = i_edcEinstellungsParameter;
			EDC_LoetprotokollZveiUnitData eDC_LoetprotokollZveiUnitData = new EDC_LoetprotokollZveiUnitData
			{
				messageId = Guid.NewGuid().ToString()
			};
			if (i_enmLoetprotokoll == null)
			{
				throw new ArgumentNullException("Input parameter in EDC_LoetprotokollZveiUnitDataMapper.FUN_edcVerarbeiteDaten() is NULL");
			}
			List<EDC_LoetprotokollDaten> list = i_enmLoetprotokoll.ToList();
			if (!list.Any())
			{
				throw new ArgumentNullException("Input parameter in EDC_LoetprotokollZveiUnitDataMapper.FUN_edcVerarbeiteDaten() is empty");
			}
			SUB_FuegeSubUnitDataHinzu(list, eDC_LoetprotokollZveiUnitData);
			EDC_LoetprotokollDaten eDC_LoetprotokollDaten = list.First();
			SUB_KopfDaten(eDC_LoetprotokollZveiUnitData, eDC_LoetprotokollDaten.PRO_edcKopf);
			SUB_ElementeHinzu(eDC_LoetprotokollZveiUnitData, eDC_LoetprotokollDaten.PRO_lstModulElemente);
			EDC_LoetprotokollElement eDC_LoetprotokollElement = FUN_edcHolePannelOrCarrierCode(eDC_LoetprotokollDaten);
			eDC_LoetprotokollZveiUnitData.unit = eDC_LoetprotokollElement.PRO_strIstWert;
			eDC_LoetprotokollDaten.PRO_edcKopf.PRO_lstScannerCodes.Remove(eDC_LoetprotokollElement);
			eDC_LoetprotokollDaten.PRO_edcKopf.PRO_lstScannerCodes.Remove(FUN_edcHoleSerialNummerCode(eDC_LoetprotokollDaten));
			SUB_FuegeRestlicheCodesHinzu(eDC_LoetprotokollZveiUnitData, eDC_LoetprotokollDaten.PRO_edcKopf.PRO_lstScannerCodes);
			return eDC_LoetprotokollZveiUnitData;
		}

		private static EDC_LoetprotokollElement FUN_edcHolePannelOrCarrierCode(EDC_LoetprotokollDaten i_edcErstesLoetprotokoll)
		{
			object eDC_LoetprotokollElement = i_edcErstesLoetprotokoll.PRO_edcKopf.PRO_lstScannerCodes.Find((EDC_LoetprotokollElement i_edcScannerCode) => i_edcScannerCode.PRO_strIdentifier == "enmProt|enmCodes|enmPanel");
			EDC_LoetprotokollElement eDC_LoetprotokollElement2 = i_edcErstesLoetprotokoll.PRO_edcKopf.PRO_lstScannerCodes.Find((EDC_LoetprotokollElement i_edcScannerCode) => i_edcScannerCode.PRO_strIdentifier == "enmProt|enmCodes|enmCarrier");
			if (eDC_LoetprotokollElement == null && eDC_LoetprotokollElement2 == null)
			{
				throw new ArgumentNullException("Code for panel and carrieer are null in EDC_LoetprotokollZveiUnitDataMapper.FUN_edcHolePannelOrCarrierCode()");
			}
			if (eDC_LoetprotokollElement != null && eDC_LoetprotokollElement2 != null)
			{
				throw new ArgumentException("Code for panel and carrieer are set in EDC_LoetprotokollZveiUnitDataMapper.FUN_edcHolePannelOrCarrierCode()");
			}
			if (eDC_LoetprotokollElement == null)
			{
				eDC_LoetprotokollElement = eDC_LoetprotokollElement2;
			}
			return (EDC_LoetprotokollElement)eDC_LoetprotokollElement;
		}

		private static EDC_LoetprotokollElement FUN_edcHoleSerialNummerCode(EDC_LoetprotokollDaten i_edcErstesLoetprotokoll)
		{
			return i_edcErstesLoetprotokoll.PRO_edcKopf.PRO_lstScannerCodes.Find((EDC_LoetprotokollElement i_edcScannerCode) => i_edcScannerCode.PRO_strIdentifier == "enmProt|enmCodes|enmSerialNumber");
		}

		private void SUB_FuegeSubUnitDataHinzu(List<EDC_LoetprotokollDaten> i_lstLoetprotokoll, EDC_LoetprotokollZveiUnitData i_edcUnitDataType)
		{
			List<subUnitDataType> list = new List<subUnitDataType>();
			foreach (EDC_LoetprotokollDaten item in i_lstLoetprotokoll)
			{
				if (item.PRO_edcKopf.PRO_lstScannerCodes == null)
				{
					throw new ArgumentNullException("No scanner codes available EDC_LoetprotokollZveiUnitDataMapper.FUN_edcVerarbeiteDaten()");
				}
				EDC_LoetprotokollElement eDC_LoetprotokollElement = FUN_edcHoleSerialNummerCode(item);
				if (eDC_LoetprotokollElement == null)
				{
					throw new ArgumentNullException("Code for serial number in EDC_LoetprotokollZveiUnitDataMapper.FUN_edcVerarbeiteDaten() is NULL");
				}
				string state = (item.PRO_edcKopf.PRO_edcBearbeitenExtern?.PRO_strIstWert?.ToLower().Equals(false.ToString().ToLower()) == true) ? "badBoard" : ((item.PRO_edcKopf.PRO_edcFehler.PRO_strIstWert == "0") ? m_edcEinstellungsParamter.PRO_strPcbStatusGut : m_edcEinstellungsParamter.PRO_strPcbStatusSchlecht);
				subUnitDataType subUnitDataType = new subUnitDataType
				{
					subUnit = eDC_LoetprotokollElement.PRO_strIstWert,
					positionType = "sequence",
					state = state
				};
				string text = item.PRO_edcKopf.PRO_edcNutzenPosition?.PRO_strIstWert;
				if (!string.IsNullOrWhiteSpace(text))
				{
					subUnitDataType.position = text;
				}
				list.Add(subUnitDataType);
			}
			i_edcUnitDataType.subUnitData = list.ToArray();
		}

		private void SUB_KopfDaten(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, EDC_LoetprotokollKopfElemente i_edcLoetprotokollKopfElemente)
		{
			i_edcUnitDataType.starttime = Convert.ToDateTime(i_edcLoetprotokollKopfElemente.PRO_edcEinlaufZeitpunkt.PRO_strIstWert).ToString("yyyy-MM-ddTHH:mm:sszzz");
			i_edcUnitDataType.endtimeSpecified = true;
			i_edcUnitDataType.endtime = Convert.ToDateTime(i_edcLoetprotokollKopfElemente.PRO_edcAuslaufZeitpunkt.PRO_strIstWert).ToString("yyyy-MM-ddTHH:mm:sszzz");
			i_edcUnitDataType.state = ((i_edcLoetprotokollKopfElemente.PRO_edcFehler.PRO_strIstWert == "0") ? m_edcEinstellungsParamter.PRO_strPcbStatusGut : m_edcEinstellungsParamter.PRO_strPcbStatusSchlecht);
			if (string.IsNullOrEmpty(m_edcEinstellungsParamter.PRO_strEquipmentBezeichnung))
			{
				i_edcUnitDataType.equipment = i_edcLoetprotokollKopfElemente.PRO_edcMaschineIdentifier.PRO_strIstWert;
			}
			else
			{
				i_edcUnitDataType.equipment = m_edcEinstellungsParamter.PRO_strEquipmentBezeichnung;
			}
			i_edcUnitDataType.@operator = i_edcLoetprotokollKopfElemente.PRO_edcBenutzer.PRO_strIstWert;
			SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollKopfElemente.PRO_edcLaufendeNummer);
			SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollKopfElemente.PRO_edcTransportbreiteIst);
			SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollKopfElemente.PRO_edcBibliothekName);
			SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollKopfElemente.PRO_edcProgrammName);
			SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollKopfElemente.PRO_edcProgrammVersionsId);
			SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollKopfElemente.PRO_edcArbeitsVersion);
			SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollKopfElemente.PRO_edcModus);
		}

		private void SUB_CodesHinzu(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, EDC_LoetprotokollKopfElemente i_edcLoetprotokollKopfElemente)
		{
			i_edcUnitDataType.unit = i_edcLoetprotokollKopfElemente.PRO_edcLaufendeNummer.PRO_strIstWert;
			if (i_edcLoetprotokollKopfElemente.PRO_lstScannerCodes != null)
			{
				EDC_LoetprotokollElement eDC_LoetprotokollElement = i_edcLoetprotokollKopfElemente.PRO_lstScannerCodes.FirstOrDefault();
				if (eDC_LoetprotokollElement != null)
				{
					i_edcUnitDataType.unit = eDC_LoetprotokollElement.PRO_strIstWert;
					IEnumerable<EDC_LoetprotokollElement> lstRestLicheCodes = i_edcLoetprotokollKopfElemente.PRO_lstScannerCodes.Skip(1);
					SUB_FuegeRestlicheCodesHinzu(i_edcUnitDataType, lstRestLicheCodes);
				}
			}
		}

		private void SUB_FuegeRestlicheCodesHinzu(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, IEnumerable<EDC_LoetprotokollElement> lstRestLicheCodes)
		{
			foreach (EDC_LoetprotokollElement lstRestLicheCode in lstRestLicheCodes)
			{
				EDC_LoetprotokollElement i_edcLoetprotokollElement = new EDC_LoetprotokollElement
				{
					PRO_strIstWert = lstRestLicheCode.PRO_strIstWert,
					PRO_enmProtokollParameterTyp = ENUM_ProtokollParameterTyp.enmProcessingParameter,
					PRO_strName = lstRestLicheCode.PRO_strName,
					PRO_strIdentifier = lstRestLicheCode.PRO_strIdentifier
				};
				SUB_ElementHinzu(i_edcUnitDataType, i_edcLoetprotokollElement);
			}
		}

		private void SUB_ElementeHinzu(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, List<EDC_LoetprotokollElement> i_lstElemente)
		{
			foreach (EDC_LoetprotokollElement item in i_lstElemente)
			{
				SUB_ElementHinzu(i_edcUnitDataType, item);
			}
		}

		private void SUB_ElementHinzu(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, EDC_LoetprotokollElement i_edcLoetprotokollElement)
		{
			if (i_edcLoetprotokollElement.PRO_enmProtokollParameterTyp == ENUM_ProtokollParameterTyp.enmProcessingParameter)
			{
				SUB_AddProcessingParameters(i_edcUnitDataType, i_edcLoetprotokollElement);
			}
			if (i_edcLoetprotokollElement.PRO_enmProtokollParameterTyp == ENUM_ProtokollParameterTyp.enmMeasuringParameter)
			{
				SUB_AddMeasuring(i_edcUnitDataType, i_edcLoetprotokollElement);
			}
			if (i_edcLoetprotokollElement.PRO_enmProtokollParameterTyp == ENUM_ProtokollParameterTyp.enmProductionResource)
			{
				SUB_AddProductionResource(i_edcUnitDataType, i_edcLoetprotokollElement);
			}
		}

		private void SUB_AddMeasuring(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, EDC_LoetprotokollElement i_edcElement)
		{
			measuringType i_edcMeasuring = FUN_fdcAddMeasuring(i_edcUnitDataType, i_edcElement);
			channelType channelType = FUN_fdcAddChannel(i_edcMeasuring, i_edcElement);
			SUB_AddSample(channelType, i_edcElement);
			if (!string.IsNullOrWhiteSpace(i_edcElement.PRO_strSollwert))
			{
				channelType.nominalValue = new nominalType
				{
					value = i_edcElement.PRO_strSollwert
				};
			}
			if (!string.IsNullOrWhiteSpace(i_edcElement.PRO_strToleranzPlus))
			{
				SUB_AddLimitHh(channelType, i_edcElement);
			}
			if (!string.IsNullOrWhiteSpace(i_edcElement.PRO_strToleranzMinus))
			{
				SUB_AddLimitLl(channelType, i_edcElement);
			}
		}

		private void SUB_AddLimitHh(channelType i_edcChannel, EDC_LoetprotokollElement i_edcElement)
		{
			i_edcChannel.limit_hh = new limitType();
			i_edcChannel.limit_hh.value = FUN_strToleranzWertErmitteln(i_edcChannel, i_edcElement, i_blnTolPlusNehmen: true);
			if (m_edcEinstellungsParamter.PRO_blnRelativeToleranzen)
			{
				i_edcChannel.limit_hh.relative = "1";
			}
		}

		private void SUB_AddLimitLl(channelType i_edcChannel, EDC_LoetprotokollElement i_edcElement)
		{
			i_edcChannel.limit_ll = new limitType();
			i_edcChannel.limit_ll.value = FUN_strToleranzWertErmitteln(i_edcChannel, i_edcElement, i_blnTolPlusNehmen: false);
			if (m_edcEinstellungsParamter.PRO_blnRelativeToleranzen)
			{
				i_edcChannel.limit_ll.relative = "1";
			}
		}

		private string FUN_strToleranzWertErmitteln(channelType i_edcChannel, EDC_LoetprotokollElement i_edcElement, bool i_blnTolPlusNehmen)
		{
			if (m_edcEinstellungsParamter.PRO_blnRelativeToleranzen)
			{
				if (i_blnTolPlusNehmen)
				{
					return i_edcElement.PRO_strToleranzPlus;
				}
				return i_edcElement.PRO_strToleranzMinus;
			}
			string text = (!i_blnTolPlusNehmen) ? ("-" + i_edcElement.PRO_strToleranzMinus) : i_edcElement.PRO_strToleranzPlus;
			nominalType nominalValue = i_edcChannel.nominalValue;
			if (nominalValue == null)
			{
				return text;
			}
			double num = FUN_dblNachkommaWert(nominalValue.value);
			double num2 = FUN_dblNachkommaWert(text);
			int i_i32AnzahlNachkommaZahlen = 0;
			try
			{
				i_i32AnzahlNachkommaZahlen = (from i_strWert in new string[2]
				{
					nominalValue.value,
					text
				}
				select i_strWert.Replace(',', '.')).Max((string i_strWert) => i_strWert.Split('.').Last().Length);
			}
			catch
			{
			}
			return EDC_ZahlenFormatHelfer.FUN_strWert(num + num2, i_i32AnzahlNachkommaZahlen);
		}

		private double FUN_dblNachkommaWert(string i_strWert)
		{
			if (string.IsNullOrWhiteSpace(i_strWert))
			{
				return 0.0;
			}
			if (!double.TryParse(i_strWert.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
			{
				return 0.0;
			}
			return result;
		}

		private void SUB_AddSample(channelType i_edcChannel, EDC_LoetprotokollElement i_edcElement)
		{
			if (i_edcChannel.lstSample == null)
			{
				i_edcChannel.lstSample = new List<sampleType>();
			}
			sampleType item = new sampleType
			{
				value = i_edcElement.PRO_strIstWert
			};
			i_edcChannel.lstSample.Add(item);
		}

		private measuringType FUN_fdcAddMeasuring(EDC_LoetprotokollZveiUnitData i_edcUnitDataTyp, EDC_LoetprotokollElement i_edcElement)
		{
			if (i_edcUnitDataTyp.lstMeasuring == null)
			{
				i_edcUnitDataTyp.lstMeasuring = new List<measuringType>();
			}
			measuringType measuringType = new measuringType();
			i_edcUnitDataTyp.lstMeasuring.Add(measuringType);
			return measuringType;
		}

		private channelType FUN_fdcAddChannel(measuringType i_edcMeasuring, EDC_LoetprotokollElement i_edcElement)
		{
			if (i_edcMeasuring.lstChannel == null)
			{
				i_edcMeasuring.lstChannel = new List<channelType>();
			}
			channelType channelType = new channelType
			{
				name = m_edcNamenMapper.FUN_strHoleMapping(i_edcElement.PRO_strIdentifier, i_edcElement.PRO_strName),
				UnitOfMeasure = i_edcElement.PRO_strEinheit
			};
			i_edcMeasuring.lstChannel.Add(channelType);
			return channelType;
		}

		private void SUB_AddProductionResource(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, EDC_LoetprotokollElement i_edcElement)
		{
			if (i_edcUnitDataType.productionResources == null)
			{
				i_edcUnitDataType.productionResources = new productionResourcesType();
			}
			SUB_AddResource(i_edcUnitDataType.productionResources, i_edcElement);
		}

		private void SUB_AddResource(productionResourcesType i_edcProductionResourcesType, EDC_LoetprotokollElement i_edcElement)
		{
			if (i_edcProductionResourcesType.lstResource == null)
			{
				i_edcProductionResourcesType.lstResource = new List<resourceType>();
			}
			resourceType item = new resourceType
			{
				type = m_edcNamenMapper.FUN_strHoleMapping(i_edcElement.PRO_strIdentifier, i_edcElement.PRO_strName),
				name = i_edcElement.PRO_strIstWert
			};
			i_edcProductionResourcesType.lstResource.Add(item);
		}

		private void SUB_AddProcessingParameters(EDC_LoetprotokollZveiUnitData i_edcUnitDataType, EDC_LoetprotokollElement i_edcElement)
		{
			if (i_edcUnitDataType.processingParameters == null)
			{
				i_edcUnitDataType.processingParameters = new processingParametersType();
			}
			SUB_AddParameters(i_edcUnitDataType.processingParameters, i_edcElement);
		}

		private void SUB_AddParameters(processingParametersType i_edcProcessingParameters, EDC_LoetprotokollElement i_edcElement)
		{
			if (i_edcProcessingParameters.lstParameter == null)
			{
				i_edcProcessingParameters.lstParameter = new List<parameterType>();
			}
			parameterType item = new parameterType
			{
				name = m_edcNamenMapper.FUN_strHoleMapping(i_edcElement.PRO_strIdentifier, i_edcElement.PRO_strName),
				value = i_edcElement.PRO_strIstWert,
				UnitOfMeasure = (string.IsNullOrWhiteSpace(i_edcElement.PRO_strEinheit) ? null : i_edcElement.PRO_strEinheit)
			};
			i_edcProcessingParameters.lstParameter.Add(item);
		}
	}
}
