using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Model.Extensions
{
	public static class EDC_ArrayParameterExtensions
	{
		public static IList<T> FUN_lstTypisierteListeLaden2D<T>(this EDC_ArrayParameter i_edcArrayParameter)
		{
			return i_edcArrayParameter.PRO_lstValue.Cast<EDC_ArrayParameter>().SelectMany((EDC_ArrayParameter i_edcParameter) => i_edcParameter.PRO_lstValue).Cast<T>()
				.ToList();
		}

		public static IList<T> FUN_lstTypisierteListeLaden<T>(this EDC_ArrayParameter i_edcArrayParameter)
		{
			return i_edcArrayParameter.PRO_lstValue.Cast<T>().ToList();
		}
	}
}
