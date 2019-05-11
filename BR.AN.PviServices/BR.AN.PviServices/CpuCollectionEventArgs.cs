namespace BR.AN.PviServices
{
	public class CpuCollectionEventArgs : CollectionEventArgs
	{
		private CpuCollection propCpus;

		public CpuCollection Cpus => propCpus;

		public CpuCollectionEventArgs(string name, string address, int error, string language, Action action, CpuCollection cpuObjs)
			: base(name, address, error, language, action, cpuObjs)
		{
			propCpus = cpuObjs;
		}
	}
}
