using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	public class HardwareInfo : PviCBEvents, IDisposable
	{
		private ArrayList propItems;

		private Cpu propCpu;

		private uint propEntriesCount;

		private uint propDataLength;

		private PlcFamily propHWPLCFamily;

		private byte[] propHWData;

		private bool propDisposed;

		private List<HardwareItem> _HardwareItems;

		internal Service Service
		{
			get
			{
				if (propCpu != null)
				{
					return propCpu.Service;
				}
				return null;
			}
		}

		public Base Parent => propCpu;

		[CLSCompliant(false)]
		public uint EntriesCount
		{
			get
			{
				return propEntriesCount;
			}
		}

		[CLSCompliant(false)]
		public uint DataLength
		{
			get
			{
				return propDataLength;
			}
		}

		public PlcFamily PlcFamily => propHWPLCFamily;

		public byte[] HardwareInfoData => propHWData;

		internal List<HardwareItem> HardwareItems => _HardwareItems;

		public event DisposeEventHandler Disposing;

		public event PviEventHandler Error;

		public event PviEventHandler Uploaded;

		private void PtrToHWInfoStruct(IntPtr pData, int dataLen)
		{
			int num = 0;
			propEntriesCount = (uint)Marshal.ReadInt32(pData, num);
			num += 4;
			propDataLength = (uint)Marshal.ReadInt32(pData, num);
			num += 4;
			propHWPLCFamily = (PlcFamily)Marshal.ReadInt32(pData, num);
			num += 4;
			propHWData = null;
			if (0 < dataLen - num && propDataLength == dataLen - num)
			{
				propHWData = new byte[propDataLength];
				for (int i = 0; i < propDataLength; i++)
				{
					propHWData[i] = Marshal.ReadByte(pData, num + i);
				}
			}
			else
			{
				propHWData = new byte[1];
			}
			BytesToHardwareInfo(propHWPLCFamily, propHWData);
		}

		internal HardwareInfo(Cpu cpuObj)
		{
			propItems = new ArrayList();
			propCpu = cpuObj;
			propDisposed = false;
			_internId = 0u;
			propEntriesCount = 0u;
			propDataLength = 0u;
			propHWPLCFamily = PlcFamily.None;
			propHWData = null;
			AddToCBReceivers();
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public void Dispose()
		{
			RemoveFromCBReceivers();
			if (!propDisposed)
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing)
		{
			RemoveFromCBReceivers();
			if (!propDisposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					propDisposed = true;
					propHWData = null;
					propCpu = null;
					propItems.Clear();
					propItems = null;
				}
			}
		}

		private void RemoveFromCBReceivers()
		{
			if (Service != null)
			{
				Service.RemoveID(_internId);
			}
		}

		private bool AddToCBReceivers()
		{
			if (Service != null)
			{
				return Service.AddID(this, ref _internId);
			}
			return false;
		}

		public virtual void CopyTo(Array array, int count)
		{
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (PVIReadAccessTypes.UploadHardWareInfo == accessType)
			{
				if (errorCode == 0)
				{
					PtrToHWInfoStruct(pData, (int)dataLen);
				}
				OnUploaded(new PviEventArgs("HardwareUpload", propCpu.Address, errorCode, Service.Language, (Action)accessType, Service));
				if (errorCode != 0)
				{
					OnError(new PviEventArgs("HardwareUpload", propCpu.Address, errorCode, Service.Language, (Action)accessType, Service));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		protected internal virtual void OnError(PviEventArgs e)
		{
			if (Service.ErrorException)
			{
				PviException ex = new PviException(e.ErrorText, e.ErrorCode, this, e);
				throw ex;
			}
			if (Service.ErrorEvent && this.Error != null)
			{
				this.Error(this, e);
			}
			Service.OnError(this, e);
		}

		protected internal virtual void OnUploaded(PviEventArgs e)
		{
			if (this.Uploaded != null)
			{
				this.Uploaded(this, e);
			}
		}

		public int Upload()
		{
			int num = 0;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComReadArgumentRequest(Service.hPvi, propCpu.LinkId, AccessTypes.HardwareUpload, IntPtr.Zero, 0, Service.cbRead, 4294967294u, _internId);
			}
			return PInvokePvicom.PviComMsgReadArgumentRequest(Service.hPvi, propCpu.LinkId, AccessTypes.HardwareUpload, IntPtr.Zero, 0, Service.WindowHandle, 721u, _internId);
		}

		private int ReadIO2005(PlcFamily plcFamily, byte[] data, int offset, ref HardwareItem uploadedHardwareModuleInfo, ref string uploadedAddress)
		{
			int num = (int)data[offset] % 16;
			if (offset + num > data.Length)
			{
				return -1;
			}
			int moduleNumber = data[offset + 2];
			byte b = data[offset + 5];
			if (data[offset + 4] == 0)
			{
				uploadedAddress = "SL" + b;
			}
			else if (data[offset + 4] == 1)
			{
				uploadedAddress = "SL16.IF1.ST1.SL" + b;
			}
			uploadedHardwareModuleInfo = new HardwareItem(plcFamily, moduleNumber, uploadedAddress);
			return offset + num;
		}

		private int ReadCpuSystem(PlcFamily plcFamily, byte[] data, int offset, ref HardwareItem uploadedHardwareModuleInfo, ref string uploadedAddress)
		{
			int num = (int)data[offset] % 16;
			if (offset + num > data.Length)
			{
				return -1;
			}
			int moduleNumber = 256 * data[offset + 2] + data[offset + 3];
			UploadHardwareModuleType uploadHardwareModuleType = ((int)data[offset + 4] % 2 != 0) ? UploadHardwareModuleType.CPU : UploadHardwareModuleType.SYSTEM;
			if (uploadHardwareModuleType == UploadHardwareModuleType.SYSTEM)
			{
				uploadedAddress = "SY" + data[offset + 5];
			}
			else
			{
				uploadedAddress = "SL" + data[offset + 5];
			}
			uploadedHardwareModuleInfo = new HardwareItem(plcFamily, moduleNumber, uploadedAddress);
			return offset + num;
		}

		private int ReadIO2003(PlcFamily plcFamily, byte[] data, int offset, ref HardwareItem uploadedHardwareModuleInfo, ref string uploadedAddress)
		{
			int num = (int)data[offset] % 16;
			if (offset + num > data.Length)
			{
				return -1;
			}
			int moduleNumber = data[offset + 2];
			uploadedAddress = "SL" + data[offset + 3];
			uploadedHardwareModuleInfo = new HardwareItem(plcFamily, moduleNumber, uploadedAddress);
			return offset + num;
		}

		private int ReadSlotModule(PlcFamily plcFamily, byte[] data, int offset, ref HardwareItem uploadedHardwareModuleInfo, ref string uploadedAddress)
		{
			int num = (int)data[offset] % 16;
			if (offset + num > data.Length)
			{
				return -1;
			}
			int moduleNumber = data[offset + 1];
			uploadedAddress = "SS" + data[offset + 3];
			if (plcFamily == PlcFamily.System2003)
			{
				uploadedAddress = "SY1.SS" + data[offset + 3];
			}
			uploadedHardwareModuleInfo = new HardwareItem(plcFamily, moduleNumber, uploadedAddress);
			return offset + num;
		}

		private int ReadUploadInfo(byte[] data, UploadTag tag, int startOffset, int endOffset, out uint value)
		{
			int num = startOffset;
			do
			{
				UploadTag uploadTag = (UploadTag)((int)data[num] / 16);
				int num2 = (int)data[num] % 16;
				if (uploadTag == tag)
				{
					value = 0u;
					switch (num2)
					{
					case 1:
						value = data[num + 1];
						break;
					case 2:
						value = (uint)(256 * data[num + 1] + data[num + 2]);
						break;
					case 4:
						value = (uint)(16777216 * data[num + 1] + 65536 * data[num + 2] + 256 * data[num + 3] + data[num + 4]);
						break;
					}
					return num + num2 + 1;
				}
				num += num2 + 1;
			}
			while (num < endOffset);
			value = uint.MaxValue;
			return -1;
		}

		private int ReadUploadInfo(byte[] data, UploadTag tag, int startOffset, int endOffset, out string value)
		{
			int num = startOffset;
			do
			{
				UploadTag uploadTag = (UploadTag)((int)data[num] / 16);
				int num2 = (int)data[num] % 16;
				if (uploadTag == tag)
				{
					value = "";
					for (int i = 1; i < num2; i++)
					{
						value += (char)data[num + i];
					}
					return num + num2 + 1;
				}
				num += num2 + 1;
			}
			while (num < endOffset);
			value = null;
			return -1;
		}

		private int ReadAdvanced(byte[] data, int offset, ref HardwareItem uploadedHardwareModuleInfo, ref string uploadedAddress, ref string uploadedAutomationRuntime, ref int uploadedModuleNr)
		{
			int num = data[offset + 1];
			if (offset + num > data.Length)
			{
				return -1;
			}
			PlcFamily plcFamily = (PlcFamily)data[offset + 2];
			if (plcFamily == PlcFamily.System2010)
			{
				plcFamily = PlcFamily.None;
			}
			UploadHardwareModuleType uploadHardwareModuleType = UploadHardwareModuleType.MOD;
			switch (data[offset + 3])
			{
			case 1:
				uploadHardwareModuleType = ((ReadUploadInfo(data, UploadTag.SubSlot, offset + 8, offset + num, out uint value2) <= 0 || value2 == 0) ? UploadHardwareModuleType.CPU : UploadHardwareModuleType.SUB);
				break;
			case 2:
				uploadHardwareModuleType = ((ReadUploadInfo(data, UploadTag.SubSlot, offset + 8, offset + num, out uint value) <= 0 || value == 0) ? UploadHardwareModuleType.SYSTEM : UploadHardwareModuleType.SUB);
				break;
			}
			int num2 = 16777216 * data[offset + 4] + 65536 * data[offset + 5] + 256 * data[offset + 6] + data[offset + 7];
			if (num2 > 0)
			{
				uploadedModuleNr = num2;
			}
			if (ReadUploadInfo(data, UploadTag.Address, offset + 8, offset + num, out uploadedAddress) < 0)
			{
				int i = ReadUploadInfo(data, UploadTag.FormatE2, offset + 8, offset + num, out uint value3);
				if (value3 == 1)
				{
					uploadedAddress = "";
					for (; i > 0 && i + 1 < offset + num; i += 2)
					{
						UploadTag uploadTag = (UploadTag)((int)data[i] / 16);
						if (uploadTag != UploadTag.SL && uploadTag != UploadTag.SS && uploadTag != UploadTag.ST && uploadTag != UploadTag.IF)
						{
							break;
						}
						if (uploadedAddress.Length > 0)
						{
							uploadedAddress += ".";
						}
						uploadedAddress = uploadedAddress + uploadTag.ToString() + data[i + 1];
					}
				}
				else if (i < 0)
				{
					if (uploadHardwareModuleType == UploadHardwareModuleType.CPU)
					{
						uploadedAddress = "$root";
					}
					else
					{
						i = offset + 8;
						while (i > 0 && i + 1 < offset + num)
						{
							switch ((int)data[i] / 16)
							{
							case 0:
								if (data[i + 1] > 1)
								{
									uploadedAddress = "SY" + data[i + 1];
								}
								else
								{
									uploadedAddress = "SL" + data[i + 1];
								}
								i += 2;
								continue;
							case 3:
								if (!string.IsNullOrEmpty(uploadedAddress))
								{
									uploadedAddress = uploadedAddress + ".SS" + data[i + 1];
								}
								else
								{
									uploadedAddress = "SS" + data[i + 1];
								}
								i += 2;
								continue;
							}
							break;
						}
					}
				}
			}
			if (uploadedAddress != null && num2 == 4553)
			{
				uploadedAddress = null;
			}
			uploadedHardwareModuleInfo = new HardwareItem(plcFamily, num2, uploadedAddress);
			return offset + num;
		}

		private int ReadItem(PlcFamily plcFamily, byte[] data, int offset, out HardwareItem uploadedHardwareModuleInfo, out string uploadedAddress, out string uploadedAutomationRuntime, out int uploadedModuleNr)
		{
			uploadedHardwareModuleInfo = null;
			uploadedAddress = null;
			uploadedAutomationRuntime = null;
			uploadedModuleNr = 0;
			if (offset < 0)
			{
				return -1;
			}
			try
			{
				switch ((int)data[offset] / 16)
				{
				case 1:
					return ReadIO2005(plcFamily, data, offset, ref uploadedHardwareModuleInfo, ref uploadedAddress);
				case 3:
					return ReadCpuSystem(plcFamily, data, offset, ref uploadedHardwareModuleInfo, ref uploadedAddress);
				case 6:
					return ReadIO2003(plcFamily, data, offset, ref uploadedHardwareModuleInfo, ref uploadedAddress);
				case 8:
					return ReadSlotModule(plcFamily, data, offset, ref uploadedHardwareModuleInfo, ref uploadedAddress);
				case 15:
					return ReadAdvanced(data, offset, ref uploadedHardwareModuleInfo, ref uploadedAddress, ref uploadedAutomationRuntime, ref uploadedModuleNr);
				}
			}
			catch
			{
			}
			return -1;
		}

		private void BytesToHardwareInfo(PlcFamily plcFamily, byte[] hwData)
		{
			int num = 0;
			_HardwareItems = new List<HardwareItem>();
			if (hwData == null || hwData.GetLength(0) == 0)
			{
				return;
			}
			for (int i = 0; i < hwData.GetLength(0); i++)
			{
				if (num < 0)
				{
					break;
				}
				if (num >= hwData.GetLength(0))
				{
					break;
				}
				HardwareItem uploadedHardwareModuleInfo = null;
				num = ReadItem(plcFamily, hwData, num, out uploadedHardwareModuleInfo, out string _, out string _, out int _);
				if (uploadedHardwareModuleInfo != null)
				{
					_HardwareItems.Add(uploadedHardwareModuleInfo);
				}
			}
		}

		public override string ToString()
		{
			string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<HardwareItems>\r\n";
			if (_HardwareItems != null)
			{
				foreach (HardwareItem hardwareItem in _HardwareItems)
				{
					str = str + "<Item " + hardwareItem.ToString() + "/>\r\n";
				}
			}
			return str + "</HardwareItems>\r\n";
		}
	}
}
