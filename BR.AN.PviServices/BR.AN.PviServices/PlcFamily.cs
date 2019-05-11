using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	[ComVisible(true)]
	public enum PlcFamily
	{
		None = -1,
		System2010 = 0,
		System2005 = 1,
		System2003 = 2,
		LogicScanner = 3,
		PCBased = 4,
		PowerPanel = 5,
		Panel = 6,
		MemCard = 7,
		PanelC300 = 8,
		PanelC200 = 9,
		X20 = 10,
		CANX2X = 0xFF
	}
}
