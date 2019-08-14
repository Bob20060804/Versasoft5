using Ersa.Platform.Common.Data.Aoi;
using Ersa.Platform.Common.Data.Cad;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public class EDC_LoetprogrammStand
	{
		public int PRO_i32SetNummer
		{
			get;
			set;
		}

		public string PRO_strKommentar
		{
			get;
			set;
		}

		public bool PRO_blnIstValide
		{
			get;
			set;
		}

		public EDC_Ecp3Bilddaten PRO_edcEcp3Bilddaten
		{
			get;
			set;
		}

		public EDC_LoetprogrammData PRO_edcProgrammInfo
		{
			get;
			set;
		}

		public IEnumerable<EDC_LoetprogrammParameterData> PRO_enuParameter
		{
			get;
			set;
		}

		public IEnumerable<EDC_LoetprogrammSatzDatenData> PRO_enuSatzdaten
		{
			get;
			set;
		}

		public IEnumerable<EDC_LoetprogrammNutzenData> PRO_enuNutzenParameter
		{
			get;
			set;
		}

		public IEnumerable<EDC_LoetprogrammEcp3DatenData> PRO_enuEcp3Daten
		{
			get;
			set;
		}

		public IEnumerable<EDC_CadVerbotenerBereichData> PRO_enuCadVerboteneBereiche
		{
			get;
			set;
		}

		public IEnumerable<EDC_CadAblaufSchrittData> PRO_enuCadAblaufSchritte
		{
			get;
			set;
		}

		public IEnumerable<EDC_CadCncSchrittData> PRO_enuCadCncSchritte
		{
			get;
			set;
		}

		public IEnumerable<EDC_CadRoutenSchrittData> PRO_enuCadRoutenSchritte
		{
			get;
			set;
		}

		public IEnumerable<EDC_CadRoutenData> PRO_enuCadRoutenDaten
		{
			get;
			set;
		}

		public IEnumerable<EDC_CadBewegungsGruppenData> PRO_enuCadBewegungsGruppen
		{
			get;
			set;
		}

		public IEnumerable<EDC_CadBildData> PRO_enuCadBilder
		{
			get;
			set;
		}

		public string PRO_strCadEinstellungen
		{
			get;
			set;
		}

		public string PRO_strCadDaten
		{
			get;
			set;
		}

		public string PRO_strAoiEinstellungen
		{
			get;
			set;
		}

		public string PRO_strAoiDaten
		{
			get;
			set;
		}

		public IEnumerable<EDC_AoiSchrittData> PRO_enuAoiSchritte
		{
			get;
			set;
		}

		public IDictionary<ENUM_BildVerwendung, EDC_FiducialBilddaten> PRO_dicFiducialBilder
		{
			get;
			set;
		}
	}
}
