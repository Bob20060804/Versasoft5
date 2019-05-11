using System;

namespace Ersa.Platform.Common.Exceptions
{
	public class EDC_KeinLoetParameterIstSollConverterRegistriert : Exception
	{
		public EDC_KeinLoetParameterIstSollConverterRegistriert()
		{
		}

		public EDC_KeinLoetParameterIstSollConverterRegistriert(string i_strMessage)
			: base(i_strMessage)
		{
		}
	}
}
