using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Mes.Interfaces;
using Ersa.Platform.Mes.Modell;
using Ersa.Platform.UI.Common.Interfaces;
using System.ComponentModel.Composition;
using System.IO;

namespace Ersa.Platform.Mes.Dienste
{
	[Export(typeof(INF_MesEinstellungenImportExportDienst))]
	public class EDC_MesEinstellungenImportExportDienst : INF_MesEinstellungenImportExportDienst
	{
		private const string mC_strJsonFilter = "JSON Files|*.json|All Files (*.*)|*.*";

		private const string mC_strJsonErweiterung = "json";

		private const string mC_strExportDateiname = "settings";

		[Import]
		public INF_IODienst PRO_edcIoDienst
		{
			get;
			set;
		}

		[Import("Ersa.JsonSerialisierer")]
		public INF_SerialisierungsDienst PRO_edcSerialisierer
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
		public INF_IoDialogHelfer PRO_edcIoDialogHelfer
		{
			get;
			set;
		}

		public T FUN_edcImport<T>()
		{
			string text = Path.Combine(PRO_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadExport"), "Mes");
			if (!PRO_edcIoDienst.FUN_blnVerzeichnisExistiert(text))
			{
				PRO_edcIoDienst.SUB_VerzeichnisErstellen(text);
			}
			string text2 = PRO_edcIoDialogHelfer.FUN_strOpenFileDialog(text, "JSON Files|*.json|All Files (*.*)|*.*");
			if (string.IsNullOrEmpty(text2))
			{
				return default(T);
			}
			string i_strFormatierterString = PRO_edcIoDienst.FUN_strDateiInhaltLesen(text2);
			return PRO_edcSerialisierer.FUN_objDeserialisieren<T>(i_strFormatierterString);
		}

		public void SUB_Export(EDC_MesTypEinstellung i_edcMesEinstellungen)
		{
			string text = Path.Combine(PRO_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadExport"), "Mes");
			if (!PRO_edcIoDienst.FUN_blnVerzeichnisExistiert(text))
			{
				PRO_edcIoDienst.SUB_VerzeichnisErstellen(text);
			}
			string text2 = PRO_edcIoDialogHelfer.FUN_strSaveFiledialog("JSON Files|*.json|All Files (*.*)|*.*", text, "json", "settings");
			if (!string.IsNullOrEmpty(text2))
			{
				string i_strDateiInhalt = PRO_edcSerialisierer.FUN_strSerialisieren(i_edcMesEinstellungen);
				PRO_edcIoDienst.SUB_DateiInhaltSchreiben(text2, i_strDateiInhalt);
			}
		}
	}
}
