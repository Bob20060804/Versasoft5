using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ersa.Platform.Common.Model.Extensions
{
	public static class EDC_AdressRelevanterTeilExtensions
	{
		public static T FUN_edcParentErmitteln<T>(this EDC_AdressRelevanterTeil i_edcAdressRelevanterTeil) where T : EDC_AdressRelevanterTeil
		{
			if (i_edcAdressRelevanterTeil.PRO_edcParent == null)
			{
				return null;
			}
			if (i_edcAdressRelevanterTeil.PRO_edcParent is T)
			{
				return (T)i_edcAdressRelevanterTeil.PRO_edcParent;
			}
			return i_edcAdressRelevanterTeil.PRO_edcParent.FUN_edcParentErmitteln<T>();
		}

		public static bool FUN_blnHatAenderung(this EDC_AdressRelevanterTeil i_edcAdressRelevanterTeil)
		{
			return (from i_edcParam in i_edcAdressRelevanterTeil.FUN_enuParameterErmitteln()
			where i_edcParam.PRO_edcParameterBeschreibung.PRO_enmParameterVerhalten != ENUM_ParameterVerhalten.enmStat
			select i_edcParam).Any((EDC_PrimitivParameter i_edcParam) => i_edcParam.PRO_blnHatAenderung);
		}

		public static IEnumerable<EDC_PrimitivParameter> FUN_lstGeaenderteParameterErmitteln(this EDC_AdressRelevanterTeil i_edcAdressRelevanterTeil)
		{
			return from i_edcParam in i_edcAdressRelevanterTeil.FUN_enuParameterErmitteln()
			where i_edcParam.PRO_blnHatAenderung
			select i_edcParam;
		}

		public static IList<EDC_LokalisierungsKeyContainer> FUN_enuLokalisierungsKeysErmitteln<T>(this EDC_AdressRelevanterTeil i_edcAdressRelevanterTeil) where T : EDC_AdressRelevanterTeil
		{
			return i_edcAdressRelevanterTeil.FUN_enuLokalisierungsKeysErmitteln((EDC_AdressRelevanterTeil i_edcAdressTeil) => !(i_edcAdressTeil is T));
		}

		public static IList<EDC_LokalisierungsKeyContainer> FUN_enuLokalisierungsKeysErmitteln(this EDC_AdressRelevanterTeil i_edcAdressRelevanterTeil, EDC_AdressRelevanterTeil i_edcParent)
		{
			return i_edcAdressRelevanterTeil.FUN_enuLokalisierungsKeysErmitteln((EDC_AdressRelevanterTeil i_edcAdressTeil) => i_edcAdressTeil != i_edcParent);
		}

		public static IEnumerable<EDC_PrimitivParameter> FUN_enuParameterErmitteln(this EDC_AdressRelevanterTeil i_edcAdressRelevanterTeil)
		{
			return i_edcAdressRelevanterTeil.FUN_enuElementeHolen().OfType<EDC_PrimitivParameter>();
		}

		public static void SUB_AenderungenSignalisieren(this EDC_AdressRelevanterTeil i_edcAdressTeil)
		{
			foreach (EDC_StructParameter item in i_edcAdressTeil.FUN_enuElementeHolen().OfType<EDC_StructParameter>())
			{
				item.SUB_AenderungSignalisieren();
			}
		}

		[Obsolete("Bitte Ersa.Global.Common.Extensions.EDC_CollectionExtensions.FUN_enuUnion verwenden")]
		public static IEnumerable<EDC_AdressRelevanterTeil> FUN_enuUnion(this EDC_AdressRelevanterTeil i_edcFirst, params EDC_AdressRelevanterTeil[] ia_edcSecond)
		{
			return new EDC_AdressRelevanterTeil[1]
			{
				i_edcFirst
			}.Union(ia_edcSecond);
		}

		[Obsolete("Bitte Ersa.Global.Common.Extensions.EDC_CollectionExtensions.FUN_enuUnion verwenden")]
		public static IEnumerable<EDC_AdressRelevanterTeil> FUN_enuUnion(this EDC_AdressRelevanterTeil i_edcFirst, IEnumerable<EDC_AdressRelevanterTeil> i_enuSecond)
		{
			return new EDC_AdressRelevanterTeil[1]
			{
				i_edcFirst
			}.Union(i_enuSecond);
		}

		public static IEnumerable<EDC_AdressRelevanterTeil> FUN_enuAdressTeileMitAttributErmitteln<T>(this EDC_AdressRelevanterTeil i_edcAdressTeil) where T : Attribute
		{
			return (from i_edcTeil in i_edcAdressTeil.FUN_enuElementeHolen()
			where !(i_edcTeil is EDC_PrimitivParameter)
			select i_edcTeil).SelectMany((EDC_AdressRelevanterTeil i_edcTeil) => i_edcTeil.FUN_enuErmittleAdressRelevanteTeileMitAttribut<T>());
		}

		public static IEnumerable<EDC_AdressRelevanterTeil> FUN_enuAdressTeileMitAttributErmitteln<T>(this EDC_AdressRelevanterTeil i_edcAdressTeil, Func<T, bool> i_delIstRelevant) where T : Attribute
		{
			return (from i_edcTeil in i_edcAdressTeil.FUN_enuElementeHolen()
			where !(i_edcTeil is EDC_PrimitivParameter)
			select i_edcTeil).SelectMany((EDC_AdressRelevanterTeil i_edcTeil) => i_edcTeil.FUN_enuErmittleAdressRelevanteTeileMitAttribut(i_delIstRelevant));
		}

		private static IEnumerable<EDC_AdressRelevanterTeil> FUN_enuErmittleAdressRelevanteTeileMitAttribut<T>(this EDC_AdressRelevanterTeil i_edcTeil) where T : Attribute
		{
			return (from i_fdcPropertyInfo in i_edcTeil.FUN_enuErmittlePropertiesMitAttribut<T>()
			select i_fdcPropertyInfo.GetValue(i_edcTeil, null) into i_edcParameter
			where i_edcParameter != null
			select i_edcParameter).Cast<EDC_AdressRelevanterTeil>();
		}

		private static IEnumerable<EDC_AdressRelevanterTeil> FUN_enuErmittleAdressRelevanteTeileMitAttribut<T>(this EDC_AdressRelevanterTeil i_edcTeil, Func<T, bool> i_delRelevant) where T : Attribute
		{
			return (from i_fdcProperty in i_edcTeil.FUN_enuErmittlePropertiesMitAttribut<T>().Where(delegate(PropertyInfo i_fdcProperty)
			{
				T customAttribute = i_fdcProperty.GetCustomAttribute<T>();
				if (customAttribute != null)
				{
					return i_delRelevant(customAttribute);
				}
				return false;
			})
			select i_fdcProperty.GetValue(i_edcTeil, null) into i_objParam
			where i_objParam != null
			select i_objParam).Cast<EDC_AdressRelevanterTeil>();
		}

		private static IEnumerable<PropertyInfo> FUN_enuErmittlePropertiesMitAttribut<T>(this EDC_AdressRelevanterTeil i_edcTeil) where T : Attribute
		{
			return from i_fdcProperty in i_edcTeil.GetType().GetProperties()
			where i_fdcProperty.IsDefined(typeof(T), inherit: false)
			select i_fdcProperty;
		}

		private static IList<EDC_LokalisierungsKeyContainer> FUN_enuLokalisierungsKeysErmitteln(this EDC_AdressRelevanterTeil i_edcAdressRelevanterTeil, Predicate<EDC_AdressRelevanterTeil> i_delSollAufnehmen)
		{
			EDC_AdressRelevanterTeil eDC_AdressRelevanterTeil = i_edcAdressRelevanterTeil;
			List<EDC_LokalisierungsKeyContainer> list = new List<EDC_LokalisierungsKeyContainer>();
			while (eDC_AdressRelevanterTeil != null && i_delSollAufnehmen(eDC_AdressRelevanterTeil))
			{
				if (!string.IsNullOrWhiteSpace(eDC_AdressRelevanterTeil.PRO_strNameKey) || !string.IsNullOrWhiteSpace(eDC_AdressRelevanterTeil.PRO_strNameSuffix))
				{
					EDC_LokalisierungsKeyContainer edcKey = new EDC_LokalisierungsKeyContainer
					{
						PRO_strLocKey = eDC_AdressRelevanterTeil.PRO_strNameKey,
						PRO_strSuffix = eDC_AdressRelevanterTeil.PRO_strNameSuffix
					};
					if (list.FindIndex(delegate(EDC_LokalisierungsKeyContainer i_edcElem)
					{
						if (i_edcElem.PRO_strLocKey == edcKey.PRO_strLocKey)
						{
							return i_edcElem.PRO_strSuffix == edcKey.PRO_strSuffix;
						}
						return false;
					}) < 0)
					{
						list.Add(edcKey);
					}
				}
				eDC_AdressRelevanterTeil = eDC_AdressRelevanterTeil.PRO_edcParent;
			}
			list.Reverse();
			return list;
		}
	}
}
