namespace Ersa.Platform.Common.Model
{
	/// <summary>
	/// PCB State
	/// </summary>
	public enum ENUM_PcbZustand
	{
		enmAusgelaufenPcbOkFertigGeloetet = 0,
		enmEntnommenPcbOkFertigGeloetet = 1,
		enmAusgelaufenPcbFehlerhaftFertigGeloetet = 2,
		enmEntnommenPcbFehlerhaftFertigGeloetet = 3,
		enmAusgelaufenPcbOkNichtFertigGeloetet = 4,
		enmEntnommenPcbOkNichtFertigGeloetet = 5,
		enmAusgelaufenPcbFehlerhaftNichtFertigGeloetet = 6,
		enmEntnommenPcbFehlerhaftNichtFertigGeloetet = 7,
		enmKeinPcbZustand = 0xFF
	}
}
