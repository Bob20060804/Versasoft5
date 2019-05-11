using System.ComponentModel;

namespace BR.AN.PviServices
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void CpuPhysicalMemReadEventHandler(object sender, CpuPhysicalMemReadEventArgs e);
}
