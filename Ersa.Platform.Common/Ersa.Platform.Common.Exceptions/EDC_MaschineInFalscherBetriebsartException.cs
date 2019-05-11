using System;

namespace Ersa.Platform.Common.Exceptions
{
	[Serializable]
	public class EDC_MaschineInFalscherBetriebsartException : Exception
	{
		public EDC_MaschineInFalscherBetriebsartException(params string[] ia_strZulaessigeBetriebsarten)
			: base(FUN_strMessageErstellen(ia_strZulaessigeBetriebsarten))
		{
		}

		private static string FUN_strMessageErstellen(string[] ia_strZulaessigeBetriebsarten)
		{
			return string.Format("The maschine is in the wrong operation mode. Allowed modes: {0}", (ia_strZulaessigeBetriebsarten == null) ? "none" : string.Join(", ", ia_strZulaessigeBetriebsarten));
		}
	}
}
