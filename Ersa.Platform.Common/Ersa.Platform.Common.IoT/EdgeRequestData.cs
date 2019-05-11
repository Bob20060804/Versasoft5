using Ersa.Global.Common.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ersa.Platform.Common.IoT
{
	public class EdgeRequestData
	{
		private const string RequestVersion = "0.0.1";

		[JsonProperty(PropertyName = "version")]
		public string VersionId
		{
			get
			{
				return "0.0.1";
			}
		}

		[JsonProperty(PropertyName = "timestamp")]
		public DateTime Timestamp
		{
			get;
			set;
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

		[JsonProperty(PropertyName = "devicemode")]
		public int OperatingMode
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

		[JsonProperty(PropertyName = "classification")]
		public int Classification
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "visuName")]
		public string VisuName
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "visuVersion")]
		public string VisuVersion
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "firmware")]
		public string FirmwareVersion
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "user")]
		public string UserName
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "note")]
		public string Note
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "file")]
		public string FileName
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "content")]
		public string FileContent
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "teledouble")]
		public Dictionary<string, double> TelemetryDouble
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "telestring")]
		public Dictionary<string, string> TelemetryString
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "messages")]
		public Dictionary<int, List<string>> Messages
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

		[JsonIgnore]
		public IntelloClassification ClassificationEnum
		{
			get
			{
				return (IntelloClassification)Classification;
			}
			set
			{
				Classification = (int)value;
			}
		}

		[JsonIgnore]
		public IoTDeviceOperatingMode OperatingModeEnum
		{
			get
			{
				return (IoTDeviceOperatingMode)OperatingMode;
			}
			set
			{
				OperatingMode = (int)value;
			}
		}

		public void SetAttachmentFile(string file)
		{
			if (!string.IsNullOrEmpty(file))
			{
				FileInfo fileInfo = new FileInfo(file);
				if (fileInfo.Exists)
				{
					FileName = fileInfo.Name;
					FileContent = GetFileContentAsBase64(file);
				}
			}
		}

		public byte[] GetBytesFromFileContent()
		{
			return EDC_Base64Helfer.FUN_bytGetBytesFromBase64String(FileContent);
		}

		public string WriteFileContentToFile(string path)
		{
			return EDC_Base64Helfer.FUN_strWriteFileContentToFile(FileName, path, FileContent);
		}

		private string GetFileContentAsBase64(string file)
		{
			return EDC_Base64Helfer.FUN_strGetFileContentAsBase64(file);
		}
	}
}
