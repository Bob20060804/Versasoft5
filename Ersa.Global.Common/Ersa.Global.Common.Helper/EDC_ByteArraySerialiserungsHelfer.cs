using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ersa.Global.Common.Helper
{
	public static class EDC_ByteArraySerialiserungsHelfer
	{
		public static byte[] FUNa_bytSerialize<T>(this T i_objParent)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, i_objParent);
				return memoryStream.ToArray();
			}
		}

		public static T FUN_objDeserialize<T>(this byte[] ia_bytArray)
		{
			using (MemoryStream serializationStream = new MemoryStream(ia_bytArray))
			{
				return (T)new BinaryFormatter().Deserialize(serializationStream);
			}
		}
	}
}
