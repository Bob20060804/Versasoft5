namespace BR.AN.PviServices
{
	public class HardwareItem
	{
		private PlcFamily _PlcFamily;

		private int _ModuleNumber;

		private string _Address;

		public PlcFamily PlcFamily => _PlcFamily;

		public int ModuleNumber => _ModuleNumber;

		public string Address => _Address;

		public HardwareItem(PlcFamily plcFamily, int moduleNumber, string plcAddress)
		{
			_PlcFamily = plcFamily;
			_ModuleNumber = moduleNumber;
			_Address = plcAddress;
		}

		public override string ToString()
		{
			return "PlcFamily=\"" + PlcFamily + "\" ModuleNumber=\"" + ModuleNumber + "\" Address=\"" + Address + "\"";
		}
	}
}
