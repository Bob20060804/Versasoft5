using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BR.AN.PviServices
{
	public abstract class PInvokePvicom
	{
		[DllImport("user32")]
		internal static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);

		[DllImport("user32")]
		internal static extern int GetWindowContextHelpId(int h);

		[DllImport("user32")]
		internal static extern int GetActiveWindow();

		[DllImport("user32")]
		internal static extern int GetParent(int owner);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		[DllImport("kernel32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern uint GetTickCount();

		[DllImport("pvicom.dll", EntryPoint = "PviGetVersion")]
		internal static extern void PviXCGetVersion(IntPtr pVersion, int dataLen);

		[DllImport("pvicom64.dll", EntryPoint = "PviGetVersion")]
		internal static extern void Pvi64XCGetVersion(IntPtr pVersion, int dataLen);

		[DllImport("pvicom.dll", EntryPoint = "PviXInitialize")]
		internal static extern int PviXCInitialize(out uint pviHandle, int ComTimeout, int RetryTimeMessage, [MarshalAs(UnmanagedType.LPStr)] string pInitParam, IntPtr pRes2);

		[DllImport("pvicom64.dll", EntryPoint = "PviXInitialize")]
		internal static extern int Pvi64XCInitialize(out uint pviHandle, int ComTimeout, int RetryTimeMessage, [MarshalAs(UnmanagedType.LPStr)] string pInitParam, IntPtr pRes2);

		[DllImport("pvicom64.dll", EntryPoint = "PviXCreate")]
		internal static extern int PviX64MCreate(uint hPvi, out uint pLinkId, byte[] pObjName, ObjectType nPObjType, byte[] pPObjDesc, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, byte[] pLinkDesc);

		[DllImport("pvicom.dll", EntryPoint = "PviXCreate")]
		internal static extern int PviXCCreate(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, PviCallback pCallback, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom64.dll", EntryPoint = "PviXCreate")]
		internal static extern int Pvi64XCCreate(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, PviCallback pCallback, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom.dll", EntryPoint = "PviXCreate")]
		internal static extern int PviXMCreate(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom64.dll", EntryPoint = "PviXCreate")]
		internal static extern int Pvi64XMCreate(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom.dll", EntryPoint = "PviXCreateRequest")]
		internal static extern int PviXCCreateRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, PviCallback pCallback, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXCreateRequest")]
		internal static extern int Pvi64XCCreateRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, PviCallback pCallback, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXCreateRequest")]
		internal static extern int PviXMCreateRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXCreateRequest")]
		internal static extern int Pvi64XMCreateRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, ObjectType nPObjType, [MarshalAs(UnmanagedType.LPStr)] string pPObjDesc, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXCreateResponse")]
		internal static extern int Pvi64XCreateResponse(uint hPvi, IntPtr eventMsgNo, out uint linkID);

		[DllImport("pvicom.dll")]
		internal static extern int PviXCreateResponse(uint hPvi, IntPtr eventMsgNo, out uint linkID);

		[DllImport("pvicom64.dll", EntryPoint = "PviXLinkResponse")]
		internal static extern int Pvi64XLinkResponse(uint hPvi, IntPtr eventMsgNo, out uint linkID);

		[DllImport("pvicom.dll")]
		internal static extern int PviXLinkResponse(uint hPvi, IntPtr eventMsgNo, out uint linkID);

		[DllImport("pvicom64.dll", EntryPoint = "PviXUnlinkResponse")]
		internal static extern int Pvi64XUnlinkResponse(uint hPvi, IntPtr wParam);

		[DllImport("pvicom.dll")]
		internal static extern int PviXUnlinkResponse(uint hPvi, IntPtr wParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXWriteRequest")]
		internal static extern int PviXCWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviCallback nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXWriteRequest")]
		internal static extern int Pvi64XCWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviCallback nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXWriteRequest")]
		internal static extern int Pvi64XMWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXWriteRequest")]
		internal static extern int PviXMWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXWriteResponse")]
		internal static extern int Pvi64XWriteResponse(uint hPvi, IntPtr eventMsgNo);

		[DllImport("pvicom.dll")]
		internal static extern int PviXWriteResponse(uint hPvi, IntPtr eventMsgNo);

		[DllImport("pvicom64.dll", EntryPoint = "PviXWriteResultResponse")]
		internal static extern int Pvi64XWriteResultResponse(uint hPvi, IntPtr eventMsgNo, IntPtr pData, int dataLen);

		[DllImport("pvicom.dll")]
		internal static extern int PviXWriteResultResponse(uint hPvi, IntPtr eventMsgNo, IntPtr pData, int dataLen);

		[DllImport("pvicom.dll", EntryPoint = "PviXReadRequest")]
		internal static extern int PviXCReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, PviCallback nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadRequest")]
		internal static extern int Pvi64XCReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, PviCallback nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXReadArgumentRequest")]
		internal static extern int PviXFnReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviFunction nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadArgumentRequest")]
		internal static extern int Pvi64XFnReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviFunction nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXReadRequest")]
		internal static extern int PviXFnReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, PviFunction nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadRequest")]
		internal static extern int Pvi64XFnReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, PviFunction nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXWriteRequest")]
		internal static extern int PviXFnWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviFunction nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXWriteRequest")]
		internal static extern int Pvi64XFnWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviFunction nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXGetNextResponse")]
		internal static extern int Pvi64XGetNextResponse(uint hPvi, out int wParam, IntPtr lParam, out PviFunction fnPtr, SafeWaitHandle hEvent);

		[DllImport("pvicom.dll")]
		internal static extern int PviXGetNextResponse(uint hPvi, out int wParam, IntPtr lParam, out PviFunction fnPtr, SafeWaitHandle hEvent);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadRequest")]
		internal static extern int Pvi64XMReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXReadRequest")]
		internal static extern int PviXMReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadResponse")]
		internal static extern int Pvi64XReadResponse(uint hPvi, IntPtr EventMsgNum, IntPtr pData, int dataLen);

		[DllImport("pvicom.dll")]
		internal static extern int PviXReadResponse(uint hPvi, IntPtr EventMsgNum, IntPtr pData, int dataLen);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadResponse")]
		internal static extern int Pvi64XFnReadResponse(uint hPvi, int EventMsgNum, IntPtr pData, int dataLen);

		[DllImport("pvicom.dll", EntryPoint = "PviXReadResponse")]
		internal static extern int PviXFnReadResponse(uint hPvi, int EventMsgNum, IntPtr pData, int dataLen);

		[DllImport("pvicom64.dll", EntryPoint = "PviXWriteResponse")]
		internal static extern int Pvi64XFnWriteResponse(uint hPvi, int wParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXWriteResponse")]
		internal static extern int PviXFnWriteResponse(uint hPvi, int EventMsgNum);

		[DllImport("pvicom.dll", EntryPoint = "PviXReadArgumentRequest")]
		internal static extern int PviXCReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviCallback nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadArgumentRequest")]
		internal static extern int Pvi64XCReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviCallback nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXReadArgumentRequest")]
		internal static extern int Pvi64XMReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXReadArgumentRequest")]
		internal static extern int PviXMReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr nRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXLinkRequest")]
		internal static extern int PviXCLinkRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, PviCallback pCallback, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXLinkRequest")]
		internal static extern int Pvi64XCLinkRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, PviCallback pCallback, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXLinkRequest")]
		internal static extern int Pvi64XMLinkRequest(uint hPvi, byte[] pObjName, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, byte[] pLinkDesc, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXLinkRequest")]
		internal static extern int PviXMLinkRequest(uint hPvi, byte[] pObjName, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, byte[] pLinkDesc, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXDeleteRequest")]
		internal static extern int PviXCDeleteRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXDeleteRequest")]
		internal static extern int Pvi64XCDeleteRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXDeleteRequest")]
		internal static extern int Pvi64XMDeleteRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXDeleteRequest")]
		internal static extern int PviXMDeleteRequest(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXDeleteResponse")]
		internal static extern int Pvi64XDeleteResponse(uint hPvi, IntPtr wParam);

		[DllImport("pvicom.dll")]
		internal static extern int PviXDeleteResponse(uint hPvi, IntPtr wParam);

		[DllImport("pvicom.dll")]
		internal static extern int PviXDelete(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName);

		[DllImport("pvicom64.dll", EntryPoint = "PviXDelete")]
		internal static extern int Pvi64XDelete(uint hPvi, [MarshalAs(UnmanagedType.LPStr)] string pObjName);

		[DllImport("pvicom.dll", EntryPoint = "PviXUnlinkRequest")]
		internal static extern int PviXCUnlinkRequest(uint hPvi, uint linkID, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXUnlinkRequest")]
		internal static extern int Pvi64XCUnlinkRequest(uint hPvi, uint linkID, PviCallback hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXUnlinkRequest")]
		internal static extern int Pvi64XMUnlinkRequest(uint hPvi, uint linkID, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXUnlinkRequest")]
		internal static extern int PviXMUnlinkRequest(uint hPvi, uint linkID, IntPtr hRespMsg, uint respMsgNo, uint respParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXUnlink")]
		internal static extern int Pvi64XUnlink(uint hPvi, uint linkID);

		[DllImport("pvicom.dll")]
		internal static extern int PviXUnlink(uint hPvi, uint linkID);

		[DllImport("PVICom64.dll", EntryPoint = "PviXGetResponseInfo")]
		internal static extern int Pvi64XGetResponseInfo(uint hPvi, IntPtr wParam, IntPtr pParam, out uint pDataLen, ref ResponseInfo pInfo, uint InfoLen);

		[DllImport("PVICom.dll")]
		internal static extern int PviXGetResponseInfo(uint hPvi, IntPtr wParam, IntPtr pParam, out uint pDataLen, ref ResponseInfo pInfo, uint InfoLen);

		[DllImport("PVICom64.dll", EntryPoint = "PviXGetResponseInfo")]
		internal static extern int Pvi64XFnGetResponseInfo(uint hPvi, int wParam, out int pParam, out uint pDataLen, ref ResponseInfo pInfo, uint InfoLen);

		[DllImport("PVICom.dll", EntryPoint = "PviXGetResponseInfo")]
		internal static extern int PviXFnGetResponseInfo(uint hPvi, int wParam, out uint pParam, out uint pDataLen, ref ResponseInfo pInfo, uint InfoLen);

		[DllImport("pvicom64.dll", EntryPoint = "PviXWrite")]
		internal static extern int Pvi64XWrite(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr pRstData, int rstDataLen);

		[DllImport("pvicom.dll")]
		internal static extern int PviXWrite(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr pRstData, int rstDataLen);

		[DllImport("pvicom64.dll", EntryPoint = "PviXRead")]
		internal static extern int Pvi64XRead(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pArgData, int argDataLen, IntPtr pData, int dataLen);

		[DllImport("pvicom.dll")]
		internal static extern int PviXRead(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pArgData, int argDataLen, IntPtr pData, int dataLen);

		[DllImport("pvicom.dll", EntryPoint = "PviXLink")]
		internal static extern int PviXCLink(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, PviCallback hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom64.dll", EntryPoint = "PviXLink")]
		internal static extern int Pvi64XCLink(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, PviCallback hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom.dll", EntryPoint = "PviXLink")]
		internal static extern int PviXMLink(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom64.dll", EntryPoint = "PviXLink")]
		internal static extern int Pvi64XMLink(uint hPvi, out uint pLinkId, [MarshalAs(UnmanagedType.LPStr)] string pObjName, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, [MarshalAs(UnmanagedType.LPStr)] string pLinkDesc);

		[DllImport("pvicom.dll", EntryPoint = "PviXInitialize")]
		internal static extern int PviXMInitialize(out uint pviHandle, int ComTimeout, int RetryTimeMessage, byte[] pInitParam, IntPtr pRes2);

		[DllImport("pvicom64.dll", EntryPoint = "PviXInitialize")]
		internal static extern int Pvi64XMInitialize(out uint pviHandle, int ComTimeout, int RetryTimeMessage, byte[] pInitParam, IntPtr pRes2);

		[DllImport("pvicom.dll", EntryPoint = "PviXSetGlobEventMsg")]
		internal static extern int PviXCSetGlobEventMsg(uint pviHandle, uint globalEvents, PviCallback pCallback, uint eventMsgNo, uint eventParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXSetGlobEventMsg")]
		internal static extern int Pvi64XCSetGlobEventMsg(uint pviHandle, uint globalEvents, PviCallback pCallback, uint eventMsgNo, uint eventParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXSetGlobEventMsg")]
		internal static extern int PviXFSetGlobEventMsg(uint pviHandle, uint globalEvents, PviMessageFunction pCallback, uint eventMsgNo, uint eventParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXSetGlobEventMsg")]
		internal static extern int Pvi64XFSetGlobEventMsg(uint pviHandle, uint globalEvents, PviMessageFunction pCallback, uint eventMsgNo, uint eventParam);

		[DllImport("pvicom64.dll", EntryPoint = "PviXSetGlobEventMsg")]
		internal static extern int Pvi64XMSetGlobEventMsg(uint pviHandle, uint globalEvents, IntPtr pEventMsg, uint eventMsgNo, uint eventParam);

		[DllImport("pvicom.dll", EntryPoint = "PviXSetGlobEventMsg")]
		internal static extern int PviXMSetGlobEventMsg(uint pviHandle, uint globalEvents, IntPtr pEventMsg, uint eventMsgNo, uint eventParam);

		[DllImport("PVICom64.dll", EntryPoint = "PviXDeinitialize")]
		internal static extern int Pvi64XDeinitialize(uint hPvi);

		[DllImport("PVICom.dll")]
		internal static extern int PviXDeinitialize(uint hPvi);

		[DllImport("PVICom64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#31")]
		internal static extern int Pvi64ServerClient(IntPtr PviSecure, int flag);

		[DllImport("PVICom.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#31")]
		internal static extern int PviServerClient(IntPtr PviSecure, int flag);

		internal static int PviComServerClient(IntPtr PviSecure, int flag)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64ServerClient(PviSecure, flag);
			}
			return PviServerClient(PviSecure, flag);
		}

		internal static int PviComInitialize(out uint pviHandle, int commTimeout, int retryTimeMessage, string pInitParam, IntPtr pRes2)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCInitialize(out pviHandle, commTimeout, retryTimeMessage, pInitParam, pRes2);
			}
			return PviXCInitialize(out pviHandle, commTimeout, retryTimeMessage, pInitParam, pRes2);
		}

		internal static void PviComGetVersionInfo(IntPtr pVersion, int dataLen)
		{
			if (8 == IntPtr.Size)
			{
				Pvi64XCGetVersion(pVersion, dataLen);
			}
			else
			{
				PviXCGetVersion(pVersion, dataLen);
			}
		}

		internal static int PviComDeinitialize(uint pviHandle)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XDeinitialize(pviHandle);
			}
			return PviXDeinitialize(pviHandle);
		}

		internal static int PviComMsgInitialize(out uint pviHandle, int commTimeout, int retryTimeMessage, byte[] pInitParam, IntPtr pRes2)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMInitialize(out pviHandle, commTimeout, retryTimeMessage, pInitParam, pRes2);
			}
			return PviXMInitialize(out pviHandle, commTimeout, retryTimeMessage, pInitParam, pRes2);
		}

		internal static int PviComSetGlobEventMsg(uint pviHandle, uint globalEvents, PviCallback pCallback, uint eventMsgNo, uint eventParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCSetGlobEventMsg(pviHandle, globalEvents, pCallback, eventMsgNo, eventParam);
			}
			return PviXCSetGlobEventMsg(pviHandle, globalEvents, pCallback, eventMsgNo, eventParam);
		}

		internal static int PviComMsgSetGlobEventMsg(uint pviHandle, uint globalEvents, IntPtr pEventMsg, uint eventMsgNo, uint eventParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMSetGlobEventMsg(pviHandle, globalEvents, pEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam);
			}
			return PviXMSetGlobEventMsg(pviHandle, globalEvents, pEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam);
		}

		internal static int PviComMsgDeleteRequest(uint hPvi, string pObjName, IntPtr hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMDeleteRequest(hPvi, pObjName, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
			}
			return PviXMDeleteRequest(hPvi, pObjName, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
		}

		internal static int PviComLink(uint hPvi, out uint pLinkId, string pObjName, PviCallback hEventMsg, uint eventMsgNo, uint eventParam, string pLinkDesc)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCLink(hPvi, out pLinkId, pObjName, hEventMsg, eventMsgNo, eventParam, pLinkDesc);
			}
			return PviXCLink(hPvi, out pLinkId, pObjName, hEventMsg, eventMsgNo, eventParam, pLinkDesc);
		}

		internal static int PviComMsgLink(uint hPvi, out uint pLinkId, string prcObjName, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, string linkDesc)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMLink(hPvi, out pLinkId, prcObjName, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, linkDesc);
			}
			return PviXMLink(hPvi, out pLinkId, prcObjName, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, linkDesc);
		}

		internal static int PviComUnlink(uint hPvi, uint linkID)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XUnlink(hPvi, linkID);
			}
			return PviXUnlink(hPvi, linkID);
		}

		internal static int PviComRead(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pArgData, int argDataLen, IntPtr pData, int dataLen)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XRead(hPvi, linkID, nAccess, pArgData, argDataLen, pData, dataLen);
			}
			return PviXRead(hPvi, linkID, nAccess, pArgData, argDataLen, pData, dataLen);
		}

		internal static int PviComReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, PviCallback nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCReadRequest(hPvi, linkID, nAccess, nRespMsg, respMsgNo, respParam);
			}
			return PviXCReadRequest(hPvi, linkID, nAccess, nRespMsg, respMsgNo, respParam);
		}

		internal static int PviComFnReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviFunction nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XFnReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
			}
			return PviXFnReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
		}

		internal static int PviComFnReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, PviFunction nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XFnReadRequest(hPvi, linkID, nAccess, nRespMsg, respMsgNo, respParam);
			}
			return PviXFnReadRequest(hPvi, linkID, nAccess, nRespMsg, respMsgNo, respParam);
		}

		internal static int PviComFnWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviFunction nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XFnWriteRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
			}
			return PviXFnWriteRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
		}

		internal static int PviComGetNextResponse(uint hPvi, out int wParam, IntPtr lParam, out PviFunction respFnPtr, SafeWaitHandle eventSignal)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XGetNextResponse(hPvi, out wParam, lParam, out respFnPtr, eventSignal);
			}
			return PviXGetNextResponse(hPvi, out wParam, lParam, out respFnPtr, eventSignal);
		}

		internal static int PviComMsgReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMReadRequest(hPvi, linkID, nAccess, nRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
			}
			return PviXMReadRequest(hPvi, linkID, nAccess, nRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
		}

		internal static int PviComUnlinkRequest(uint hPvi, uint linkID, PviCallback hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCUnlinkRequest(hPvi, linkID, hRespMsg, respMsgNo, respParam);
			}
			return PviXCUnlinkRequest(hPvi, linkID, hRespMsg, respMsgNo, respParam);
		}

		internal static int PviComMsgUnlinkRequest(uint hPvi, uint linkID, IntPtr hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMUnlinkRequest(hPvi, linkID, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
			}
			return PviXMUnlinkRequest(hPvi, linkID, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
		}

		internal static int PviComMsgCreateRequest(uint hPvi, string prcObjName, ObjectType nPObjType, string prcObjDesc, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, string linkDesc, IntPtr hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMCreateRequest(hPvi, prcObjName, nPObjType, prcObjDesc, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, linkDesc, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
			}
			return PviXMCreateRequest(hPvi, prcObjName, nPObjType, prcObjDesc, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, linkDesc, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
		}

		internal static int PviComCreateRequest(uint hPvi, string prcObjName, ObjectType nPObjType, string prcObjDesc, PviCallback hEventMsg, uint eventMsgNo, uint eventParam, string linkDesc, PviCallback hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCCreateRequest(hPvi, prcObjName, nPObjType, prcObjDesc, hEventMsg, eventMsgNo, eventParam, linkDesc, hRespMsg, respMsgNo, respParam);
			}
			return PviXCCreateRequest(hPvi, prcObjName, nPObjType, prcObjDesc, hEventMsg, eventMsgNo, eventParam, linkDesc, hRespMsg, respMsgNo, respParam);
		}

		internal static int PviComMsgLinkRequest(uint hPvi, byte[] prcObjName, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, byte[] prcLinkDesc, IntPtr hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMLinkRequest(hPvi, prcObjName, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, prcLinkDesc, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
			}
			return PviXMLinkRequest(hPvi, prcObjName, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, prcLinkDesc, hRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
		}

		internal static int PviComLinkRequest(uint hPvi, string prcObjName, PviCallback hEventMsg, uint eventMsgNo, uint eventParam, string prcLinkDesc, PviCallback hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCLinkRequest(hPvi, prcObjName, hEventMsg, eventMsgNo, eventParam, prcLinkDesc, hRespMsg, respMsgNo, respParam);
			}
			return PviXCLinkRequest(hPvi, prcObjName, hEventMsg, eventMsgNo, eventParam, prcLinkDesc, hRespMsg, respMsgNo, respParam);
		}

		internal static int PviComWrite(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr pRstData, int rstDataLen)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XWrite(hPvi, linkID, nAccess, pData, dataLen, pRstData, rstDataLen);
			}
			return PviXWrite(hPvi, linkID, nAccess, pData, dataLen, pRstData, rstDataLen);
		}

		internal static int PviComWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviCallback nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCWriteRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
			}
			return PviXCWriteRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
		}

		internal static int PviComMsgWriteRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMWriteRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
			}
			return PviXMWriteRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
		}

		internal static int PviComReadArgumentRequest(Service sObject, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, uint respMsgNo, uint respParam)
		{
			if (sObject.EventMessageType == EventMessageType.CallBack)
			{
				return PviComReadArgumentRequest(sObject.hPvi, linkID, nAccess, pData, dataLen, sObject.cbRead, 4294967294u, respParam);
			}
			return PviComMsgReadArgumentRequest(sObject.hPvi, linkID, nAccess, pData, dataLen, sObject.WindowHandle, respParam, respMsgNo);
		}

		internal static int PviComReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, PviCallback nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
			}
			return PviXCReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, respMsgNo, respParam);
		}

		internal static int PviComMsgReadArgumentRequest(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen, IntPtr nRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
			}
			return PviXMReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, nRespMsg, Base.MakeWindowMessage(respMsgNo), respParam);
		}

		internal static int PviComDeleteRequest(uint hPvi, string prcObjName, PviCallback hRespMsg, uint respMsgNo, uint respParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCDeleteRequest(hPvi, prcObjName, hRespMsg, respMsgNo, respParam);
			}
			return PviXCDeleteRequest(hPvi, prcObjName, hRespMsg, respMsgNo, respParam);
		}

		internal static int PviComDelete(uint hPvi, string prcObjName)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XDelete(hPvi, prcObjName);
			}
			return PviXDelete(hPvi, prcObjName);
		}

		internal static int PviComCreateResponse(uint hPvi, IntPtr eventMsgNo, out uint linkID)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCreateResponse(hPvi, eventMsgNo, out linkID);
			}
			return PviXCreateResponse(hPvi, eventMsgNo, out linkID);
		}

		internal static int PviComMsgCreate(uint hPvi, out uint pLinkId, string prcObjName, ObjectType nPObjType, string prcObjDesc, IntPtr hEventMsg, uint eventMsgNo, uint eventParam, string linkDesc)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XMCreate(hPvi, out pLinkId, prcObjName, nPObjType, prcObjDesc, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, linkDesc);
			}
			return PviXMCreate(hPvi, out pLinkId, prcObjName, nPObjType, prcObjDesc, hEventMsg, Base.MakeWindowMessage(eventMsgNo), eventParam, linkDesc);
		}

		internal static int PviComCreate(uint hPvi, out uint pLinkId, string prcObjName, ObjectType nPObjType, string prcObjDesc, PviCallback pCallBack, uint eventMsgNo, uint eventParam, string linkDesc)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XCCreate(hPvi, out pLinkId, prcObjName, nPObjType, prcObjDesc, pCallBack, eventMsgNo, eventParam, linkDesc);
			}
			return PviXCCreate(hPvi, out pLinkId, prcObjName, nPObjType, prcObjDesc, pCallBack, eventMsgNo, eventParam, linkDesc);
		}

		internal static int PviComGetResponseInfo(uint hPvi, IntPtr wParam, IntPtr pParam, out uint pDataLen, ref ResponseInfo pInfo, uint infoLen)
		{
			int num = 0;
			if (8 == IntPtr.Size)
			{
				return Pvi64XGetResponseInfo(hPvi, wParam, pParam, out pDataLen, ref pInfo, infoLen);
			}
			return PviXGetResponseInfo(hPvi, wParam, pParam, out pDataLen, ref pInfo, infoLen);
		}

		internal static int PviComFnGetResponseInfo(uint hPvi, int wParam, out int pParam, out uint pDataLen, ref ResponseInfo pInfo, uint infoLen)
		{
			int num = 0;
			if (8 == IntPtr.Size)
			{
				num = Pvi64XFnGetResponseInfo(hPvi, wParam, out pParam, out pDataLen, ref pInfo, infoLen);
			}
			else
			{
				num = PviXFnGetResponseInfo(hPvi, wParam, out uint pParam2, out pDataLen, ref pInfo, infoLen);
				pParam = (int)pParam2;
			}
			return num;
		}

		internal static int PviComReadResponse(uint hPvi, IntPtr eventMsgNum, IntPtr pData, int dataLen)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XReadResponse(hPvi, eventMsgNum, pData, dataLen);
			}
			return PviXReadResponse(hPvi, eventMsgNum, pData, dataLen);
		}

		internal static int PviComFnReadResponse(uint hPvi, int eventMsgNum, IntPtr pData, int dataLen)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XFnReadResponse(hPvi, eventMsgNum, pData, dataLen);
			}
			return PviXFnReadResponse(hPvi, eventMsgNum, pData, dataLen);
		}

		internal static int PviComFnWriteResponse(uint hPvi, int eventMsgNum)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XFnWriteResponse(hPvi, eventMsgNum);
			}
			return PviXFnWriteResponse(hPvi, eventMsgNum);
		}

		internal static int PviComWriteResponse(uint hPvi, IntPtr eventMsgNum)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XWriteResponse(hPvi, eventMsgNum);
			}
			return PviXWriteResponse(hPvi, eventMsgNum);
		}

		internal static int PviComUnlinkResponse(uint hPvi, IntPtr wParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XUnlinkResponse(hPvi, wParam);
			}
			return PviXUnlinkResponse(hPvi, wParam);
		}

		internal static int PviComLinkResponse(uint hPvi, IntPtr eventMsgNo, out uint linkID)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XLinkResponse(hPvi, eventMsgNo, out linkID);
			}
			return PviXLinkResponse(hPvi, eventMsgNo, out linkID);
		}

		internal static int PviComFunctionSetGlobEventMsg(uint pviHandle, uint globalEvents, PviMessageFunction pFunction, uint eventMsgNo, uint eventParam)
		{
			if (8 == IntPtr.Size)
			{
				return Pvi64XFSetGlobEventMsg(pviHandle, globalEvents, pFunction, eventMsgNo, eventParam);
			}
			return PviXFSetGlobEventMsg(pviHandle, globalEvents, pFunction, eventMsgNo, eventParam);
		}
	}
}
