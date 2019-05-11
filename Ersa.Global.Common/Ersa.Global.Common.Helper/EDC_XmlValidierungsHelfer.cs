using System;
using System.IO;
using System.Xml;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_XmlValidierungsHelfer
	{
		public static bool FUN_blnPruefeXmlWellFormed(string i_strXml)
		{
			using (XmlReader xmlReader = XmlReader.Create(new StringReader(i_strXml)))
			{
				try
				{
					while (xmlReader.Read())
					{
					}
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}
	}
}
