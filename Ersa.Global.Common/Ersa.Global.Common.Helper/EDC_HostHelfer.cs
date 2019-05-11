using System;
using System.Linq;
using System.Net;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_HostHelfer
	{
		public static bool FUN_blnIstHostaAdressLokal(string i_strHostaname)
		{
			if (string.IsNullOrEmpty(i_strHostaname))
			{
				return false;
			}
			if (i_strHostaname.ToLower().Contains("localhost"))
			{
				return true;
			}
			try
			{
				int num = i_strHostaname.LastIndexOf("\\", StringComparison.Ordinal);
				if (num > 0)
				{
					i_strHostaname = i_strHostaname.Substring(0, num);
				}
				IPAddress[] hostAddresses = Dns.GetHostAddresses(i_strHostaname);
				IPAddress[] hostAddresses2 = Dns.GetHostAddresses(Dns.GetHostName());
				IPAddress[] array = hostAddresses;
				foreach (IPAddress iPAddress in array)
				{
					if (IPAddress.IsLoopback(iPAddress))
					{
						return true;
					}
					if (hostAddresses2.Contains(iPAddress))
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}
	}
}
