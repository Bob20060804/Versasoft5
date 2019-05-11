using System;

namespace BR.AN.PviServices
{
	public class PviEventArgs : EventArgs
	{
		internal string propErrorText = string.Empty;

		internal string propCurLanguage = string.Empty;

		internal Service propService;

		internal string propName;

		internal string propAddress;

		internal int propErrorCode;

		internal string propLanguage;

		internal Action propAction;

		public string Name => propName;

		public string Address => propAddress;

		public int ErrorCode => propErrorCode;

		public string ErrorText
		{
			get
			{
				if (propErrorText == string.Empty)
				{
					propCurLanguage = propLanguage;
					if (propService == null)
					{
						propErrorText = Service.GetErrorText(propErrorCode, propLanguage);
					}
					else
					{
						propErrorText = propService.Utilities.GetErrorText(propErrorCode, propLanguage);
					}
					if (propErrorText == null)
					{
						return "";
					}
					return propErrorText;
				}
				if (propCurLanguage.CompareTo(propLanguage) == 0)
				{
					return propErrorText;
				}
				propCurLanguage = propLanguage;
				if (propService == null)
				{
					propErrorText = Service.GetErrorText(propErrorCode, propLanguage);
				}
				else
				{
					propErrorText = propService.Utilities.GetErrorText(propErrorCode, propLanguage);
				}
				if (propErrorText == null)
				{
					return "";
				}
				return propErrorText;
			}
		}

		public Action Action => propAction;

		public PviEventArgs(string name, string address, int errorCode, string language, Action action)
		{
			propName = name;
			propAddress = address;
			propErrorCode = errorCode;
			propLanguage = language;
			propAction = action;
		}

		internal PviEventArgs(string name, string address, int errorCode, string language, Action action, Service service)
		{
			propName = name;
			propAddress = address;
			propErrorCode = errorCode;
			propLanguage = language;
			propAction = action;
			propService = service;
		}

		internal void SetErrorCode(int errorCode)
		{
			propErrorCode = errorCode;
		}
	}
}
