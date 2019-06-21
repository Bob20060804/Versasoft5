using Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor
{
	public class EDC_AblaufSchrittAenderung
	{
		public ENUM_AenderungsArt PRO_enmAenderungsArt
		{
			get;
		}

		public EDC_VorlageElement PRO_edcElement
		{
			get;
		}

		public EDC_AblaufSchritte PRO_lstQuelle
		{
			get;
		}

		public EDC_AblaufSchritte PRO_lstZiel
		{
			get;
		}

		public int PRO_i32AlterIndex
		{
			get;
		}

		public int PRO_i32NeuerIndex
		{
			get;
		}

		private EDC_AblaufSchrittAenderung(ENUM_AenderungsArt i_enmAenderungsArt, EDC_VorlageElement i_edcElement, EDC_AblaufSchritte i_edcQuelle, EDC_AblaufSchritte i_edcZiel, int i_i32AlterIndex, int i_i32NeuerIndex)
		{
			PRO_enmAenderungsArt = i_enmAenderungsArt;
			PRO_edcElement = i_edcElement;
			PRO_lstQuelle = i_edcQuelle;
			PRO_lstZiel = i_edcZiel;
			PRO_i32AlterIndex = i_i32AlterIndex;
			PRO_i32NeuerIndex = i_i32NeuerIndex;
		}

		public static EDC_AblaufSchrittAenderung FUN_edcHinzugefuegtErzeugen(EDC_VorlageElement i_edcElement, EDC_AblaufSchritte i_lstZiel, int i_i32NeuerIndex)
		{
			return new EDC_AblaufSchrittAenderung(ENUM_AenderungsArt.Hinzugefuegt, i_edcElement, null, i_lstZiel, -1, i_i32NeuerIndex);
		}

		public static EDC_AblaufSchrittAenderung FUN_edcVerschobenErzeugen(EDC_VorlageElement i_edcElement, EDC_AblaufSchritte i_lstQuelle, EDC_AblaufSchritte i_lstZiel, int i_i32AlterIndex, int i_i32NeuerIndex)
		{
			return new EDC_AblaufSchrittAenderung(ENUM_AenderungsArt.Verschoben, i_edcElement, i_lstQuelle, i_lstZiel, i_i32AlterIndex, i_i32NeuerIndex);
		}

		public static EDC_AblaufSchrittAenderung FUN_edcEntferntErzeugen(EDC_VorlageElement i_edcElement, EDC_AblaufSchritte i_lstZiel, int i_i32AlterIndex)
		{
			return new EDC_AblaufSchrittAenderung(ENUM_AenderungsArt.Entfernt, i_edcElement, null, i_lstZiel, i_i32AlterIndex, -1);
		}
	}
}
