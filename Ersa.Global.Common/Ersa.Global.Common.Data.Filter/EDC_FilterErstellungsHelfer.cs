using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Global.Common.Data.Filter
{
	public static class EDC_FilterErstellungsHelfer
	{
		public const string gC_strOperatorGleich = "=";

		public const string gC_strOperatorKleiner = "<";

		public const string gC_strOperatorGroesser = ">";

		public const string gC_strOperatorUngleich = "<>";

		public const string gC_strFilterOperatorZwischen = "between";

		public const string gC_strFilterOperatorWie = "like";

		public const string gC_strOperatorIn = "in";

		public const string gC_strOperatorNotIn = "not in";

		public const string gC_strVerknuepfungUnd = "and";

		public const string gC_strVerknuepfungOder = "or";

		public const string gC_strParameterPrefixName = "Parameter";

		public const string gC_strAlleVerknuepfungen = "or|and";

		public const string gC_strStringFilterOperationen = "=|<>|in|not in|like";

		public const string gC_strNumericFilterOperationen = "=|<>|in|not in|<|>";

		public const string gC_strDateTimeFilterOperationen = "=|<>|<|>|between";

		public const string gC_strBooleanFilterOperationen = "=|<>";

		private static readonly Dictionary<string, ENUM_FilterOperatoren> ms_enmDictionaryFilterOperatoren = new Dictionary<string, ENUM_FilterOperatoren>
		{
			{
				"=",
				ENUM_FilterOperatoren.FilterOperatorGleich
			},
			{
				"<",
				ENUM_FilterOperatoren.FilterOperatorKleiner
			},
			{
				">",
				ENUM_FilterOperatoren.FilterOperatorGroesser
			},
			{
				"<>",
				ENUM_FilterOperatoren.FilterOperatorUngleich
			},
			{
				"between",
				ENUM_FilterOperatoren.FilterOperatorZwischen
			},
			{
				"like",
				ENUM_FilterOperatoren.FilterOperatorWie
			},
			{
				"in",
				ENUM_FilterOperatoren.FilterOperatorIn
			},
			{
				"not in",
				ENUM_FilterOperatoren.FilterOperatorNotIn
			}
		};

		private static readonly Dictionary<string, ENUM_FilterVerknuepfung> ms_enmDictionaryFilterVerknuepfungen = new Dictionary<string, ENUM_FilterVerknuepfung>
		{
			{
				"and",
				ENUM_FilterVerknuepfung.FilterVerknuepfungUnd
			},
			{
				"or",
				ENUM_FilterVerknuepfung.FilterVerknuepfungOder
			}
		};

		public static IEnumerable<ENUM_FilterOperatoren> FUN_enuErstelleOperatorenListe(string i_strDatentyp)
		{
			switch (i_strDatentyp)
			{
			case "String":
				return FUN_enuKonvertiereStringOperatoernZuEnumOperatoren("=|<>|in|not in|like");
			case "Int16":
			case "Int32":
			case "Int64":
			case "Single":
			case "Byte":
				return FUN_enuKonvertiereStringOperatoernZuEnumOperatoren("=|<>|in|not in|<|>");
			case "DateTime":
				return FUN_enuKonvertiereStringOperatoernZuEnumOperatoren("=|<>|<|>|between");
			case "Boolean":
				return FUN_enuKonvertiereStringOperatoernZuEnumOperatoren("=|<>");
			default:
				return FUN_enuKonvertiereStringOperatoernZuEnumOperatoren("=|<>");
			}
		}

		public static string FUN_strErstelleFilterWhereStatement(IEnumerable<STRUCT_FilterKonkret> i_lstFilterKonkret, Dictionary<string, object> i_fdcDictionary)
		{
			int i_i32AnzahlParameter = 0;
			string i_strWhereStatement = "Where";
			return FUN_strErstelleWhereStatementTeil(i_fdcDictionary, i_lstFilterKonkret, i_strWhereStatement, ref i_i32AnzahlParameter);
		}

		public static string FUN_strErstelleFilterOrderByStatement(IEnumerable<STRUCT_FilterKonkret> i_lstFilterKonkret)
		{
			string text = string.Empty;
			foreach (STRUCT_FilterKonkret item in from i_sttFilterKonkret in i_lstFilterKonkret
			orderby i_sttFilterKonkret.PRO_i32SortierReihenfolge
			select i_sttFilterKonkret)
			{
				if (item.PRO_i32SortierReihenfolge > 0)
				{
					string pRO_strTabellenname = item.PRO_strTabellenname;
					string pRO_strSpaltenName = item.PRO_strSpaltenName;
					text = (string.IsNullOrEmpty(text) ? "Order By" : $"{text},");
					text = $"{text} {pRO_strTabellenname}.{pRO_strSpaltenName}";
				}
			}
			return text;
		}

		public static List<string> FUN_lstSplittereStringInListe(string i_strOperatorenListe)
		{
			return (from i_strOperator in i_strOperatorenListe.Split('|')
			select i_strOperator.Trim()).ToList();
		}

		public static ENUM_FilterOperatoren FUN_enmKonvertiereStringOperatorZuEnumOperator(string i_strOperator)
		{
			if (ms_enmDictionaryFilterOperatoren.TryGetValue(i_strOperator, out ENUM_FilterOperatoren value))
			{
				return value;
			}
			throw new ArgumentOutOfRangeException("i_strOperator", $"The operator '{i_strOperator}' is invalid or not supported.");
		}

		public static IEnumerable<ENUM_FilterOperatoren> FUN_enuKonvertiereStringOperatoernZuEnumOperatoren(string i_strOperatorenListe)
		{
			return FUN_lstSplittereStringInListe(i_strOperatorenListe).Select(FUN_enmKonvertiereStringOperatorZuEnumOperator);
		}

		public static string FUN_strKonvertiereEnumOperatorZuStringOperator(ENUM_FilterOperatoren i_enmFilterOperator)
		{
			string key = ms_enmDictionaryFilterOperatoren.FirstOrDefault((KeyValuePair<string, ENUM_FilterOperatoren> i_strValue) => i_strValue.Value == i_enmFilterOperator).Key;
			if (key == null)
			{
				throw new ArgumentOutOfRangeException("i_enmFilterOperator", $"The operator '{i_enmFilterOperator}' is invalid or not supported.");
			}
			return key;
		}

		public static ENUM_FilterVerknuepfung FUN_enmKonvertiereStringVerknuepfungZuEnumVerknuepfung(string i_strVerknuepfung)
		{
			if (ms_enmDictionaryFilterVerknuepfungen.TryGetValue(i_strVerknuepfung, out ENUM_FilterVerknuepfung value))
			{
				return value;
			}
			throw new ArgumentOutOfRangeException("i_strVerknuepfung", $"The operator '{i_strVerknuepfung}' is invalid or not supported.");
		}

		public static string FUN_strKonvertiereEnumVerknuepfungZuStringVerknuepfung(ENUM_FilterVerknuepfung i_enmFilterVerknuepfung)
		{
			string key = ms_enmDictionaryFilterVerknuepfungen.FirstOrDefault((KeyValuePair<string, ENUM_FilterVerknuepfung> i_strValue) => i_strValue.Value == i_enmFilterVerknuepfung).Key;
			if (key == null)
			{
				throw new ArgumentOutOfRangeException("i_enmFilterVerknuepfung", $"The operator '{i_enmFilterVerknuepfung}' is invalid or not supported.");
			}
			return key;
		}

		public static IEnumerable<ENUM_FilterVerknuepfung> FUN_enuKonvertiereStringVerknuepfungenZuEnumVerknuepfungen(string i_strVerknuepfungsListe)
		{
			return FUN_lstSplittereStringInListe(i_strVerknuepfungsListe).Select(FUN_enmKonvertiereStringVerknuepfungZuEnumVerknuepfung);
		}

		private static string FUN_strErstelleWhereStatementTeil(Dictionary<string, object> i_fdcDictionary, IEnumerable<STRUCT_FilterKonkret> i_lstFilterKonkret, string i_strWhereStatement, ref int i_i32AnzahlParameter)
		{
			IOrderedEnumerable<STRUCT_FilterKonkret> orderedEnumerable = from i_sttFilterKonkret in i_lstFilterKonkret
			orderby i_sttFilterKonkret.PRO_i32FilterPosition
			select i_sttFilterKonkret;
			List<STRUCT_FilterKonkret> list = new List<STRUCT_FilterKonkret>();
			List<STRUCT_FilterKonkret> list2 = new List<STRUCT_FilterKonkret>();
			foreach (STRUCT_FilterKonkret item in orderedEnumerable)
			{
				if (item.PRO_i32FilterPosition == 0)
				{
					list2.Add(item);
				}
				else
				{
					list.Add(item);
				}
			}
			list.AddRange(list2);
			foreach (STRUCT_FilterKonkret item2 in list)
			{
				string pRO_strTabellenname = item2.PRO_strTabellenname;
				string pRO_strSpaltenName = item2.PRO_strSpaltenName;
				if (!string.IsNullOrEmpty(pRO_strTabellenname) && !string.IsNullOrEmpty(pRO_strSpaltenName))
				{
					if (!"Where".Equals(i_strWhereStatement) && !string.IsNullOrEmpty(i_strWhereStatement))
					{
						i_strWhereStatement = $"{i_strWhereStatement} {FUN_strKonvertiereEnumVerknuepfungZuStringVerknuepfung(item2.PRO_edcFilterVerknüpfung)}";
					}
					i_strWhereStatement = string.Format((item2.PRO_edcFilterOperation == ENUM_FilterOperatoren.FilterOperatorWie) ? "{0} UPPER({1}.{2}) {3}" : "{0} {1}.{2} {3}", i_strWhereStatement, pRO_strTabellenname, pRO_strSpaltenName, FUN_strErstelleOperatorenTeil(item2, ref i_i32AnzahlParameter, i_fdcDictionary));
				}
				if (item2.PRO_lstKonkreteFilter != null)
				{
					IList<STRUCT_FilterKonkret> list3 = (item2.PRO_lstKonkreteFilter as IList<STRUCT_FilterKonkret>) ?? item2.PRO_lstKonkreteFilter.ToList();
					if (list3.Any())
					{
						i_strWhereStatement = $"{i_strWhereStatement} {FUN_strKonvertiereEnumVerknuepfungZuStringVerknuepfung(item2.PRO_edcListenVerknüpfung)} ({FUN_strErstelleWhereStatementTeil(i_fdcDictionary, list3, string.Empty, ref i_i32AnzahlParameter)})";
					}
				}
			}
			return i_strWhereStatement;
		}

		private static string FUN_strErstelleOperatorenTeil(STRUCT_FilterKonkret i_sttFilterKonkret, ref int i_i32AnzahlParameter, Dictionary<string, object> i_fdcDictionary)
		{
			string result = string.Empty;
			ENUM_FilterOperatoren pRO_edcFilterOperation = i_sttFilterKonkret.PRO_edcFilterOperation;
			switch (pRO_edcFilterOperation)
			{
			case ENUM_FilterOperatoren.FilterOperatorGleich:
			case ENUM_FilterOperatoren.FilterOperatorUngleich:
			case ENUM_FilterOperatoren.FilterOperatorGroesser:
			case ENUM_FilterOperatoren.FilterOperatorKleiner:
			{
				string arg = FUN_strErzeugeParameterNameUndFuegeInDictionaryEin(ref i_i32AnzahlParameter, i_fdcDictionary, i_sttFilterKonkret.PRO_lstExacterWerte.First(), i_sttFilterKonkret.PRO_strDatentyp);
				result = $"{FUN_strKonvertiereEnumOperatorZuStringOperator(pRO_edcFilterOperation)} @{arg}";
				break;
			}
			case ENUM_FilterOperatoren.FilterOperatorWie:
			{
				string i_objExacterWert = $"%{i_sttFilterKonkret.PRO_lstExacterWerte.First()}%";
				string arg = FUN_strErzeugeParameterNameUndFuegeInDictionaryEin(ref i_i32AnzahlParameter, i_fdcDictionary, i_objExacterWert, i_sttFilterKonkret.PRO_strDatentyp);
				result = $"{FUN_strKonvertiereEnumOperatorZuStringOperator(pRO_edcFilterOperation)} UPPER(@{arg})";
				break;
			}
			case ENUM_FilterOperatoren.FilterOperatorZwischen:
			{
				string arg = FUN_strErzeugeParameterNameUndFuegeInDictionaryEin(ref i_i32AnzahlParameter, i_fdcDictionary, i_sttFilterKonkret.PRO_objUntererWert, i_sttFilterKonkret.PRO_strDatentyp);
				string arg2 = FUN_strErzeugeParameterNameUndFuegeInDictionaryEin(ref i_i32AnzahlParameter, i_fdcDictionary, i_sttFilterKonkret.PRO_objObererWert, i_sttFilterKonkret.PRO_strDatentyp);
				result = $"{FUN_strKonvertiereEnumOperatorZuStringOperator(pRO_edcFilterOperation)} @{arg} and @{arg2}";
				break;
			}
			case ENUM_FilterOperatoren.FilterOperatorIn:
			case ENUM_FilterOperatoren.FilterOperatorNotIn:
				result = $"{FUN_strKonvertiereEnumOperatorZuStringOperator(pRO_edcFilterOperation)} ({FUN_strErstelleWerteListenTeil(i_sttFilterKonkret.PRO_lstExacterWerte, ref i_i32AnzahlParameter, i_fdcDictionary, i_sttFilterKonkret.PRO_strDatentyp)})";
				break;
			}
			return result;
		}

		private static string FUN_strErstelleWerteListenTeil(IEnumerable<object> i_enmExakteWerte, ref int i_i32AnzahlParameter, Dictionary<string, object> i_fdcDictionary, string i_strParameterType)
		{
			string text = string.Empty;
			foreach (object item in i_enmExakteWerte)
			{
				string arg = FUN_strErzeugeParameterNameUndFuegeInDictionaryEin(ref i_i32AnzahlParameter, i_fdcDictionary, item, i_strParameterType);
				if (!string.IsNullOrEmpty(text))
				{
					text = $"{text},";
				}
				text = $"{text} @{arg}";
			}
			return text;
		}

		private static string FUN_strErzeugeParameterNameUndFuegeInDictionaryEin(ref int i_i32AnzahlParameter, Dictionary<string, object> i_fdcDictionary, object i_objExacterWert, string i_strParameterType)
		{
			string text = "Parameter" + i_i32AnzahlParameter;
			switch (i_strParameterType)
			{
			case "Int16":
				i_fdcDictionary.Add(text, Convert.ToInt16(i_objExacterWert));
				break;
			case "int":
			case "Int32":
				i_fdcDictionary.Add(text, Convert.ToInt32(i_objExacterWert));
				break;
			case "Int64":
				i_fdcDictionary.Add(text, Convert.ToInt64(i_objExacterWert));
				break;
			case "Single":
				i_fdcDictionary.Add(text, Convert.ToSingle(i_objExacterWert));
				break;
			default:
				i_fdcDictionary.Add(text, i_objExacterWert);
				break;
			}
			i_i32AnzahlParameter++;
			return text;
		}
	}
}
