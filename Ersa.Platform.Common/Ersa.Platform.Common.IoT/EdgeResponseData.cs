using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ersa.Platform.Common.IoT
{
	public class EdgeResponseData
	{
		public const string ResponseSuccess = "ok";

		public const string ResponseFail = "nok";

		public const string TelemetryIntervall = "TelemetryIntervall";

		private const string ResponseVersion = "0.0.1";

		[JsonProperty(PropertyName = "version")]
		public string VersionId
		{
			get
			{
				return "0.0.1";
			}
		}

		[JsonProperty(PropertyName = "request")]
		public int IoTRequest
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "type")]
		public int DeviceTyp
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "deviceId")]
		public string DeviceId
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "state")]
		public string ResponseState
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "error")]
		public string ErrorMessage
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "parameter")]
		public Dictionary<string, object> Parameter
		{
			get;
			set;
		}

		[JsonIgnore]
		public IoTDeviceTyp IoTDeviceTypEnum
		{
			get
			{
				return (IoTDeviceTyp)DeviceTyp;
			}
			set
			{
				DeviceTyp = (int)value;
			}
		}

		[JsonIgnore]
		public IoTRequestType IoTRequestTypeEnum
		{
			get
			{
				return (IoTRequestType)IoTRequest;
			}
			set
			{
				IoTRequest = (int)value;
			}
		}
	}
}
