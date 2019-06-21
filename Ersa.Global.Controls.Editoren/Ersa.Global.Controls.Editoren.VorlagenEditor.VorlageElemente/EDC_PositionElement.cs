namespace Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente
{
	public abstract class EDC_PositionElement : EDC_VorlageElement
	{
		public override string PRO_strIconUri => "pack://application:,,,/Ersa.Global.Controls;component/Bilder/Icons/Icon_Info_24x24.png";

		public override bool PRO_blnKannDavorEingefuegtWerden => PRO_enmPositionstyp != ENUM_Positionstyp.Start;

		public ENUM_Positionstyp PRO_enmPositionstyp
		{
			get;
		}

		protected EDC_PositionElement(ENUM_Positionstyp i_enmPositionstyp)
		{
			PRO_enmPositionstyp = i_enmPositionstyp;
		}
	}
}
