using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Dienste.Interfaces;
using Ersa.Platform.Infrastructure.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace Ersa.Platform.Dienste
{
	[Export(typeof(INF_HilfeDienst))]
	public class EDC_HilfeDienst : INF_HilfeDienst
	{
		private const string mC_strMappingDatei = "Mapping.csv";

		private const string mC_strOnlineHelpVerzeichnis = "OnlineHelp";

		private const string mC_strDefaultHilfeSeite = "index";

		private const string mC_strDefaultSprache = "de-DE";

		private static IDictionary<string, string> ms_dicHilfeSeitenMapping;

		private readonly INF_IODienst m_edcIoDienst;

		private string m_strNameDerAktuellenCulture;

		[Import]
		public INF_VisuSettingsDienst PRO_edcVisuSettingsDienst
		{
			get;
			set;
		}

		[Import]
		public Ersa.Global.Dienste.Interfaces.INF_AppSettingsDienst PRO_edcAppSettingsDienst
		{
			get;
			set;
		}

		private IDictionary<string, string> PRO_dicHilfeSeitenMapping => ms_dicHilfeSeitenMapping ?? (ms_dicHilfeSeitenMapping = FUN_dicHilfeSeitenMappingErstellen());

		private string PRO_strPfadZumHilfeVerzeichnis => PRO_edcVisuSettingsDienst.FUN_strGlobalSettingWertErmitteln("PfadHilfe");

		[ImportingConstructor]
		public EDC_HilfeDienst(INF_IODienst i_edcIoDienst, IEventAggregator i_fdcEventAggregator)
		{
			m_edcIoDienst = i_edcIoDienst;
			m_strNameDerAktuellenCulture = "de-DE";
			i_fdcEventAggregator.GetEvent<EDC_SpracheGeaendertEvent>().Subscribe(delegate(EDC_SpracheGeaendertPayload i_edcPayload)
			{
				m_strNameDerAktuellenCulture = FUN_strSpracheLang(i_edcPayload.PRO_fdcNeueCultureInfo.TwoLetterISOLanguageName);
			});
		}

		public Uri FUN_fdcHilfeUriErmitteln(string i_strHilfeKey)
		{
			if (!PRO_dicHilfeSeitenMapping.TryGetValue(i_strHilfeKey, out string value))
			{
				value = "index";
			}
			string text = Path.Combine(PRO_strPfadZumHilfeVerzeichnis, "OnlineHelp", m_strNameDerAktuellenCulture, value + ".html");
			if (!m_edcIoDienst.FUN_blnDateiExistiert(text))
			{
				text = Path.Combine(PRO_strPfadZumHilfeVerzeichnis, "OnlineHelp", FUN_strSpracheLang("en"), value + ".html");
			}
			return new Uri(Path.GetFullPath(text));
		}

		public Uri FUN_fdcAnleitungUriErmitteln()
		{
			string text = PRO_edcAppSettingsDienst.FUN_strAppSettingErmitteln("ManualFilePath");
			if (text == null)
			{
				return null;
			}
			return new Uri(Path.GetFullPath(text));
		}

		private string FUN_strSpracheLang(string i_strSprache)
		{
			if (i_strSprache == "en")
			{
				return "en-GB";
			}
			if (i_strSprache == "cs")
			{
				return "cs-CZ";
			}
			return i_strSprache + "-" + i_strSprache.ToUpper();
		}

		private IDictionary<string, string> FUN_dicHilfeSeitenMappingErstellen()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string i_strDateiPfad = Path.Combine(PRO_strPfadZumHilfeVerzeichnis, "OnlineHelp", "Mapping.csv");
			foreach (string item in m_edcIoDienst.FUN_lstDateiInhaltLesen(i_strDateiPfad))
			{
				if (!string.IsNullOrWhiteSpace(item))
				{
					string[] array = item.Split(';');
					if (!dictionary.ContainsKey(array[0]))
					{
						dictionary.Add(array[0], array[1]);
					}
				}
			}
			return dictionary;
		}
	}
}
