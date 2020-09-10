using Ersa.Platform.Common.Mes;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.Mes.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Mes.Dienste
{
	/// <summary>
	/// MES参数验证助手
	/// </summary>
	[Export(typeof(INF_MesArgumenteValidierungsHelfer))]
	public class EDC_MesArgumenteValidierungsHelfer : INF_MesArgumenteValidierungsHelfer
	{
		private const string mC_strFehlerMaschinendatenNull = "11_1705";

		private const string mC_strFehlerPflichtargument = "11_1706";

		private const string mC_strFehlerDatentypFalsch = "11_1707";

		private const string mC_strFehlerArgumentNullOderLeer = "11_1708";

		[Import]
		public INF_LokalisierungsDienst PRO_edcLokalisierungsDienst
		{
			get;
			set;
		}

		public void SUB_PruefeArgumente(ENUM_MesFunktionen i_enmFunktion, EDC_MesMaschinenDaten i_edcMesMaschinenDaten)
		{
			if (i_edcMesMaschinenDaten == null)
			{
				throw new ArgumentException(PRO_edcLokalisierungsDienst.FUN_strText("11_1705"), "i_edcMesMaschinenDaten");
			}
			ENUM_MesMaschinenDatenArgumente[] i_enuPflichtArgumente;
			switch (i_enmFunktion)
			{
			case ENUM_MesFunktionen.OeeZyklischSenden:
				return;
			case ENUM_MesFunktionen.MeldungAufgetretenSenden:
				return;
			case ENUM_MesFunktionen.MeldungQuittiertSenden:
				return;
			case ENUM_MesFunktionen.MeldungAutomatischQuittiertSenden:
				return;
			case ENUM_MesFunktionen.MeldungZurueckgesetztSenden:
				return;
			case ENUM_MesFunktionen.AngefordertesRezeptBestaetigenSenden:
				return;
			case ENUM_MesFunktionen.RezeptAenderungSenden:
				return;
			case ENUM_MesFunktionen.BadBoardInformationenAnfordern:
				return;
			case ENUM_MesFunktionen.StartPcbBearbeitungSenden:
				return;
			case ENUM_MesFunktionen.AuslaufFreigabeAnfordern:
				return;
			case ENUM_MesFunktionen.EndePcbBearbeitungSenden:
				return;
			case ENUM_MesFunktionen.RuestmaterialAenderungAnfordern:
				return;
			case ENUM_MesFunktionen.RuestwerkzeugAenderungAnfordern:
				return;
			case ENUM_MesFunktionen.RuestwerkzeugAbmeldungSenden:
				return;
			case ENUM_MesFunktionen.RuestwerkzeugAnmeldungSenden:
				return;
			case ENUM_MesFunktionen.PingSenden:
				return;
			case ENUM_MesFunktionen.OeeAenderungSenden:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[2]
				{
					ENUM_MesMaschinenDatenArgumente.OeeAenderungSendenCode,
					ENUM_MesMaschinenDatenArgumente.OeeAenderungSendenName
				};
				break;
			case ENUM_MesFunktionen.RezeptAnfordern:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.RezeptAnfordernCodes
				};
				break;
			case ENUM_MesFunktionen.EinlaufFreigabeAnfordern:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.EinlaufFreigabeAnfordernCodes
				};
				break;
			case ENUM_MesFunktionen.PcbProzessParameterSenden:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.PcbProzessParameterSendenProtokoll
				};
				break;
			case ENUM_MesFunktionen.RuestmaterialAbmeldungSenden:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[2]
				{
					ENUM_MesMaschinenDatenArgumente.RuestmaterialAbmeldungSendenMaterialnummer,
					ENUM_MesMaschinenDatenArgumente.RuestmaterialAbmeldungSendenPosition
				};
				break;
			case ENUM_MesFunktionen.RuestmaterialAnmeldungSenden:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[2]
				{
					ENUM_MesMaschinenDatenArgumente.RuestmaterialAnmeldungSendenMaterialnummer,
					ENUM_MesMaschinenDatenArgumente.RuestmaterialAnmeldungSendenPosition
				};
				break;
			case ENUM_MesFunktionen.UserLoginAnfordern:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.LoginUserId
				};
				break;
			case ENUM_MesFunktionen.UserLogoutSenden:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.LogOutUserId
				};
				break;
			case ENUM_MesFunktionen.SendConveyorWidth:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.ConveyorWidth
				};
				break;
			case ENUM_MesFunktionen.CarrierUnassignSenden:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.CarrierUnassignSendenProtokoll
				};
				break;
			case ENUM_MesFunktionen.SendCodes:
				i_enuPflichtArgumente = new ENUM_MesMaschinenDatenArgumente[1]
				{
					ENUM_MesMaschinenDatenArgumente.SendCodesCode
				};
				break;
			default:
				throw new ArgumentOutOfRangeException("i_enmFunktion", i_enmFunktion.ToString(), null);
			}
			SUB_PruefePflichtargumente(i_edcMesMaschinenDaten, i_enuPflichtArgumente);
			SUB_PruefeDatentypen(i_edcMesMaschinenDaten);
		}

		private void SUB_PruefePflichtargumente(EDC_MesMaschinenDaten i_edcMesMaschinenDaten, IEnumerable<ENUM_MesMaschinenDatenArgumente> i_enuPflichtArgumente)
		{
			foreach (ENUM_MesMaschinenDatenArgumente item in i_enuPflichtArgumente)
			{
				if (!i_edcMesMaschinenDaten.PRO_dicArgumente.ContainsKey(item))
				{
					throw new ArgumentException(string.Format(PRO_edcLokalisierungsDienst.FUN_strText("11_1706"), item), "edcMaschinenDatenArgument");
				}
			}
		}

		private void SUB_PruefeDatentypen(EDC_MesMaschinenDaten i_edcMesMaschinenDaten)
		{
			foreach (ENUM_MesMaschinenDatenArgumente key in i_edcMesMaschinenDaten.PRO_dicArgumente.Keys)
			{
				Type type = key.FUN_objHoleTyp();
				if (i_edcMesMaschinenDaten.PRO_dicArgumente[key] == null)
				{
					throw new ArgumentException(string.Format(PRO_edcLokalisierungsDienst.FUN_strText("11_1706"), key), "enmArgument");
				}
				if (i_edcMesMaschinenDaten.PRO_dicArgumente[key].GetType() == typeof(JArray))
				{
					break;
				}
				if (i_edcMesMaschinenDaten.PRO_dicArgumente[key].GetType() != type && !type.IsInstanceOfType(i_edcMesMaschinenDaten.PRO_dicArgumente[key]))
				{
					throw new ArgumentException(string.Format(PRO_edcLokalisierungsDienst.FUN_strText("11_1707"), key, type), "enmArgument");
				}
				if (type == typeof(string) && string.IsNullOrEmpty((string)i_edcMesMaschinenDaten.PRO_dicArgumente[key]))
				{
					throw new ArgumentException(string.Format(PRO_edcLokalisierungsDienst.FUN_strText("11_1708"), key), "enmArgument");
				}
			}
		}
	}
}
