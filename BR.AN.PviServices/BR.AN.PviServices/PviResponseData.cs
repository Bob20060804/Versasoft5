using System;

namespace BR.AN.PviServices
{
	internal class PviResponseData
	{
		private AccessModes propAccessMode;

		private AccessTypes propAccessType;

		private EventTypes propEventType;

		private int propErrorCode;

		private uint propLinkID;

		private int propState;

		private int propWParam;

		private int propLParam;

		private Action propAction;

		private IntPtr propPtrData;

		private int propDataLen;

		public AccessModes AccessMode => propAccessMode;

		public AccessTypes AccessType => propAccessType;

		internal EventTypes EventType => propEventType;

		public int ErrorCode => propErrorCode;

		public uint LinkID => propLinkID;

		public int State => propState;

		public int WParam => propWParam;

		public int LParam => propLParam;

		public Action Action => propAction;

		public IntPtr PtrData => propPtrData;

		public int DataLen => propDataLen;

		public bool IsError
		{
			get
			{
				if (propErrorCode == 0 || 12002 == propErrorCode)
				{
					return false;
				}
				return true;
			}
		}

		public PviResponseData(int wParam, int lParam, IntPtr pData, int dataLen, ResponseInfo info)
		{
			propDataLen = dataLen;
			propAction = (Action)lParam;
			propWParam = wParam;
			propLParam = lParam;
			propPtrData = pData;
			propAccessMode = (AccessModes)info.Mode;
			propAccessType = (AccessTypes)info.Type;
			propEventType = (EventTypes)info.Type;
			propState = info.Status;
			propLinkID = info.LinkId;
			propErrorCode = info.Error;
		}
	}
}
