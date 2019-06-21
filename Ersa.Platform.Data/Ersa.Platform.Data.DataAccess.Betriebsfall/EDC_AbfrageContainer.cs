using System.Collections.Generic;

namespace Ersa.Platform.Data.DataAccess.Betriebsfall
{
	public class EDC_AbfrageContainer
	{
		public object PRO_objSelectObjekt
		{
			get;
			set;
		}

		public Dictionary<string, object> PRO_dicDictionary
		{
			get;
			set;
		}

		public EDC_AbfrageContainer(object i_objSelectObjekt, Dictionary<string, object> i_dicDictionary)
		{
			PRO_objSelectObjekt = i_objSelectObjekt;
			PRO_dicDictionary = i_dicDictionary;
		}
	}
}
