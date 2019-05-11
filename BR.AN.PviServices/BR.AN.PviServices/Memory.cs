using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Memory : PviCBEvents, IDisposable
	{
		internal const int SET_PVICALLBACK_DATA = -2;

		private Service propService;

		internal string propErrorText = string.Empty;

		internal string propCurLanguage = string.Empty;

		private Cpu propCpu;

		private uint propFlags;

		private MemoryType propType;

		private uint propStartAddress;

		private uint propTotalLen;

		private uint propFreeLen;

		private uint propFreeBlockLen;

		private string propName;

		private string propAddress;

		internal Base propParent;

		internal bool propDisposed;

		internal object propUserData;

		internal int propErrorCode;

		public string Address
		{
			get
			{
				return propAddress;
			}
			set
			{
				propAddress = value;
			}
		}

		public string Name => propName;

		public MemoryType Type => propType;

		[Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
		[CLSCompliant(false)]
		public uint StartAddress
		{
			get
			{
				if (propStartAddress < 0)
				{
					return 0u;
				}
				return propStartAddress;
			}
		}

		[CLSCompliant(false)]
		public uint TotalLength
		{
			get
			{
				if (propTotalLen < 0)
				{
					return 0u;
				}
				return propTotalLen;
			}
		}

		[CLSCompliant(false)]
		public uint FreeLength
		{
			get
			{
				if (propFreeLen < 0)
				{
					return 0u;
				}
				return propFreeLen;
			}
		}

		[CLSCompliant(false)]
		public uint FreeBlockLength
		{
			get
			{
				if (propFreeBlockLen < 0)
				{
					return 0u;
				}
				return propFreeBlockLen;
			}
		}

		public Service Service => propService;

		public string FullName
		{
			get
			{
				if (Name.Length > 0)
				{
					return Parent.FullName + "." + Name;
				}
				return Parent.FullName;
			}
		}

		internal uint LinkId
		{
			get
			{
				if (propCpu != null)
				{
					return propCpu.LinkId;
				}
				return 0u;
			}
		}

		internal uint InternId => _internId;

		public object UserData
		{
			get
			{
				return propUserData;
			}
			set
			{
				propUserData = value;
			}
		}

		public int ErrorCode => propErrorCode;

		public string ErrorText
		{
			get
			{
				string text = "en";
				if (Service != null)
				{
					text = Service.Language;
				}
				if (propErrorText == string.Empty)
				{
					propCurLanguage = text;
					if (Service == null)
					{
						propErrorText = Service.GetErrorText(propErrorCode, text);
					}
					else
					{
						propErrorText = Service.Utilities.GetErrorText(propErrorCode);
					}
					return propErrorText;
				}
				if (propCurLanguage.CompareTo(text) == 0)
				{
					return propErrorText;
				}
				propCurLanguage = text;
				if (Service == null)
				{
					propErrorText = Service.GetErrorText(propErrorCode, text);
				}
				else
				{
					propErrorText = Service.Utilities.GetErrorText(propErrorCode);
				}
				return propErrorText;
			}
		}

		public virtual Base Parent => propParent;

		public event DisposeEventHandler Disposing;

		public event PviEventHandler Cleaned;

		public event PviEventHandler Error;

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
					propAddress = null;
					propCpu = null;
					propName = null;
					propParent = null;
					propUserData = null;
				}
			}
		}

		internal Memory(Cpu cpu, APIFC_CPmemInfoRes memory)
		{
			propService = cpu.Service;
			propDisposed = false;
			propCpu = cpu;
			propParent = cpu;
			propFlags = memory.flags;
			propType = memory.type;
			propStartAddress = memory.start_adr;
			propTotalLen = memory.total_len;
			propFreeLen = memory.free_len;
			propFreeBlockLen = memory.freeblk_len;
			propName = propType.ToString();
			propAddress = propName;
			AddToCBReceivers();
		}

		internal Memory(Cpu cpu, string name)
		{
			propService = cpu.Service;
			propDisposed = false;
			propCpu = cpu;
			propParent = cpu;
			propName = propType.ToString();
			propAddress = name;
			AddToCBReceivers();
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

		public bool IsStartAddressValid()
		{
			return (propFlags & 1) != 0;
		}

		public bool IsTotalLengthValid()
		{
			return (propFlags & 2) != 0;
		}

		public bool IsFreeLengthValid()
		{
			return (propFlags & 4) != 0;
		}

		public bool IsFreeBlockLengthValid()
		{
			return (propFlags & 8) != 0;
		}

		public void Clean()
		{
			int num = 0;
			int num2 = 0;
			if (propCpu.BootMode != BootMode.Diagnostics)
			{
				OnError(new PviEventArgs(propName, propAddress, 4025, propCpu.Service.Language, Action.ClearMemory, Service));
				return;
			}
			num = Marshal.SizeOf(typeof(int));
			Marshal.WriteInt32(Service.RequestBuffer, (int)propType);
			num2 = WriteRequest(propCpu.Service.hPvi, propCpu.LinkId, AccessTypes.ClearMemory, Service.RequestBuffer, num, 618u);
			if (num2 != 0)
			{
				OnError(new PviEventArgs(propName, propAddress, num2, propCpu.Service.Language, Action.ClearMemory, Service));
			}
		}

		protected virtual void OnCleaned(PviEventArgs e)
		{
			if (this.Cleaned != null)
			{
				this.Cleaned(this, e);
			}
		}

		protected internal virtual void OnError(PviEventArgs e)
		{
			propErrorCode = e.ErrorCode;
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

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			propErrorCode = errorCode;
			if (PVIWriteAccessTypes.ClearMemory == accessType)
			{
				if (propErrorCode == 0)
				{
					OnCleaned(new PviEventArgs(Name, Address, propErrorCode, Service.Language, Action.ClearMemory, Service));
					return;
				}
				OnCleaned(new PviEventArgs(Name, Address, propErrorCode, Service.Language, Action.ClearMemory, Service));
				OnError(new PviEventArgs(Name, Address, propErrorCode, Service.Language, Action.ClearMemory, Service));
			}
			else
			{
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
			}
		}

		internal int WriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, uint respParam)
		{
			int num = 0;
			if (Service.EventMessageType == EventMessageType.CallBack)
			{
				return PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.cbWrite, 4294967294u, InternId);
			}
			return PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, pData, dataLen, Service.WindowHandle, respParam, _internId);
		}

		public int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Memory memory)
		{
			string text = "";
			text = reader.GetAttribute("Name");
			if (text != null && text.Length > 0)
			{
				memory.propName = text;
			}
			text = reader.GetAttribute("Address");
			if (text != null && text.Length > 0)
			{
				memory.propAddress = text;
			}
			text = reader.GetAttribute("UserData");
			if (text != null && text.Length > 0)
			{
				memory.propUserData = text;
			}
			text = reader.GetAttribute("ErrorCode");
			if (text != null && text.Length > 0)
			{
				int result = 0;
				if (PviParse.TryParseInt32(text, out result))
				{
					memory.propErrorCode = result;
				}
			}
			text = reader.GetAttribute("FreeBlockLength");
			if (text != null && text.Length > 0)
			{
				uint result2 = 0u;
				if (PviParse.TryParseUInt32(text, out result2))
				{
					memory.propFreeBlockLen = result2;
				}
			}
			text = reader.GetAttribute("FreeLength");
			if (text != null && text.Length > 0)
			{
				uint result3 = 0u;
				if (PviParse.TryParseUInt32(text, out result3))
				{
					memory.propFreeLen = result3;
				}
			}
			text = reader.GetAttribute("StartAddress");
			if (text != null && text.Length > 0)
			{
				uint result4 = 0u;
				if (PviParse.TryParseUInt32(text, out result4))
				{
					memory.propStartAddress = result4;
				}
			}
			text = reader.GetAttribute("TotalLength");
			if (text != null && text.Length > 0)
			{
				uint result5 = 0u;
				if (PviParse.TryParseUInt32(text, out result5))
				{
					memory.propTotalLen = result5;
				}
			}
			text = reader.GetAttribute("Type");
			if (text != null && text.Length > 0 && text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "dram":
					memory.propType = MemoryType.Dram;
					break;
				case "fixram":
					memory.propType = MemoryType.FixRam;
					break;
				case "globalanalog":
					memory.propType = MemoryType.GlobalAnalog;
					break;
				case "globaldigital":
					memory.propType = MemoryType.GlobalDigital;
					break;
				case "oo":
					memory.propType = MemoryType.Io;
					break;
				case "memcard":
					memory.propType = MemoryType.MemCard;
					break;
				case "os":
					memory.propType = MemoryType.Os;
					break;
				case "permanen":
					memory.propType = MemoryType.Permanent;
					break;
				case "systemram":
					memory.propType = MemoryType.SystemRam;
					break;
				case "systemrom":
					memory.propType = MemoryType.SystemRom;
					break;
				case "tmp":
					memory.propType = MemoryType.Tmp;
					break;
				case "userram":
					memory.propType = MemoryType.UserRam;
					break;
				case "userrom":
					memory.propType = MemoryType.UserRom;
					break;
				}
			}
			reader.Read();
			return 0;
		}

		internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			writer.WriteStartElement("Memory");
			if (propName != null && propName.Length > 0)
			{
				writer.WriteAttributeString("Name", propName);
			}
			if (propAddress != null && propAddress.Length > 0)
			{
				writer.WriteAttributeString("Address", propAddress);
			}
			if (propUserData is string && propUserData != null && ((string)propUserData).Length > 0)
			{
				writer.WriteAttributeString("UserData", propUserData.ToString());
			}
			if (propErrorCode > 0)
			{
				writer.WriteAttributeString("ErrorCode", propErrorCode.ToString());
			}
			writer.WriteAttributeString("FreeBlockLength", FreeBlockLength.ToString());
			writer.WriteAttributeString("FreeLength", FreeLength.ToString());
			writer.WriteAttributeString("StartAddress", StartAddress.ToString());
			writer.WriteAttributeString("TotalLength", TotalLength.ToString());
			writer.WriteAttributeString("Type", Type.ToString());
			writer.WriteEndElement();
			return 0;
		}
	}
}
