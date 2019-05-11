using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_DienstHelfer
	{
		public static bool FUN_blnDienstStarten(string i_strDienstName, int i_i32TimeoutMillisekunden, out string i_strMeldung)
		{
			i_strMeldung = string.Empty;
			ServiceController serviceController = new ServiceController(i_strDienstName);
			try
			{
				TimeSpan timeout = TimeSpan.FromMilliseconds(i_i32TimeoutMillisekunden);
				serviceController.Start();
				serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
			}
			catch (Exception ex)
			{
				i_strMeldung = ex.Message;
				if (ex.InnerException != null)
				{
					i_strMeldung = $"{ex.InnerException.Message} - {ex.Message}";
				}
				return false;
			}
			return true;
		}

		public static bool FUN_blnDienstBeenden(string i_strDienstName, int i_i32TimeoutMillisekunden, out string i_strMeldung)
		{
			i_strMeldung = string.Empty;
			ServiceController serviceController = new ServiceController(i_strDienstName);
			try
			{
				TimeSpan timeout = TimeSpan.FromMilliseconds(i_i32TimeoutMillisekunden);
				serviceController.Stop();
				serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
			}
			catch (Exception ex)
			{
				i_strMeldung = ex.Message;
				if (ex.InnerException != null)
				{
					i_strMeldung = $"{ex.InnerException.Message} - {ex.Message}";
				}
				return false;
			}
			return true;
		}

		public static bool FUN_blnDienstNeustarten(string i_strDienstName, int i_i32TimeoutMillisekunden, out string i_strMeldung)
		{
			i_strMeldung = string.Empty;
			ServiceController serviceController = new ServiceController(i_strDienstName);
			try
			{
				TimeSpan timeout = TimeSpan.FromMilliseconds(i_i32TimeoutMillisekunden);
				serviceController.Stop();
				serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
				serviceController.Start();
				serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
			}
			catch (Exception ex)
			{
				i_strMeldung = ex.Message;
				if (ex.InnerException != null)
				{
					i_strMeldung = $"{ex.InnerException.Message} - {ex.Message}";
				}
				return false;
			}
			return true;
		}

		public static IEnumerable<string> FUN_enuErstellerInstallierteDiensteListe()
		{
			return (from i_fdcDienst in ServiceController.GetServices().ToList()
			select i_fdcDienst.ServiceName).ToList();
		}

		public static bool FUN_blnIstDienstGestartet(string i_strDienstName)
		{
			return ServiceControllerStatus.Running.Equals(new ServiceController(i_strDienstName).Status);
		}

		public static bool FUN_blnIstDienstInstalliert(string i_strDienstName)
		{
			foreach (ServiceController item in ServiceController.GetServices().ToList())
			{
				if (item.ServiceName.Equals(i_strDienstName))
				{
					return true;
				}
			}
			return false;
		}
	}
}
