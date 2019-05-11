using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.Common
{
	public class EDC_ElementVersion : BindableBase
	{
		public string PRO_strElementName
		{
			get;
		}

		public Version PRO_fdcVersion
		{
			get;
		}

		public string PRO_strVersion
		{
			get
			{
				if (PRO_fdcVersion.Revision <= 0)
				{
					return $"{PRO_fdcVersion.Major}.{PRO_fdcVersion.Minor:000}.{PRO_fdcVersion.Build:000}";
				}
				return $"{PRO_fdcVersion.Major}.{PRO_fdcVersion.Minor:000}.{PRO_fdcVersion.Build:000}-{PRO_fdcVersion.Revision}";
			}
		}

		public EDC_ElementVersion(string i_strElementName, Version i_fdcVersion)
		{
			PRO_strElementName = i_strElementName;
			PRO_fdcVersion = i_fdcVersion;
		}

		public override string ToString()
		{
			return string.Format("Element: {0}, Version: {1}", PRO_strElementName, (PRO_fdcVersion == null) ? "null" : PRO_strVersion);
		}
	}
}
