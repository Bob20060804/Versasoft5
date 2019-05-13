using System.ComponentModel;
using System.Linq;

namespace Ersa.Platform.Infrastructure.Validierung
{
	public abstract class EDC_NotificationObjectMitDataErrorInfo : EDC_NotificationObjectMitValidierung, IDataErrorInfo
	{
		public string Error => string.Empty;

		public string this[string i_strPropertyName]
		{
			get
			{
				EDC_PropertyValidierungsFehler eDC_PropertyValidierungsFehler = FUN_enuValidierungsFehlerErmitteln(i_strPropertyName).FirstOrDefault();
				if (eDC_PropertyValidierungsFehler == null)
				{
					return string.Empty;
				}
				return eDC_PropertyValidierungsFehler.PRO_strFehlerKey;
			}
		}
	}
}
