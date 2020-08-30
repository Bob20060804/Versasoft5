using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class processingParametersRequestType
	{
		[XmlElement("parameter")]
		public parameterRequestType[] parameter
		{
			get;
			set;
		}
	}
}
