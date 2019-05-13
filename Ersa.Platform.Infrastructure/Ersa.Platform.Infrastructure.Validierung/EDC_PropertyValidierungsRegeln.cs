using System.Collections.Generic;

namespace Ersa.Platform.Infrastructure.Validierung
{
	public class EDC_PropertyValidierungsRegeln
	{
		public string PRO_strPropertyName
		{
			get;
			private set;
		}

		public IEnumerable<EDC_PropertyValidierungsRegel> PRO_enuRegeln
		{
			get;
			private set;
		}

		public EDC_PropertyValidierungsRegeln(string i_strPropertyName, IEnumerable<EDC_PropertyValidierungsRegel> i_enuRegeln)
		{
			PRO_strPropertyName = i_strPropertyName;
			PRO_enuRegeln = i_enuRegeln;
			foreach (EDC_PropertyValidierungsRegel item in PRO_enuRegeln)
			{
				item.PRO_strPropertyName = i_strPropertyName;
			}
		}
	}
}
