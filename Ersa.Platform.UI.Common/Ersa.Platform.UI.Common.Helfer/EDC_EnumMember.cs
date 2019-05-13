using Ersa.Platform.Infrastructure;
using System.Windows.Media;

namespace Ersa.Platform.UI.Common.Helfer
{
	public class EDC_EnumMember : EDC_NotificationObjectMitSprachUmschaltung
	{
		public object PRO_enmValue
		{
			get;
			set;
		}

		public int PRO_i32Value
		{
			get;
			set;
		}

		public string PRO_strDescription
		{
			get;
			set;
		}

		public Brush PRO_fdcFarbe
		{
			get;
			set;
		}
	}
}
