using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class processingParametersType
	{
		[XmlElement("parameter")]
		public List<parameterType> lstParameter
		{
			get;
			set;
		}
	}
}
