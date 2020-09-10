using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Meldungen;
using Ersa.Platform.Dienste.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace Ersa.Platform.Dienste
{
	[Export(typeof(INF_MeldungsHilfeDienst))]
	public class EDC_MeldungsHilfeDienst : INF_MeldungsHilfeDienst
	{
		private readonly INF_IODienst m_edcIoDienst;

		private readonly INF_VisuSettingsDienst m_edcSettingsDienst;

		[ImportingConstructor]
		public EDC_MeldungsHilfeDienst(INF_IODienst i_edcIoDienst, INF_VisuSettingsDienst i_edcSettingsDienst)
		{
			m_edcIoDienst = i_edcIoDienst;
			m_edcSettingsDienst = i_edcSettingsDienst;
		}

		public Uri FUN_fdcErweiterteHilfeUriErmitteln(INF_Meldung i_edcMeldung, string i_strCultureName)
		{
			Uri uri = FUN_fdcHilfeUriErmitteln(i_edcMeldung, i_strCultureName);
			if (uri != null)
			{
				return uri;
			}
			uri = FUN_fdcHilfeUriErmitteln(i_edcMeldung, "en");
			if (uri != null)
			{
				return uri;
			}
			return FUN_fdcHilfeUriErmitteln(i_edcMeldung, "de");
		}

		private Uri FUN_fdcHilfeUriErmitteln(INF_Meldung i_edcMeldung, string i_strSprache)
		{
			string i_strSprache2 = FUN_strInternatTelVorwahl(i_strSprache);
			string strBasisPfad = FUN_strMeldungsHilfePfadErmitteln(i_strSprache2);
			if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(strBasisPfad))
			{
				return null;
			}
			string text = (from i_strDateiname in FUN_enuMoeglicheDateinamenErmittlen(i_edcMeldung)
			select FUN_enuDateiPfadeMitMoeglichenExtensionsErstellen(strBasisPfad, i_strDateiname)).Select(FUN_strNeusteVorhandeneDateiErmitteln).FirstOrDefault((string i_strDateiname) => !string.IsNullOrEmpty(i_strDateiname));
			if (!string.IsNullOrEmpty(text))
			{
				return new Uri(Path.GetFullPath(text));
			}
			return null;
		}

		private string FUN_strInternatTelVorwahl(string i_strSprache)
		{
			int num = 44;
			switch (i_strSprache)
			{
			case "de":
				num = 49;
				break;
			case "en":
				num = 44;
				break;
			case "fr":
				num = 33;
				break;
			}
			return $"{num:D3}";
		}

		private string FUN_strMeldungsHilfePfadErmitteln(string i_strSprache)
		{
			return Path.Combine(m_edcSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadHilfe"), i_strSprache, "help/Messages");
		}

		private IEnumerable<string> FUN_enuMoeglicheDateinamenErmittlen(INF_Meldung i_edcMeldung)
		{
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.{i_edcMeldung.PRO_i32MeldungsOrt1:D4}.{i_edcMeldung.PRO_i32MeldungsOrt2:D4}.{i_edcMeldung.PRO_i32MeldungsOrt3:D4}";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.XXXX.{i_edcMeldung.PRO_i32MeldungsOrt2:D4}.{i_edcMeldung.PRO_i32MeldungsOrt3:D4}";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.{i_edcMeldung.PRO_i32MeldungsOrt1:D4}.XXXX.{i_edcMeldung.PRO_i32MeldungsOrt3:D4}";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.{i_edcMeldung.PRO_i32MeldungsOrt1:D4}.{i_edcMeldung.PRO_i32MeldungsOrt2:D4}.XXXX";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.XXXX.XXXX.{i_edcMeldung.PRO_i32MeldungsOrt3:D4}";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.XXXX.{i_edcMeldung.PRO_i32MeldungsOrt2:D4}.XXXX";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.{i_edcMeldung.PRO_i32MeldungsOrt1:D4}.XXXX.XXXX";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.XXXX.XXXX.XXXX";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.{i_edcMeldung.PRO_i32MeldungsOrt1:D4}.{i_edcMeldung.PRO_i32MeldungsOrt2:D4}";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}.{i_edcMeldung.PRO_i32MeldungsOrt1:D4}";
			yield return $"{i_edcMeldung.PRO_i32MeldungsNummer:D4}";
		}

		private IEnumerable<string> FUN_enuDateiPfadeMitMoeglichenExtensionsErstellen(string i_strBasisPfad, string i_strDateiname)
		{
			return new string[3]
			{
				Path.Combine(i_strBasisPfad, i_strDateiname + ".htm"),
				Path.Combine(i_strBasisPfad, i_strDateiname + ".html"),
				Path.Combine(i_strBasisPfad, i_strDateiname + ".txt")
			};
		}

		private string FUN_strNeusteVorhandeneDateiErmitteln(IEnumerable<string> i_enuDateiPfade)
		{
			return (from i_strDatei in i_enuDateiPfade
			where m_edcIoDienst.FUN_blnDateiExistiert(i_strDatei)
			orderby m_edcIoDienst.FUN_dtmDateiDatumErmitteln(i_strDatei) descending
			select i_strDatei).FirstOrDefault();
		}
	}
}
