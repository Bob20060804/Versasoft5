using System;
using System.Collections.Generic;

namespace Ersa.Global.Common.Extensions
{
	public static class EDC_ExceptionExtensions
	{
		public static IEnumerable<Exception> FUN_enuHoleInnerExceptions(this Exception i_fdcException)
		{
			if (i_fdcException != null)
			{
				Exception fdcInnerException = i_fdcException;
				do
				{
					yield return fdcInnerException;
					fdcInnerException = fdcInnerException.InnerException;
				}
				while (fdcInnerException != null);
			}
		}
	}
}
