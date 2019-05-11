using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
	public class Profiler : PviCBEvents, IDisposable
	{
		private const uint TTServiceProfiler = 65025u;

		private const uint TTServiceDataCode = 3u;

		private const int TTServiceWrite = 1;

		private const int TTServiceRead = 2;

		private const string definitionModule = "prfmod$e";

		private string propName;

		private Cpu propParent;

		private ProfilerState propState;

		private uint propInternId;

		private int propErrorCode;

		private bool propCommandActive;

		internal bool propDisposed;

		public string Name => propName;

		public Cpu Cpu => propParent;

		public int ErrorNumber => propErrorCode;

		internal uint InternId => propInternId;

		public ProfilerState State => propState;

		public bool CommandActive => propCommandActive;

		public event ProfilerEventHandler StateRead;

		public event ProfilerEventHandler InfoRead;

		public event ProfilerEventHandler Installed;

		public event ProfilerEventHandler Deinstalled;

		public event ProfilerEventHandler Started;

		public event ProfilerEventHandler Stopped;

		public event ProfilerEventHandler StackRead;

		public event ProfilerEventHandler Error;

		public event DisposeEventHandler Disposing;

		public Profiler(Cpu cpu, string name)
		{
			propDisposed = false;
			propParent = cpu;
			if (cpu.propProfiler != null)
			{
				throw new InvalidOperationException();
			}
			cpu.propProfiler = this;
			propName = name;
			propCommandActive = false;
			AddToCBReceivers();
		}

		private void RemoveFromCBReceivers()
		{
			if (Cpu != null && Cpu.Service != null)
			{
				Cpu.Service.RemoveID(propInternId);
			}
		}

		private bool AddToCBReceivers()
		{
			if (Cpu != null && Cpu.Service != null)
			{
				return Cpu.Service.AddID(this, ref propInternId);
			}
			return false;
		}

		public void Install()
		{
			byte[] array = new byte[11]
			{
				5,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1003);
			propCommandActive = true;
		}

		public void Deinstall()
		{
			byte[] array = new byte[11]
			{
				2,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1004);
			propCommandActive = true;
		}

		public void Start()
		{
			byte[] array = new byte[11]
			{
				6,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1001);
			propCommandActive = true;
		}

		public void ExtendedStart()
		{
			byte[] array = new byte[11]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1000);
			propCommandActive = true;
		}

		public void Stop()
		{
			byte[] array = new byte[11]
			{
				1,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1002);
			propCommandActive = true;
		}

		public void InstallDefault()
		{
			byte[] array = new byte[11]
			{
				7,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1003);
			propCommandActive = true;
		}

		public void ReadInfo()
		{
			byte[] array = new byte[11]
			{
				4,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1006);
			propCommandActive = true;
		}

		public void ReadStack()
		{
			byte[] array = new byte[11]
			{
				4,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			char[] array2 = "prfmod$e".ToCharArray();
			for (int i = 0; i < array2.Length; i++)
			{
				array[1 + i] = (byte)array2[i];
			}
			TTS_Write(array, (byte)array.Length, 1005);
			propCommandActive = true;
		}

		public void ReadState()
		{
			byte[] array = new byte[11];
			TTS_Read(array, (byte)array.Length, 1008);
			propCommandActive = true;
		}

		protected internal virtual int TTS_Read(byte[] data, byte dataLength, int respParam)
		{
			int num = 65025;
			byte[] array = new byte[dataLength + 5];
			array[0] = (byte)(num & 0xFF);
			array[1] = (byte)((num & 0xFF00) >> 8);
			array[2] = 2;
			array[3] = 3;
			array[4] = dataLength;
			for (int i = 0; i < dataLength; i++)
			{
				array[5 + i] = data[i];
			}
			IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)array.Length);
			Marshal.Copy(array, 0, hMemory, array.Length);
			int num2 = 0;
			num2 = ((Cpu.Service.EventMessageType != 0) ? PInvokePvicom.PviComMsgReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.WindowHandle, (uint)respParam, InternId) : PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbRead, 4294967294u, InternId));
			PviMarshal.FreeHGlobal(ref hMemory);
			if (num2 != 0)
			{
				OnError(new ProfilerEventArgs(Name, (Action)respParam, num2));
			}
			return num2;
		}

		protected internal virtual int TTS_Write(byte[] data, byte dataLength, int respParam)
		{
			int num = 65025;
			byte[] array = new byte[dataLength + 5];
			array[0] = (byte)(num & 0xFF);
			array[1] = (byte)((num & 0xFF00) >> 8);
			array[2] = 1;
			array[3] = 3;
			array[4] = dataLength;
			for (int i = 0; i < dataLength; i++)
			{
				array[5 + i] = data[i];
			}
			IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr)array.Length);
			Marshal.Copy(array, 0, hMemory, array.Length);
			int num2 = 0;
			if (Cpu.Service.EventMessageType == EventMessageType.CallBack)
			{
				switch (respParam)
				{
				case 1003:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbReadA, 4294967294u, InternId);
					break;
				case 1004:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbReadB, 4294967294u, InternId);
					break;
				case 1001:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbReadC, 4294967294u, InternId);
					break;
				case 1000:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbReadD, 4294967294u, InternId);
					break;
				case 1002:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbReadE, 4294967294u, InternId);
					break;
				case 1006:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbReadF, 4294967294u, InternId);
					break;
				case 1005:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbReadG, 4294967294u, InternId);
					break;
				default:
					num2 = PInvokePvicom.PviComReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.cbRead, 4294967294u, InternId);
					break;
				}
			}
			else
			{
				num2 = PInvokePvicom.PviComMsgReadArgumentRequest(Cpu.Service.hPvi, Cpu.LinkId, AccessTypes.TTService, hMemory, array.Length, Cpu.Service.WindowHandle, (uint)respParam, InternId);
			}
			PviMarshal.FreeHGlobal(ref hMemory);
			if (num2 != 0)
			{
				OnError(new ProfilerEventArgs(Name, (Action)respParam, num2));
			}
			return num2;
		}

		internal override void OnPviCreated(int errorCode, uint linkID)
		{
			base.OnPviCreated(errorCode, linkID);
		}

		internal override void OnPviLinked(int errorCode, uint linkID, int option)
		{
			base.OnPviLinked(errorCode, linkID, option);
		}

		internal override void OnPviEvent(int errorCode, EventTypes eventType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (eventType != EventTypes.Error && eventType != EventTypes.Data)
			{
				base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
			}
		}

		internal override void OnPviWritten(int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, int option, IntPtr pData, uint dataLen)
		{
			propErrorCode = errorCode;
			if (accessType != PVIWriteAccessTypes.TTService)
			{
				base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			propErrorCode = errorCode;
			TTResponse tTResponse = default(TTResponse);
			tTResponse.dataLen = 0;
			tTResponse.profilerError = 0;
			tTResponse.profilerState = 0;
			tTResponse.serviceGroup = 0;
			tTResponse.serviceID = 0;
			if (accessType == PVIReadAccessTypes.TTService)
			{
				propCommandActive = false;
				if (pData == IntPtr.Zero)
				{
					return;
				}
				if (errorCode == 0)
				{
					tTResponse = (TTResponse)Marshal.PtrToStructure(pData, typeof(TTResponse));
					switch (option)
					{
					case 1:
					{
						ushort num6 = (ushort)(tTResponse.profilerState + (ushort)(tTResponse.profilerError << 8));
						OnInstalled(new ProfilerEventArgs(propName, Action.ProfilerInstall, num6));
						break;
					}
					case 2:
					{
						ushort num5 = (ushort)(tTResponse.profilerState + (ushort)(tTResponse.profilerError << 8));
						OnDeinstalled(new ProfilerEventArgs(propName, Action.ProfilerDeinstall, num5));
						break;
					}
					case 3:
					{
						ushort num4 = (ushort)(tTResponse.profilerState + (ushort)(tTResponse.profilerError << 8));
						OnStarted(new ProfilerEventArgs(propName, Action.ProfilerStart, num4));
						break;
					}
					case 4:
					{
						ushort num3 = (ushort)(tTResponse.profilerState + (ushort)(tTResponse.profilerError << 8));
						OnStarted(new ProfilerEventArgs(propName, Action.ProfilerExtendedStart, num3));
						break;
					}
					case 5:
					{
						ushort num7 = (ushort)(tTResponse.profilerState + (ushort)(tTResponse.profilerError << 8));
						OnStopped(new ProfilerEventArgs(propName, Action.ProfilerStop, num7));
						break;
					}
					case 6:
					{
						ushort num2 = (ushort)(tTResponse.profilerState + (ushort)(tTResponse.profilerError << 8));
						if (num2 != 0)
						{
							OnError(new ProfilerEventArgs(propName, Action.ProfilerGetInfo, num2));
							break;
						}
						byte[] array = new byte[70];
						Marshal.Copy(pData, array, 0, 70);
						string text = string.Empty;
						for (int i = 6; i < 70 && array[i] != 0; i++)
						{
							string str = text;
							char c = (char)array[i];
							text = str + c.ToString();
						}
						OnInfoRead(new ProfilerEventArgs(propName, Action.ProfilerGetInfo, text));
						break;
					}
					case 7:
					{
						ushort num = (ushort)(tTResponse.profilerState + (ushort)(tTResponse.profilerError << 8));
						OnStackRead(new ProfilerEventArgs(propName, Action.ProfilerGetStack, num));
						break;
					}
					default:
						propState = (ProfilerState)tTResponse.profilerState;
						OnStateRead(new ProfilerEventArgs(propName, Action.ProfilerReadState, tTResponse.profilerError));
						break;
					}
				}
				else
				{
					switch (option)
					{
					case 1:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerInstall, errorCode));
						break;
					case 2:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerDeinstall, errorCode));
						break;
					case 3:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerStart, errorCode));
						break;
					case 4:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerExtendedStart, errorCode));
						break;
					case 5:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerStop, errorCode));
						break;
					case 6:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerGetInfo, errorCode));
						break;
					case 7:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerGetStack, errorCode));
						break;
					default:
						OnError(new ProfilerEventArgs(propName, Action.ProfilerReadState, errorCode));
						break;
					}
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		protected virtual void OnStateRead(ProfilerEventArgs e)
		{
			if (this.StateRead != null)
			{
				this.StateRead(this, e);
			}
		}

		protected virtual void OnInfoRead(ProfilerEventArgs e)
		{
			if (this.InfoRead != null)
			{
				this.InfoRead(this, e);
			}
		}

		protected virtual void OnInstalled(ProfilerEventArgs e)
		{
			if (this.Installed != null)
			{
				this.Installed(this, e);
			}
		}

		protected virtual void OnDeinstalled(ProfilerEventArgs e)
		{
			if (this.Deinstalled != null)
			{
				this.Deinstalled(this, e);
			}
		}

		protected virtual void OnStarted(ProfilerEventArgs e)
		{
			if (this.Started != null)
			{
				this.Started(this, e);
			}
		}

		protected virtual void OnStopped(ProfilerEventArgs e)
		{
			if (this.Stopped != null)
			{
				this.Stopped(this, e);
			}
		}

		protected virtual void OnStackRead(ProfilerEventArgs e)
		{
			if (this.StackRead != null)
			{
				this.StackRead(this, e);
			}
		}

		protected virtual void OnError(ProfilerEventArgs e)
		{
			if (this.Error != null)
			{
				this.Error(this, e);
			}
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
					propName = null;
					propParent = null;
				}
			}
		}
	}
}
