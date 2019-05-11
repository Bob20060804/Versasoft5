using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ersa.Global.Common.Extensions
{
	public static class EDC_ObjectExtensions
	{
		public static T FUN_edcDeepClone<T>(this T i_objToClone)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, i_objToClone);
				memoryStream.Position = 0L;
				return (T)binaryFormatter.Deserialize(memoryStream);
			}
		}

		public static int FUN_i32GetHashCodeOderNull(this object i_objWert)
		{
			return i_objWert?.GetHashCode() ?? 0;
		}

		public static string FUN_strGibNameSpace(this object i_objWert)
		{
			if (i_objWert != null)
			{
				return i_objWert.GetType().Namespace;
			}
			return string.Empty;
		}

		public static string FUN_strGibKlassenName(this object i_objWert)
		{
			if (i_objWert != null)
			{
				return i_objWert.GetType().Name;
			}
			return string.Empty;
		}
	}
}
