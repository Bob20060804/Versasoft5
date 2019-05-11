using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	public class ErrorLogBook : Logger
	{
		internal const int PT_ERROR_MASK = 127;

		internal const int PT_ERROR_INFO = 3;

		internal const string AR_V_ERROR_LOGBOOK = "A2850";

		public const string KW_LOGBOOK_NAME = "$LOG285$";

		private string errFileName;

		public int CpuLinkId
		{
			get
			{
				if (propParent is Cpu)
				{
					return (int)((Cpu)propParent).LinkId;
				}
				return 0;
			}
		}

		public ErrorLogBook(Cpu cpu)
			: base(cpu, "$LOG285$")
		{
			errFileName = null;
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			int num = 0;
			propErrorCode = errorCode;
			if (accessType == PVIReadAccessTypes.ReadErrorLogBook)
			{
				if (errFileName == null)
				{
					propReadRequestActive = false;
					if (base.ErrorCode == 0)
					{
						LoggerEntryCollection logBookEntries = GetLogBookEntries(pData, dataLen);
						LoggerEventArgs e = new LoggerEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ReadError, logBookEntries);
						OnEntriesRead(e);
					}
					else
					{
						LoggerEventArgs e2 = new LoggerEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ReadError, new LoggerEntryCollection("EventEntries"));
						OnEntriesRead(e2);
						OnError(new PviEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ReadError, Service));
					}
					return;
				}
				FileStream fileStream = new FileStream(errFileName, FileMode.Create, FileAccess.Write);
				byte[] array = new byte[dataLen + 11];
				array[0] = 1;
				array[1] = 50;
				array[2] = 102;
				array[3] = 94;
				for (num = 0; num < base.Cpu.RuntimeVersion.Length && num <= 6; num++)
				{
					array[4 + num] = Convert.ToByte(base.Cpu.RuntimeVersion[num]);
				}
				if (dataLen < int.MaxValue)
				{
					Marshal.Copy(pData, array, 11, (int)dataLen);
					fileStream.Write(array, 0, (int)(dataLen + 11));
					fileStream.Close();
				}
				LoggerEventArgs e3 = new LoggerEventArgs(base.Name, base.Address, errorCode, Service.Language, Action.ReadErrorToFile, new LoggerEntryCollection("EventEntries"));
				OnEntriesRead(e3);
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		private void InsertSysLogBookEntries(APIFC_RLogbookRes_entry[] lbEntries, int itemCnt, LoggerEntryCollection eventEntries)
		{
			int num = 0;
			LoggerEntry loggerEntry = null;
			base.LoggerEntries.propContentVersion = 0u;
			for (num = itemCnt - 1; num > -1; num--)
			{
				LogBookEntry logBookEntry = new LogBookEntry(lbEntries[num]);
				if (logBookEntry.propErrorNumber != 0 || logBookEntry.propTask.Length != 0 || logBookEntry.propErrorInfo != 0 || logBookEntry.propASCIIData.Length != 0)
				{
					if (loggerEntry != null && LevelType.Info == logBookEntry.propLevelType && loggerEntry.propErrorNumber == logBookEntry.propErrorNumber)
					{
						loggerEntry.AppendSGxErrorInfo(logBookEntry, propCpu.IsSG4Target);
					}
					else if (LevelType.Info != logBookEntry.propLevelType)
					{
						loggerEntry = new LoggerEntry(this, base.Cpu.RuntimeVersion, logBookEntry, itemCnt - num, addKeyOnly: true, reverseOrder: false);
						loggerEntry.UpdateForSGx(logBookEntry, propCpu.IsSG4Target);
						base.LoggerEntries.Add(loggerEntry);
						eventEntries.Add(loggerEntry, addKeyOnly: true);
					}
					else
					{
						loggerEntry = null;
					}
				}
			}
		}

		private bool IsNewEntry(LogBookEntry lbEntry, int eventEntryCnt)
		{
			if (eventEntryCnt == base.LoggerEntries.Count)
			{
				for (int i = 0; i < base.LoggerEntries.Count; i++)
				{
					if (lbEntry.EqualsTo(base.LoggerEntries[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		private LoggerEntryCollection GetLogBookEntries(IntPtr pData, uint dataLen)
		{
			int num = Marshal.SizeOf(typeof(APIFC_RLogbookRes_entry));
			int num2 = (int)((long)dataLen / (long)num);
			APIFC_RLogbookRes_entry[] array = new APIFC_RLogbookRes_entry[num2];
			LoggerEntryCollection loggerEntryCollection = new LoggerEntryCollection("EventEntries");
			base.LoggerEntries.Clear();
			for (int i = 0; i < num2; i++)
			{
				array[i] = (APIFC_RLogbookRes_entry)Marshal.PtrToStructure(PviMarshal.GetIntPtr(pData, (ulong)(num * i)), typeof(APIFC_RLogbookRes_entry));
			}
			InsertSysLogBookEntries(array, num2, loggerEntryCollection);
			return loggerEntryCollection;
		}

		public string Load(string fileName)
		{
			string result = null;
			try
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				int num = (int)fileStream.Length;
				byte[] array = new byte[num];
				fileStream.Read(array, 0, num);
				IntPtr hMemory = PviMarshal.AllocHGlobal(num);
				if (1 == array[0] && 50 == array[1] && 102 == array[2] && 94 == array[3])
				{
					PviMarshal.ToAnsiString(array, 4, 7);
					Marshal.Copy(array, 11, hMemory, num - 11);
				}
				else
				{
					Marshal.Copy(array, 0, hMemory, num);
				}
				fileStream.Close();
				LoggerEntryCollection logBookEntries = GetLogBookEntries(hMemory, (uint)num);
				LoggerEventArgs e = new LoggerEventArgs(base.Name, base.Address, 0, Service.Language, Action.ReadError, logBookEntries);
				OnEntriesRead(e);
				PviMarshal.FreeHGlobal(ref hMemory);
				return result;
			}
			catch (System.Exception ex)
			{
				return ex.Message;
			}
		}

		public void Read(string fileName)
		{
			int dataLen = 4;
			int[] source = new int[1]
			{
				0
			};
			IntPtr hMemory = PviMarshal.AllocCoTaskMem(4);
			Marshal.Copy(source, 0, hMemory, 1);
			errFileName = fileName;
			propReadRequestActive = true;
			int num = ReadArgumentRequest(Service.hPvi, (uint)CpuLinkId, AccessTypes.ReadError, hMemory, dataLen, 270u);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.ReadErrorToFile, Service));
			}
			PviMarshal.FreeHGlobal(ref hMemory);
		}

		public override void Read()
		{
			propReturnValue = 0;
			int dataLen = 4;
			int[] source = new int[1]
			{
				0
			};
			IntPtr hMemory = PviMarshal.AllocCoTaskMem(4);
			Marshal.Copy(source, 0, hMemory, 1);
			errFileName = null;
			propReadRequestActive = true;
			propReturnValue = ReadArgumentRequest(Service.hPvi, (uint)CpuLinkId, AccessTypes.ReadError, hMemory, dataLen, 269u);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ReadError, Service));
			}
			PviMarshal.FreeHGlobal(ref hMemory);
		}

		public override void Read(int count)
		{
			propReturnValue = 0;
			int dataLen = 4;
			int[] source = new int[1]
			{
				count
			};
			IntPtr hMemory = PviMarshal.AllocCoTaskMem(4);
			Marshal.Copy(source, 0, hMemory, 1);
			propReadRequestActive = true;
			errFileName = null;
			propReturnValue = ReadArgumentRequest(Service.hPvi, (uint)CpuLinkId, AccessTypes.ReadError, hMemory, dataLen, 269u);
			if (propReturnValue != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, propReturnValue, Service.Language, Action.ReadError, Service));
			}
			PviMarshal.FreeHGlobal(ref hMemory);
		}

		internal override int Read(int count, long id, Action action)
		{
			propReturnValue = 0;
			errFileName = null;
			return propReturnValue;
		}

		internal override void ReadEntry(long id)
		{
			propReturnValue = 0;
			errFileName = null;
		}

		internal override void ReadIndex(Action action)
		{
			errFileName = null;
			propReturnValue = 0;
		}

		internal override int ReadModuleInfo()
		{
			return 0;
		}

		public override void Clear()
		{
			propReturnValue = 0;
		}

		public override void Connect()
		{
			PviEventArgs e = new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ConnectedEvent, Service);
			propReturnValue = 0;
			base.Fire_Connected(this, e);
		}

		public override void Connect(ConnectionType connectionType)
		{
			PviEventArgs e = new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.ConnectedEvent, Service);
			propReturnValue = 0;
			base.Fire_Connected(this, e);
		}

		public override void Delete()
		{
			propReturnValue = 0;
			OnDeleted(new PviEventArgs(base.Name, base.Address, 0, Service.Language, Action.LoggerDelete, Service));
		}

		public override void Disconnect()
		{
			propConnectionState = ConnectionStates.Disconnecting;
			propReturnValue = 0;
			OnDisconnected(new PviEventArgs(base.Name, base.Address, 4803, Service.Language, Action.LoggerDisconnect, Service));
		}

		[Obsolete("This method is no longer supported by ANSL!(Only valid for INA2000)")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Start()
		{
			propReturnValue = 0;
			OnStarted(new PviEventArgs(base.Name, base.Address, 4803, Service.Language, Action.ModuleStart, Service));
		}

		public override void Download(MemoryType memoryType, InstallMode installMode)
		{
			propReturnValue = 0;
			OnDownloaded(new PviEventArgs(base.Name, base.Address, 4803, Service.Language, Action.ModuleDownload, Service));
		}

		public override void Download(MemoryType memoryType, InstallMode installMode, string fileName)
		{
			propReturnValue = 0;
			OnDownloaded(new PviEventArgs(base.Name, base.Address, 4803, Service.Language, Action.ModuleDownload, Service));
		}

		public override void Remove()
		{
			base.Remove();
		}

		[Browsable(false)]
		[Obsolete("This method is no longer supported by ANSL!(Only valid for INA2000)")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Stop()
		{
			propReturnValue = 0;
			OnStopped(new PviEventArgs(base.Name, base.Address, 4803, Service.Language, Action.ModuleStop, Service));
		}

		public override void Upload(string fileName)
		{
			propReturnValue = 0;
			OnUploaded(new PviEventArgs(base.Name, base.Address, 4803, Service.Language, Action.ModuleUpload, Service));
		}
	}
}
