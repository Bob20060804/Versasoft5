using System.Collections.Generic;

namespace BR.AN.PviServices
{
	public class RedundantCpuIpConfigurations
	{
		public List<IpAdressConfiguration> Primary
		{
			get;
			set;
		}

		public List<IpAdressConfiguration> Secundary
		{
			get;
			set;
		}

		public IpAdressConfiguration Cluster
		{
			get;
			set;
		}

		public IpAdressConfiguration Active
		{
			get;
			set;
		}

		public IpAdressConfiguration Inactive
		{
			get;
			set;
		}

		public IpAdressConfiguration Local
		{
			get;
			set;
		}

		public IpAdressConfiguration Partner
		{
			get;
			set;
		}
	}
}
