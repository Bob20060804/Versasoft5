using System.Windows;

namespace Ersa.Platform.Common.Routing
{
	public interface INF_RoutbaresObjekt
	{
		int Id
		{
			get;
			set;
		}

		Rect InspectionArea
		{
			get;
		}

		int OrderNr
		{
			get;
			set;
		}
	}
}
