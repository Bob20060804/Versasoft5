using Ersa.Platform.Common;

namespace Ersa.Platform.Dienste.Loetprogramm.Helfer
{
	public static class EDC_SerialisierungsVersionsHelfer
	{
		public static string FUN_strSerialisierungsVersionErmitteln(int i_i32DateiVersion, EDC_ElementVersion i_edcVisuVersion)
		{
			return $"V{i_i32DateiVersion:000} / V{i_edcVisuVersion.PRO_strVersion}";
		}
	}
}
