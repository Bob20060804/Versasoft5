using System;

namespace Ersa.Global.Common.FortsetzungsPolicy
{
	public struct STRUCT_OperationsErgebnis
	{
		public static STRUCT_OperationsErgebnis PROs_sttErfolgreichesOperationsErgebnis
		{
			get
			{
				STRUCT_OperationsErgebnis result = default(STRUCT_OperationsErgebnis);
				result.PRO_fdcException = null;
				return result;
			}
		}

		public bool PRO_blnErfolgreich => PRO_fdcException == null;

		public Exception PRO_fdcException
		{
			get;
			set;
		}
	}
}
