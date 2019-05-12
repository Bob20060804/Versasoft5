using Ersa.Global.Dienste.Exceptions;
using Ersa.Global.Dienste.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.IO;

namespace Ersa.Global.Dienste
{
	[Export("Ersa.JsonSerialisierer", typeof(INF_SerialisierungsDienst))]
	[Export(typeof(INF_JsonSerialisierungsDienst))]
	public class EDC_JsonSerialisierungsDienst : INF_JsonSerialisierungsDienst, INF_SerialisierungsDienst
	{
		public string FUN_strSerialisieren<T>(T i_objObjekt)
		{
			try
			{
				return JsonConvert.SerializeObject(i_objObjekt, Formatting.Indented);
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error serializing object!", i_fdcInner);
			}
		}

		public void SUB_ValidiereGegenSchemaDatei(string i_strInhalt, string i_strSchemaDatei)
		{
		}

		public byte[] FUNa_bytSerialisieren<T>(T i_objObjekt)
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BsonDataWriter jsonWriter = new BsonDataWriter(memoryStream))
					{
						new JsonSerializer().Serialize(jsonWriter, i_objObjekt);
						return memoryStream.ToArray();
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error serializing object!", i_fdcInner);
			}
		}

		public T FUN_objDeserialisieren<T>(string i_strFormatierterString)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(i_strFormatierterString);
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error deserializing object!", i_fdcInner);
			}
		}

		public T FUN_objDeserialisieren<T>(byte[] ia_bytInhalt)
		{
			try
			{
				using (MemoryStream stream = new MemoryStream(ia_bytInhalt))
				{
					using (BsonDataReader bsonDataReader = new BsonDataReader(stream))
					{
						if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
						{
							bsonDataReader.ReadRootValueAsArray = true;
						}
						return new JsonSerializer().Deserialize<T>(bsonDataReader);
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error deserializing object!", i_fdcInner);
			}
		}

		public T FUN_objDeserialisieren<T>(string i_strFormatierterString, JsonSerializerSettings i_fdcSettings)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(i_strFormatierterString, i_fdcSettings);
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error deserializing object!", i_fdcInner);
			}
		}

		public string FUN_strSerialisierenMitTypInformationen<T>(T i_objObjekt)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			};
			try
			{
				return JsonConvert.SerializeObject(i_objObjekt, Formatting.Indented, settings);
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error serializing object!", i_fdcInner);
			}
		}

		public T FUN_objDeserialisierenMitTypInformationen<T>(string i_strFormatierterString)
		{
			try
			{
				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All
				};
				return JsonConvert.DeserializeObject<T>(i_strFormatierterString, settings);
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error deserializing object!", i_fdcInner);
			}
		}
	}
}
