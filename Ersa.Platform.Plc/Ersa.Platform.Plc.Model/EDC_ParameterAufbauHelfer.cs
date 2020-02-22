using Ersa.Platform.Common.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ersa.Platform.Plc.Model
{
    /// <summary>
    /// Setup Parameters Helper
    /// </summary>
	public static class EDC_ParameterAufbauHelfer
	{
        /// <summary>
        /// Should create parameter description
        /// </summary>
        /// <returns></returns>
		public static EDC_ParameterBeschreibung FUN_edcSollParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmStatisch,
				PRO_enmRichtung = ENUM_Richtung.enmVisuSps,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmSoll
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcSollMitAktualisierungParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmStatisch,
				PRO_enmRichtung = ENUM_Richtung.enmVisuSps,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmSollMitAktualisierung
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcAnfoParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmFluechtig,
				PRO_enmRichtung = ENUM_Richtung.enmVisuSps,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmAnfo
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcAnfoMitAktualisierungParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmFluechtig,
				PRO_enmRichtung = ENUM_Richtung.enmVisuSps,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmAnfoMitAktualisierung
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcKonfParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmStatisch,
				PRO_enmRichtung = ENUM_Richtung.enmVisuSps,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmKonf
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcStatParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmFluechtig,
				PRO_enmRichtung = ENUM_Richtung.enmSpsVisu,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmStat
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcDefaultVisuSpsParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmFluechtig,
				PRO_enmRichtung = ENUM_Richtung.enmVisuSps
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcDefaultSpsVisuParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmFluechtig,
				PRO_enmRichtung = ENUM_Richtung.enmSpsVisu,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmDefaultSpsVisu
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcSpsVisuOhneAktualisierungParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmFluechtig,
				PRO_enmRichtung = ENUM_Richtung.enmSpsVisu,
				PRO_enmParameterVerhalten = ENUM_ParameterVerhalten.enmUndefiniert
			};
		}

		public static EDC_ParameterBeschreibung FUN_edcDefaultStatischVisuSpsParameterBeschreibungErstellen()
		{
			return new EDC_ParameterBeschreibung
			{
				PRO_enmParameterPersistenz = ENUM_ParameterPersistenz.enmStatisch,
				PRO_enmRichtung = ENUM_Richtung.enmVisuSps
			};
		}

		public static IEnumerable<EDC_PrimitivParameter> FUN_lstFlacheKopienDerParameterErstellen(IEnumerable<EDC_PrimitivParameter> i_lstOriginaleListe)
		{
			if (i_lstOriginaleListe == null)
			{
				return null;
			}
			IList<EDC_PrimitivParameter> source = (i_lstOriginaleListe as IList<EDC_PrimitivParameter>) ?? i_lstOriginaleListe.ToList();
			if (source.Any())
			{
				return (from i_edcParameter in source
				select i_edcParameter.Clone()).Cast<EDC_PrimitivParameter>().ToList();
			}
			return new List<EDC_PrimitivParameter>();
		}

		public static void SUB_AenderungenAnParameternRueckgaengigMachen(IList<EDC_PrimitivParameter> i_lstOriginaleListe, IList<EDC_PrimitivParameter> i_lstTemporaereKopieDerListe)
		{
			if (i_lstOriginaleListe != null && i_lstTemporaereKopieDerListe != null)
			{
				int count = i_lstOriginaleListe.Count;
				if (count != i_lstTemporaereKopieDerListe.Count)
				{
					throw new InvalidOperationException("Cannot perform undo operation! Different number of elements!");
				}
				EDC_PrimitivParameterEqualityComparer comparer = new EDC_PrimitivParameterEqualityComparer();
				if (!i_lstOriginaleListe.SequenceEqual(i_lstTemporaereKopieDerListe, comparer))
				{
					throw new InvalidOperationException("Cannot perform undo operation! Parameter with different addresses on the same position detected!");
				}
				for (int i = 0; i < count; i++)
				{
					i_lstOriginaleListe[i].PRO_objAnzeigeWert = i_lstTemporaereKopieDerListe[i].PRO_objAnzeigeWert;
					i_lstOriginaleListe[i].PRO_objValue = i_lstTemporaereKopieDerListe[i].PRO_objValue;
				}
			}
		}

		[Obsolete("Besser FUN_edcArrayGenerischGenerieren verwenden und mit ArrayParameter<T> arbeiten.")]
		public static EDC_ArrayParameter FUN_edcArrayGenerieren<T>(int i_i32AnzahlElemente, bool i_blnBeginneBeiIndex1 = false) where T : EDC_StructParameter, new()
		{
			EDC_ArrayParameter eDC_ArrayParameter = new EDC_ArrayParameter();
			if (i_blnBeginneBeiIndex1)
			{
				for (int i = 1; i <= i_i32AnzahlElemente; i++)
				{
					EDC_ArrayParameter eDC_ArrayParameter2 = eDC_ArrayParameter;
					T val = new T();
					val.PRO_strNameKey = i.ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter2.SUB_ParameterHinzufuegen(val, i);
				}
			}
			else
			{
				for (int j = 0; j < i_i32AnzahlElemente; j++)
				{
					EDC_ArrayParameter eDC_ArrayParameter3 = eDC_ArrayParameter;
					T val2 = new T();
					val2.PRO_strNameKey = (j + 1).ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter3.SUB_ParameterHinzufuegen(val2);
				}
			}
			return eDC_ArrayParameter;
		}

		public static EDC_ArrayParameter<T> FUN_edcArrayGenerischGenerieren<T>(int i_i32AnzahlElemente, bool i_blnBeginneBeiIndex1 = false) where T : EDC_AdressRelevanterTeil, new()
		{
			EDC_ArrayParameter<T> eDC_ArrayParameter = new EDC_ArrayParameter<T>();
			if (i_blnBeginneBeiIndex1)
			{
				for (int i = 1; i <= i_i32AnzahlElemente; i++)
				{
					EDC_ArrayParameter<T> eDC_ArrayParameter2 = eDC_ArrayParameter;
					T val = new T();
					val.PRO_strNameKey = i.ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter2.SUB_ParameterHinzufuegen(val, i);
				}
			}
			else
			{
				for (int j = 0; j < i_i32AnzahlElemente; j++)
				{
					EDC_ArrayParameter<T> eDC_ArrayParameter3 = eDC_ArrayParameter;
					T val2 = new T();
					val2.PRO_strNameKey = (j + 1).ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter3.SUB_ParameterHinzufuegen(val2);
				}
			}
			return eDC_ArrayParameter;
		}

		[Obsolete("Besser FUN_edcArrayGenerischGenerieren verwenden und mit ArrayParameter<T> arbeiten.")]
		public static EDC_ArrayParameter FUN_edcArrayGenerieren<T>(int i_i32AnzahlElemente, int i_i32Dimension2) where T : EDC_StructParameter, new()
		{
			EDC_ArrayParameter eDC_ArrayParameter = new EDC_ArrayParameter();
			for (int i = 0; i < i_i32AnzahlElemente; i++)
			{
				eDC_ArrayParameter.SUB_ParameterHinzufuegen(FUN_edcArrayGenerieren<T>(i_i32Dimension2), i);
			}
			return eDC_ArrayParameter;
		}

		public static EDC_ArrayParameter<EDC_ArrayParameter<T>> FUN_edcArrayGenerischGenerieren<T>(int i_i32AnzahlElemente, int i_i32Dimension2) where T : EDC_AdressRelevanterTeil, new()
		{
			EDC_ArrayParameter<EDC_ArrayParameter<T>> eDC_ArrayParameter = new EDC_ArrayParameter<EDC_ArrayParameter<T>>();
			for (int i = 0; i < i_i32AnzahlElemente; i++)
			{
				eDC_ArrayParameter.SUB_ParameterHinzufuegen(FUN_edcArrayGenerischGenerieren<T>(i_i32Dimension2), i);
			}
			return eDC_ArrayParameter;
		}

		[Obsolete("Besser FUN_edcArrayGenerischGenerieren verwenden und mit ArrayParameter<T> arbeiten.")]
		public static EDC_ArrayParameter FUN_edcArrayGenerieren<T>(int i_i32AnzahlElemente, EDC_ParameterBeschreibung i_edcBeschreibung, bool i_blnInverseReihenfolge = false) where T : EDC_PrimitivParameter, new()
		{
			EDC_ArrayParameter eDC_ArrayParameter = new EDC_ArrayParameter();
			if (i_blnInverseReihenfolge)
			{
				for (int num = i_i32AnzahlElemente - 1; num > -1; num--)
				{
					EDC_ArrayParameter eDC_ArrayParameter2 = eDC_ArrayParameter;
					T val = new T();
					val.PRO_edcParameterBeschreibung = i_edcBeschreibung;
					val.PRO_strNameKey = (num + 1).ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter2.SUB_ParameterHinzufuegen(val, num);
				}
			}
			else
			{
				for (int i = 0; i < i_i32AnzahlElemente; i++)
				{
					EDC_ArrayParameter eDC_ArrayParameter3 = eDC_ArrayParameter;
					T val2 = new T();
					val2.PRO_edcParameterBeschreibung = i_edcBeschreibung;
					val2.PRO_strNameKey = (i + 1).ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter3.SUB_ParameterHinzufuegen(val2);
				}
			}
			return eDC_ArrayParameter;
		}

		public static EDC_ArrayParameter<T> FUN_edcArrayGenerischGenerieren<T>(int i_i32AnzahlElemente, EDC_ParameterBeschreibung i_edcBeschreibung, bool i_blnInverseReihenfolge = false) where T : EDC_PrimitivParameter, new()
		{
			EDC_ArrayParameter<T> eDC_ArrayParameter = new EDC_ArrayParameter<T>();
			if (i_blnInverseReihenfolge)
			{
				for (int num = i_i32AnzahlElemente - 1; num > -1; num--)
				{
					EDC_ArrayParameter<T> eDC_ArrayParameter2 = eDC_ArrayParameter;
					T val = new T();
					val.PRO_edcParameterBeschreibung = i_edcBeschreibung;
					val.PRO_strNameKey = (num + 1).ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter2.SUB_ParameterHinzufuegen(val, num);
				}
			}
			else
			{
				for (int i = 0; i < i_i32AnzahlElemente; i++)
				{
					EDC_ArrayParameter<T> eDC_ArrayParameter3 = eDC_ArrayParameter;
					T val2 = new T();
					val2.PRO_edcParameterBeschreibung = i_edcBeschreibung;
					val2.PRO_strNameKey = (i + 1).ToString(CultureInfo.InvariantCulture);
					eDC_ArrayParameter3.SUB_ParameterHinzufuegen(val2);
				}
			}
			return eDC_ArrayParameter;
		}

		[Obsolete("Besser FUN_edcKonfiguriertenArrayParameterMitStructsGenerischErstellen verwenden und mit ArrayParameter<T> arbeiten.")]
		public static EDC_ArrayParameter FUN_edcKonfiguriertenArrayParameterMitStructsErstellen<TKonfig, TInstanz>(IEnumerable<TKonfig> i_edcElementeKonfigs, Func<TKonfig, bool> i_delIstKonfiguriert, bool i_blnBeginneBeiIndex1 = false) where TKonfig : EDC_StructParameter where TInstanz : EDC_StructParameter, new()
		{
			IList<TKonfig> list = (i_edcElementeKonfigs as IList<TKonfig>) ?? i_edcElementeKonfigs.ToList();
			if (!list.Any(i_delIstKonfiguriert))
			{
				return null;
			}
			IEnumerable<int> enumerable = from i_edcElement in list
			where !i_delIstKonfiguriert(i_edcElement)
			select i_edcElement into i_edcBehaelter
			select (int)i_edcBehaelter.PRO_objAdressAnteil;
			EDC_ArrayParameter eDC_ArrayParameter = FUN_edcArrayGenerieren<TInstanz>(list.Count, i_blnBeginneBeiIndex1);
			foreach (int item in enumerable)
			{
				eDC_ArrayParameter.SUB_ParameterEntfernen(item);
			}
			return eDC_ArrayParameter;
		}

		public static EDC_ArrayParameter<TInstanz> FUN_edcKonfiguriertenArrayParameterMitStructsGenerischErstellen<TKonfig, TInstanz>(IEnumerable<TKonfig> i_edcElementeKonfigs, Func<TKonfig, bool> i_delIstKonfiguriert, bool i_blnBeginneBeiIndex1 = false) where TKonfig : EDC_StructParameter where TInstanz : EDC_StructParameter, new()
		{
			IList<TKonfig> list = (i_edcElementeKonfigs as IList<TKonfig>) ?? i_edcElementeKonfigs.ToList();
			if (!list.Any(i_delIstKonfiguriert))
			{
				return null;
			}
			IEnumerable<int> enumerable = from i_edcElement in list
			where !i_delIstKonfiguriert(i_edcElement)
			select i_edcElement into i_edcBehaelter
			select (int)i_edcBehaelter.PRO_objAdressAnteil;
			EDC_ArrayParameter<TInstanz> eDC_ArrayParameter = FUN_edcArrayGenerischGenerieren<TInstanz>(list.Count, i_blnBeginneBeiIndex1);
			foreach (int item in enumerable)
			{
				eDC_ArrayParameter.SUB_ParameterEntfernen(item);
			}
			return eDC_ArrayParameter;
		}

		public static void SUB_NameSuffixeFuerArrayParameterSetzen(EDC_ArrayParameter i_edcArrayParam, string i_strNameKey)
		{
			if (i_edcArrayParam != null)
			{
				foreach (EDC_AdressRelevanterTeil item in i_edcArrayParam.PRO_lstValue)
				{
					item.PRO_strNameSuffix = ((int)item.PRO_objAdressAnteil + 1).ToString(CultureInfo.InvariantCulture);
					item.PRO_strNameKey = i_strNameKey;
				}
			}
		}

		public static void SUB_ArrayParameterBenamenUndNummerieren(EDC_ArrayParameter i_edcArrayParameter, string i_strNameKey)
		{
			if (i_edcArrayParameter != null && i_edcArrayParameter.PRO_lstValue != null)
			{
				for (int i = 0; i < i_edcArrayParameter.PRO_lstValue.Count; i++)
				{
					i_edcArrayParameter.PRO_lstValue[i].PRO_strNameKey = i_strNameKey;
					i_edcArrayParameter.PRO_lstValue[i].PRO_strNameSuffix = (i + 1).ToString(CultureInfo.InvariantCulture);
				}
			}
		}

		public static void SUB_NameSuffixeFuerArrayParameterSetzen<T>(EDC_ArrayParameter<T> i_edcArrayParam, string i_strNameKey) where T : EDC_AdressRelevanterTeil
		{
			if (i_edcArrayParam != null)
			{
				foreach (T item in i_edcArrayParam.PRO_lstValue)
				{
					item.PRO_strNameSuffix = ((int)item.PRO_objAdressAnteil + 1).ToString(CultureInfo.InvariantCulture);
					item.PRO_strNameKey = i_strNameKey;
				}
			}
		}

		public static void SUB_ArrayParameterBenamenUndNummerieren<T>(EDC_ArrayParameter<T> i_edcArrayParameter, string i_strNameKey) where T : EDC_AdressRelevanterTeil
		{
			if (i_edcArrayParameter != null && i_edcArrayParameter.PRO_lstValue != null)
			{
				for (int i = 0; i < i_edcArrayParameter.PRO_lstValue.Count; i++)
				{
					i_edcArrayParameter.PRO_lstValue[i].PRO_strNameKey = i_strNameKey;
					i_edcArrayParameter.PRO_lstValue[i].PRO_strNameSuffix = (i + 1).ToString(CultureInfo.InvariantCulture);
				}
			}
		}
	}
}
