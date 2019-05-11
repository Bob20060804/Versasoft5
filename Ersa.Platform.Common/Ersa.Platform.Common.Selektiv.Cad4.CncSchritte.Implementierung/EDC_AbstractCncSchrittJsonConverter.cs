using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Fluxer;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Loeten;
using Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung.Uebergeordnet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung
{
	public class EDC_AbstractCncSchrittJsonConverter : JsonConverter
	{
		private static readonly JsonSerializerSettings ms_fdcSpecifiedSubclassConversion = new JsonSerializerSettings
		{
			ContractResolver = new EDC_BaseSpecifiedConcreteClassConverter()
		};

		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter i_fdcWriter, object i_objValue, JsonSerializer i_fdcSerializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader i_fdcReader, Type i_fdcObjectType, object i_objExistingValue, JsonSerializer i_fdcSerializer)
		{
			JObject jObject = JObject.Load(i_fdcReader);
			switch (jObject["PRO_edcTyp"].Value<long>())
			{
			case 0L:
				return JsonConvert.DeserializeObject<EDC_FluxerDosisSchritt>(jObject.ToString(), ms_fdcSpecifiedSubclassConversion);
			case 3L:
				return JsonConvert.DeserializeObject<EDC_HorizontalBewegungSchritt>(jObject.ToString(), ms_fdcSpecifiedSubclassConversion);
			case 1L:
				return JsonConvert.DeserializeObject<EDC_LoetwellenhoeheSchritt>(jObject.ToString(), ms_fdcSpecifiedSubclassConversion);
			case 4L:
				return JsonConvert.DeserializeObject<EDC_WartenSchritt>(jObject.ToString(), ms_fdcSpecifiedSubclassConversion);
			case 2L:
				return JsonConvert.DeserializeObject<EDC_VertikalBewegungSchritt>(jObject.ToString(), ms_fdcSpecifiedSubclassConversion);
			case 5L:
				return JsonConvert.DeserializeObject<EDC_WeiterfahrenSchritt>(jObject.ToString(), ms_fdcSpecifiedSubclassConversion);
			default:
				throw new Exception("Unsupported Type!");
			}
		}

		public override bool CanConvert(Type i_fdcObjectType)
		{
			return i_fdcObjectType == typeof(EDC_AbstractCncSchritt);
		}
	}
}
