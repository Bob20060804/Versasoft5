using Ersa.Global.Mvvm;
using Newtonsoft.Json;
using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung
{
	[Serializable]
	[JsonConverter(typeof(EDC_AbstractCncSchrittJsonConverter))]
	public abstract class EDC_AbstractCncSchritt : BindableBase
	{
		public abstract ENUM_AbstractCncSchrittTyp PRO_edcTyp
		{
			get;
		}

		public EDC_AbstractCncSchritt SUB_ErstelleKopie()
		{
			return MemberwiseClone() as EDC_AbstractCncSchritt;
		}

		public override bool Equals(object i_objObj)
		{
			EDC_AbstractCncSchritt eDC_AbstractCncSchritt = i_objObj as EDC_AbstractCncSchritt;
			if (eDC_AbstractCncSchritt != null)
			{
				return FUN_blnIstIdentisch(eDC_AbstractCncSchritt);
			}
			return false;
		}

		public abstract bool FUN_blnIstIdentisch(EDC_AbstractCncSchritt i_edcVergleichsSchritt);
	}
}
