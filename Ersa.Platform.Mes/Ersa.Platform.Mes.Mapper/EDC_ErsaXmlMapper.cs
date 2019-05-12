using Ersa.Platform.Common.Mes;
using Ersa.Platform.Mes.Interfaces;
using Ersa.Platform.Mes.Konfiguration;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Ersa.Platform.Mes.Mapper
{
	[Export(typeof(INF_MesProtokollMapper))]
	public class EDC_ErsaXmlMapper : INF_MesProtokollMapper
	{
		public ENUM_MesProtokollFormat PRO_enuFormat => ENUM_MesProtokollFormat.Xml;

		public string FUN_strMap(ENUM_MesFunktionen i_enuFunktion, Dictionary<ENUM_MesMaschinenDatenArgumente, object> i_dicArgumente)
		{
			XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "no"));
			XElement xElement = new XElement(i_enuFunktion.FUN_objHoleBezeichner());
			xDocument.Add(xElement);
			foreach (ENUM_MesMaschinenDatenArgumente key in i_dicArgumente.Keys)
			{
				XElement content = new XElement(key.FUN_objHoleBezeichner(), i_dicArgumente[key]);
				xElement.Add(content);
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamWriter textWriter = new StreamWriter(memoryStream, Encoding.UTF8))
				{
					xDocument.Save((TextWriter)textWriter);
					return Encoding.UTF8.GetString(memoryStream.ToArray());
				}
			}
		}
	}
}
