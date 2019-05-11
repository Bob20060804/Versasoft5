using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BR.AN.PviServices
{
	internal class PlcMessageWindow : Form
	{
		private IntPtr m_pData;

		private uint m_LenOfpData;

		private Service propService;

		public PlcMessageWindow(Service service)
		{
			propService = service;
			m_pData = IntPtr.Zero;
			m_LenOfpData = 0u;
			Text = "-§-BR.AN.PviServices MSG Window-$-";
		}

		private void WndProcServiceConnect(Message msg)
		{
			IntPtr zero = IntPtr.Zero;
			uint pDataLen = 0u;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			int num = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			num = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			propService.Callback(0, 0, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcServiceDisconnect(Message msg)
		{
			IntPtr zero = IntPtr.Zero;
			uint pDataLen = 0u;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			int num = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			num = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			propService.Callback(0, 0, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcServiceArrange(Message msg)
		{
			IntPtr zero = IntPtr.Zero;
			uint pDataLen = 0u;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			int num = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			num = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			propService.Callback(0, 0, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableActivate(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 5, error, 0);
			propService.PVICB_WriteA(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableDeactivate(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 5, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcDisconnect(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 8, 10, error, 0);
			propService.PVICB_Unlink(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcModuleDownload(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 21, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcTaskStart(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 276, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcCpuWriteTime(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 22, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcTaskStop(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 277, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcCpuModuleDelete(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 29, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcModuleDelete(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 280, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcClearMemory(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 281, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcCpuWriteSavePath(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 290, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcCpuStop(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 263, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcCpuStart(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 264, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableSetHysteresis(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 16, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableAccessTypeChange(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 13, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariablePollingPropertyChange(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 5, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableValueWrite(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 11, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableSetRefreshTime(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 15, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcEvent(int iWParam, int iLParam, Message msg)
		{
			IntPtr zero = IntPtr.Zero;
			uint pDataLen = 0u;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			int num = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			num = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			propService.PVICB_Event(iWParam, iLParam, m_pData, pDataLen, ref info);
		}

		private void WndProcConnect(int iWParam, int iLParam, Message msg)
		{
			uint linkID;
			int error = PInvokePvicom.PviComCreateResponse(propService.hPvi, msg.WParam, out linkID);
			ResponseInfo info = new ResponseInfo((int)linkID, 4, 0, error, 0);
			propService.PVICB_Create(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcInfoService(int iWParam, int iLParam, Message msg)
		{
			IntPtr zero = IntPtr.Zero;
			uint pDataLen = 0u;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			int num = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			num = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			if (info.Mode == 1)
			{
				propService.PVICB_Event(iWParam, iLParam, m_pData, pDataLen, ref info);
			}
			else
			{
				propService.PVICB_Read(iWParam, iLParam, m_pData, pDataLen, ref info);
			}
		}

		private void WndProcLink(int iWParam, int iLParam, Message msg)
		{
			uint linkID = 0u;
			int error = PInvokePvicom.PviComLinkResponse(propService.hPvi, msg.WParam, out linkID);
			ResponseInfo info = new ResponseInfo((int)linkID, 6, 0, error, 0);
			propService.PVICB_Link(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableReadInternal(int iWParam, int iLParam, Message msg)
		{
			uint pDataLen = 0u;
			int error = 0;
			IntPtr zero = IntPtr.Zero;
			ResponseInfo info = new ResponseInfo(0, 2, 11, error, 0);
			error = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			error = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			propService.PVICB_ReadE(iWParam, iLParam, m_pData, pDataLen, ref info);
		}

		private void WndProcVariableInternLink(int iWParam, int iLParam, Message msg)
		{
			uint linkID = 0u;
			int error = PInvokePvicom.PviComLinkResponse(propService.hPvi, msg.WParam, out linkID);
			ResponseInfo info = new ResponseInfo((int)linkID, 6, 0, error, 0);
			propService.PVICB_LinkA(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableInternUnlink(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComUnlinkResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 8, 0, error, 0);
			propService.PVICB_UnlinkA(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableUnlinkStructConnect(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComUnlinkResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 8, 0, error, 0);
			propService.PVICB_UnlinkD(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private void WndProcVariableInternFormat(int iWParam, int iLParam, Message msg)
		{
			uint pDataLen = 0u;
			int error = 0;
			IntPtr zero = IntPtr.Zero;
			ResponseInfo info = new ResponseInfo(0, 2, 14, error, 0);
			error = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			error = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			if (info.Mode == 6)
			{
				propService.PVICB_LinkA(iWParam, iLParam, m_pData, pDataLen, ref info);
			}
			else
			{
				propService.PVICB_EventA(iWParam, iLParam, m_pData, pDataLen, ref info);
			}
		}

		private void WndProcVariableReadFormatInternal(int iWParam, int iLParam, Message msg)
		{
			uint pDataLen = 0u;
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			num = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out pDataLen, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (pDataLen > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = pDataLen;
				m_pData = PviMarshal.AllocHGlobal(pDataLen);
			}
			num = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)pDataLen));
			propService.PVICB_ReadA(iWParam, iLParam, m_pData, pDataLen, ref info);
		}

		private void WndProcVariableExtendedTypInfo(int iWParam, int iLParam, Message msg)
		{
			uint num = 0u;
			int num2 = 0;
			IntPtr zero = IntPtr.Zero;
			num = 0u;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			num2 = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out num, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (num > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = num;
				m_pData = PviMarshal.AllocHGlobal(num);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = num;
				m_pData = PviMarshal.AllocHGlobal(num);
			}
			num2 = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)num));
			propService.PVICB_ReadE(iWParam, iLParam, m_pData, num, ref info);
		}

		private void WndProcLoggerDetectSGType(int iWParam, int iLParam, Message msg)
		{
			uint num = 0u;
			int num2 = 0;
			IntPtr zero = IntPtr.Zero;
			num = 0u;
			ResponseInfo info = new ResponseInfo(0, 0, 0, 0, 0);
			num2 = PInvokePvicom.PviComGetResponseInfo(propService.hPvi, msg.WParam, zero, out num, ref info, (uint)Marshal.SizeOf(typeof(ResponseInfo)));
			if (num > m_LenOfpData)
			{
				if (IntPtr.Zero != m_pData)
				{
					PviMarshal.FreeHGlobal(ref m_pData);
				}
				m_pData = IntPtr.Zero;
				m_LenOfpData = num;
				m_pData = PviMarshal.AllocHGlobal(num);
			}
			else if (IntPtr.Zero == m_pData)
			{
				m_LenOfpData = num;
				m_pData = PviMarshal.AllocHGlobal(num);
			}
			num2 = (info.Status = PInvokePvicom.PviComReadResponse(propService.hPvi, msg.WParam, m_pData, (int)num));
			switch (info.Mode)
			{
			case 1:
				propService.PVICB_EventA(iWParam, iLParam, m_pData, num, ref info);
				break;
			case 2:
				propService.PVICB_Read(iWParam, iLParam, m_pData, num, ref info);
				break;
			case 4:
				propService.PVICB_Create(iWParam, iLParam, m_pData, num, ref info);
				break;
			case 3:
				propService.PVICB_Write(iWParam, iLParam, m_pData, num, ref info);
				break;
			}
		}

		private void WndProcCpuGlobalForceOFF(int iWParam, int iLParam, Message msg)
		{
			int error = PInvokePvicom.PviComWriteResponse(propService.hPvi, msg.WParam);
			ResponseInfo info = new ResponseInfo(0, 3, 297, error, 0);
			propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0u, ref info);
		}

		private bool IsWndProcCaseDisconnect(uint wmMsg)
		{
			switch (wmMsg)
			{
			case 2801u:
				return true;
			case 2807u:
				return true;
			case 2804u:
				return true;
			case 202u:
				return true;
			case 502u:
				return true;
			case 602u:
				return true;
			case 402u:
				return true;
			default:
				return false;
			}
		}

		private bool IsWndProcCaseDelete(uint wmMsg)
		{
			switch (wmMsg)
			{
			case 305u:
				return true;
			case 406u:
				return true;
			case 918u:
				return true;
			default:
				return false;
			}
		}

		private bool IsWndProcCaseEvent(uint wmMsg)
		{
			switch (wmMsg)
			{
			case 707u:
				return true;
			case 705u:
				return true;
			case 2802u:
				return true;
			case 2805u:
				return true;
			case 2808u:
				return true;
			case 550u:
				return true;
			case 703u:
				return true;
			default:
				return false;
			}
		}

		private bool IsWndProcCaseConnect(uint wmMsg)
		{
			switch (wmMsg)
			{
			case 201u:
				return true;
			case 501u:
				return true;
			case 401u:
				return true;
			case 301u:
				return true;
			case 909u:
				return true;
			case 2800u:
				return true;
			case 2803u:
				return true;
			case 2806u:
				return true;
			default:
				return false;
			}
		}

		private bool IsWndProcCaseInfoService(uint wmMsg)
		{
			switch (wmMsg)
			{
			case 269u:
				return true;
			case 505u:
				return true;
			case 917u:
				return true;
			case 2812u:
				return true;
			case 613u:
				return true;
			case 617u:
				return true;
			case 216u:
				return true;
			case 291u:
				return true;
			case 616u:
				return true;
			case 621u:
				return true;
			case 1202u:
				return true;
			case 614u:
				return true;
			case 615u:
				return true;
			case 622u:
				return true;
			case 624u:
				return true;
			case 415u:
				return true;
			case 700u:
				return true;
			case 212u:
				return true;
			case 1070u:
				return true;
			case 702u:
				return true;
			case 721u:
				return true;
			case 120u:
				return true;
			case 150u:
				return true;
			case 111u:
				return true;
			case 114u:
				return true;
			case 116u:
				return true;
			case 118u:
				return true;
			case 722u:
				return true;
			case 725u:
				return true;
			case 726u:
				return true;
			case 727u:
				return true;
			case 728u:
				return true;
			case 729u:
				return true;
			default:
				return false;
			}
		}

		private bool IsWndProcCaseLink(uint wmMsg)
		{
			switch (wmMsg)
			{
			case 704u:
				return true;
			case 708u:
				return true;
			case 706u:
				return true;
			case 701u:
				return true;
			case 112u:
				return true;
			case 123u:
				return true;
			case 151u:
				return true;
			case 121u:
				return true;
			case 124u:
				return true;
			case 113u:
				return true;
			case 115u:
				return true;
			case 117u:
				return true;
			case 119u:
				return true;
			default:
				return false;
			}
		}

		private int GetWndProcCase(uint wmMsg)
		{
			int result = (int)wmMsg;
			if (IsWndProcCaseDisconnect(wmMsg))
			{
				result = -1;
			}
			if (IsWndProcCaseDelete(wmMsg))
			{
				result = -2;
			}
			if (IsWndProcCaseEvent(wmMsg))
			{
				result = -3;
			}
			if (IsWndProcCaseConnect(wmMsg))
			{
				result = -4;
			}
			if (IsWndProcCaseInfoService(wmMsg))
			{
				result = -5;
			}
			if (IsWndProcCaseLink(wmMsg))
			{
				result = -6;
			}
			return result;
		}

		protected override void WndProc(ref Message msg)
		{
			base.WndProc(ref msg);
			if (msg.Msg >= 10000)
			{
				IntPtr zero = IntPtr.Zero;
				int iWParam = 0;
				int iLParam = 0;
				uint wmMsg = (uint)(msg.Msg - 10000);
				PviMarshal.WmMsgToInt32(ref msg, ref iWParam, ref iLParam);
				switch (GetWndProcCase(wmMsg))
				{
				case 97:
					propService.DisconnectEx(PviMarshal.WmMsgToUInt32(msg.WParam));
					break;
				case 101:
					WndProcServiceConnect(msg);
					break;
				case 102:
					WndProcServiceDisconnect(msg);
					break;
				case 103:
					WndProcServiceArrange(msg);
					break;
				case 503:
					WndProcVariableActivate(iWParam, iLParam, msg);
					break;
				case 504:
					WndProcVariableDeactivate(iWParam, iLParam, msg);
					break;
				case -1:
					WndProcDisconnect(iWParam, iLParam, msg);
					break;
				case 307:
					WndProcModuleDownload(iWParam, iLParam, msg);
					break;
				case 403:
					WndProcTaskStart(iWParam, iLParam, msg);
					break;
				case 213:
					WndProcCpuWriteTime(iWParam, iLParam, msg);
					break;
				case 404:
					WndProcTaskStop(iWParam, iLParam, msg);
					break;
				case 219:
					WndProcCpuModuleDelete(iWParam, iLParam, msg);
					break;
				case -2:
					WndProcModuleDelete(iWParam, iLParam, msg);
					break;
				case 618:
					WndProcClearMemory(iWParam, iLParam, msg);
					break;
				case 215:
					WndProcCpuWriteSavePath(iWParam, iLParam, msg);
					break;
				case 204:
					WndProcCpuStop(iWParam, iLParam, msg);
					break;
				case 203:
				case 207:
					WndProcCpuStart(iWParam, iLParam, msg);
					break;
				case 514:
					WndProcVariableSetHysteresis(iWParam, iLParam, msg);
					break;
				case 551:
					WndProcVariableAccessTypeChange(iWParam, iLParam, msg);
					break;
				case 552:
					WndProcVariablePollingPropertyChange(iWParam, iLParam, msg);
					break;
				case 506:
					WndProcVariableValueWrite(iWParam, iLParam, msg);
					break;
				case 512:
					WndProcVariableSetRefreshTime(iWParam, iLParam, msg);
					break;
				case -3:
					WndProcEvent(iWParam, iLParam, msg);
					break;
				case -4:
					WndProcConnect(iWParam, iLParam, msg);
					break;
				case -5:
					WndProcInfoService(iWParam, iLParam, msg);
					break;
				case -6:
					WndProcLink(iWParam, iLParam, msg);
					break;
				case 2809:
					WndProcVariableReadInternal(iWParam, iLParam, msg);
					break;
				case 709:
					WndProcVariableInternLink(iWParam, iLParam, msg);
					break;
				case 710:
					WndProcVariableInternUnlink(iWParam, iLParam, msg);
					break;
				case 2813:
					WndProcVariableUnlinkStructConnect(iWParam, iLParam, msg);
					break;
				case 711:
					WndProcVariableInternFormat(iWParam, iLParam, msg);
					break;
				case 2810:
					WndProcVariableReadFormatInternal(iWParam, iLParam, msg);
					break;
				case 2811:
					WndProcVariableExtendedTypInfo(iWParam, iLParam, msg);
					break;
				case 916:
					WndProcLoggerDetectSGType(iWParam, iLParam, msg);
					break;
				case 222:
					WndProcCpuGlobalForceOFF(iWParam, iLParam, msg);
					break;
				}
			}
		}
	}
}
