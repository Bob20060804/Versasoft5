using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace BR.AN
{
	public class BRLicenseInfo : IDisposable
	{
		private ArrayList propLCInfos;

		private int propSearchStep;

		internal CBFindKeys callBackFindKeys;

		internal CBFindModules callBackFindMods;

		internal CBFindBRIpc callBackFindBRIpc;

		internal bool propDisposed;

		private BuRIPCStates propBRIPCState;

		public BuRIPCStates BRIPCState => propBRIPCState;

		public event LCInfosEventHandler Found;

		public event DisposeEventHandler Disposing;

		[DllImport("BrSecDll.dll")]
		internal static extern int Br_ReadDsKey(CBFindKeys pCallback, IntPtr pCBData);

		[DllImport("BrSecDll.dll")]
		internal static extern int Br_ReadLcMod(CBFindModules pCallback, IntPtr pCBData, string searchPath);

		[DllImport("BrSecDll.dll")]
		internal static extern int Br_FindBrIPC(CBFindBRIpc pCallback, IntPtr pCBData, long appId, long appSubId);

		public BRLicenseInfo()
		{
			propDisposed = false;
			propLCInfos = new ArrayList(1);
			propSearchStep = 0;
			propBRIPCState = BuRIPCStates.INVALID;
			callBackFindKeys = CallBackFindKeys;
			callBackFindMods = CallBackFindLicModules;
			callBackFindBRIpc = CallBackFindBRIpc;
		}

		public void Dispose()
		{
			if (!propDisposed)
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing)
		{
			if (!propDisposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					propDisposed = true;
					callBackFindBRIpc = null;
					callBackFindKeys = null;
					callBackFindMods = null;
					this.Disposing = null;
					this.Found = null;
					propLCInfos.Clear();
					propLCInfos = null;
				}
			}
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		protected internal bool CallBackFindKeys(int pCBData, [MarshalAs(UnmanagedType.LPStr)] string keySerial, [MarshalAs(UnmanagedType.LPStr)] string keyPort, [MarshalAs(UnmanagedType.LPStr)] string dsID, [MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] BRSECComponentEntry[] pSecInfos)
		{
			ArrayList arrayList = new ArrayList(1);
			for (int i = 0; i < pSecInfos.GetLength(0); i++)
			{
				BRSECComponentEntry bRSECComponentEntry = (BRSECComponentEntry)pSecInfos.GetValue(i);
				if (bRSECComponentEntry.OrderId.Length == 0 || bRSECComponentEntry.LicenseText.Length == 0)
				{
					break;
				}
				LicComponets value = new LicComponets(bRSECComponentEntry.OrderId, bRSECComponentEntry.LicenseText, (bRSECComponentEntry.RequiresBRIPC != 0) ? true : false);
				arrayList.Add(value);
			}
			OnCBFindKeys(new LCInfo(dsID, keyPort, "", keySerial, arrayList));
			return true;
		}

		protected internal bool CallBackFindLicModules(int pCBData, [MarshalAs(UnmanagedType.LPStr)] string licName, [MarshalAs(UnmanagedType.LPStr)] string licInfo, [MarshalAs(UnmanagedType.LPStr)] string licID, [MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] BRSECComponentEntry[] pSecInfos)
		{
			ArrayList arrayList = new ArrayList(1);
			for (int i = 0; i < pSecInfos.GetLength(0); i++)
			{
				BRSECComponentEntry bRSECComponentEntry = (BRSECComponentEntry)pSecInfos.GetValue(i);
				if (bRSECComponentEntry.OrderId.Length == 0 || bRSECComponentEntry.LicenseText.Length == 0)
				{
					break;
				}
				LicComponets value = new LicComponets(bRSECComponentEntry.OrderId, bRSECComponentEntry.LicenseText, (bRSECComponentEntry.RequiresBRIPC != 0) ? true : false);
				arrayList.Add(value);
			}
			OnCBFindKeys(new LCInfo(licID, "DLL", licInfo, licName, arrayList));
			return true;
		}

		protected internal bool CallBackFindBRIpc(int pCBData, int appID, int appSubId)
		{
			if (pCBData == 0)
			{
				OnCBFindBRIPc(appID, appSubId);
			}
			else
			{
				propBRIPCState = BuRIPCStates.RunningOnABuRIPC;
			}
			return true;
		}

		protected void OnCBFindKeys(LCInfo lcInfo)
		{
			if (lcInfo != null)
			{
				propLCInfos.Add(lcInfo);
			}
			propSearchStep--;
			if (propSearchStep == 0)
			{
				Fire_LCsFound(propLCInfos);
			}
		}

		protected void OnCBFindBRIPc(int appID, int appSubId)
		{
			propBRIPCState = BuRIPCStates.RunningOnABuRIPC;
			propSearchStep--;
			if (propSearchStep == 0)
			{
				Fire_LCsFound(null);
			}
		}

		private void Fire_LCsFound(ArrayList lcInfos)
		{
			if (this.Found != null)
			{
				this.Found(this, new LCEventArgs(lcInfos));
			}
		}

		public int Search()
		{
			int num = 0;
			int num2 = 0;
			propSearchStep = 3;
			propLCInfos.Clear();
			try
			{
				if (Br_FindBrIPC(callBackFindBRIpc, IntPtr.Zero, 0L, 0L) == 0)
				{
					propBRIPCState = BuRIPCStates.NoBuRIPC;
					OnCBFindKeys(null);
				}
				num2 = Br_ReadDsKey(callBackFindKeys, IntPtr.Zero);
				if (0 < num2)
				{
					num += num2;
				}
				else
				{
					OnCBFindKeys(null);
				}
				num2 = Br_ReadLcMod(callBackFindMods, IntPtr.Zero, null);
				if (0 < num2)
				{
					return num + num2;
				}
				OnCBFindKeys(null);
				return num;
			}
			catch
			{
				return -1;
			}
		}

		public int Search(string moduleFileName)
		{
			int num = 0;
			int num2 = 0;
			propSearchStep = 3;
			propLCInfos.Clear();
			try
			{
				if (Br_FindBrIPC(callBackFindBRIpc, IntPtr.Zero, 0L, 0L) == 0)
				{
					propBRIPCState = BuRIPCStates.NoBuRIPC;
					OnCBFindKeys(null);
				}
				num2 = Br_ReadDsKey(callBackFindKeys, IntPtr.Zero);
				if (0 < num2)
				{
					num += num2;
				}
				else
				{
					OnCBFindKeys(null);
				}
				num2 = Br_ReadLcMod(callBackFindMods, IntPtr.Zero, moduleFileName);
				if (0 < num2)
				{
					return num + num2;
				}
				OnCBFindKeys(null);
				return num;
			}
			catch
			{
				return -1;
			}
		}

		public bool SearchBuRIpc()
		{
			int num = 0;
			try
			{
				if (Br_FindBrIPC(callBackFindBRIpc, IntPtr.Zero, -1L, -1L) == 0)
				{
					propBRIPCState = BuRIPCStates.NoBuRIPC;
					return false;
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
