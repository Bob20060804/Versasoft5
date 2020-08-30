using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Loetprotokoll;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Dienste.Loetprotokoll.Interfaces;
using Ersa.Platform.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Export(typeof(INF_LoetprotokollSerialisierungsDienst))]
	public class EDC_LoetprotokollSerialisierungsDienst : INF_LoetprotokollSerialisierungsDienst
	{
		[ImportMany]
		public IEnumerable<INF_LoetprotokollSerialisierer> PRO_enuLoetprotokollSerialisierer
		{
			get;
			set;
		}

		[Import]
		public INF_IODienst PRO_edcIoDienst
		{
			get;
			set;
		}

		[Import]
		public INF_VisuSettingsDienst PRO_edcVisuSettingsDienst
		{
			get;
			set;
		}

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		public void SUB_LoetprotokollSerialisieren(EDC_LoetprotokollDaten i_edcLoetprotokoll, EDC_LoetprotokollDateiEinstellungen i_edcDateiEinstellungen)
		{
			INF_LoetprotokollSerialisierer iNF_LoetprotokollSerialisierer = PRO_enuLoetprotokollSerialisierer.Single((INF_LoetprotokollSerialisierer i_edcSerialisierer) => i_edcSerialisierer.PRO_strSerialisiererName == i_edcDateiEinstellungen.PRO_strSerialisiererName);
			i_edcDateiEinstellungen.PRO_strPfadUndDateiName = FUN_strDateiPfadErmitteln(i_edcLoetprotokoll, iNF_LoetprotokollSerialisierer.PRO_strDefaultDateiEndung, i_edcDateiEinstellungen.PRO_blnTemporaeresUnterverzeichnisVerwenden);
			string directoryName = Path.GetDirectoryName(i_edcDateiEinstellungen.PRO_strPfadUndDateiName);
			if (!PRO_edcIoDienst.FUN_blnVerzeichnisExistiert(directoryName))
			{
				PRO_edcIoDienst.SUB_VerzeichnisErstellen(directoryName);
			}
			iNF_LoetprotokollSerialisierer.SUB_LoetprotokollSerialisieren(i_edcLoetprotokoll, i_edcDateiEinstellungen);
			if (i_edcDateiEinstellungen.PRO_blnTemporaeresUnterverzeichnisVerwenden)
			{
				string i_strZiel = FUN_strDateiPfadErmitteln(i_edcLoetprotokoll, iNF_LoetprotokollSerialisierer.PRO_strDefaultDateiEndung, i_edcTempUnterVerz: false);
				string pRO_strPfadUndDateiName = i_edcDateiEinstellungen.PRO_strPfadUndDateiName;
				SUB_DateiSicherVerschieben(pRO_strPfadUndDateiName, i_strZiel);
			}
		}

		private void SUB_DateiSicherVerschieben(string i_strQuelle, string i_strZiel)
		{
			if (!PRO_edcIoDienst.FUN_blnDateiExistiert(i_strZiel))
			{
				PRO_edcIoDienst.SUB_DateiVerschieben(i_strQuelle, i_strZiel, i_blnUeberschreiben: true);
			}
			else if (PRO_edcIoDienst.FUN_blnDateiExistiert(i_strQuelle))
			{
				PRO_edcIoDienst.SUB_DateiLoeschen(i_strQuelle);
			}
		}

		private string FUN_strDateiPfadErmitteln(EDC_LoetprotokollDaten i_edcLoetprotokoll, string i_strExtension, bool i_edcTempUnterVerz)
		{
			string text = string.Empty;
			if (i_edcLoetprotokoll.PRO_edcKopf.PRO_lstScannerCodes != null && i_edcLoetprotokoll.PRO_edcKopf.PRO_lstScannerCodes.Count > 0)
			{
				text = i_edcLoetprotokoll.PRO_edcKopf.PRO_lstScannerCodes[0].PRO_strIstWert;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = i_edcLoetprotokoll.PRO_edcKopf.PRO_edcProgrammName.PRO_strIstWert;
			}
			string text2 = string.Empty;
			if (i_edcLoetprotokoll.PRO_edcKopf.PRO_edcLaufendeNummer != null)
			{
				text2 = i_edcLoetprotokoll.PRO_edcKopf.PRO_edcLaufendeNummer.PRO_strIstWert;
			}
			string text3 = string.Format("PD_{0}_{1}_{2}{3}", text, DateTime.Now.ToString("yyyyMMdd_HHmmss"), text2, i_strExtension);
			string text4 = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			for (int i = 0; i < text4.Length; i++)
			{
				char c = text4[i];
				if (text3.Contains(c))
				{
					text3 = text3.Replace(c.ToString(), "_");
					Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
					PRO_edcLogger?.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, $"The character '{c}' has been replaced by '_' because it is not valid in file names", reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name);
				}
			}
			string path = PRO_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadLoetprotokolle");
			if (i_edcTempUnterVerz)
			{
				path = Path.Combine(path, "temp");
			}
			return Path.Combine(path, text3);
		}
	}
}
