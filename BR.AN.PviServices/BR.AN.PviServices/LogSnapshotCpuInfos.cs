using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
	[CLSCompliant(true)]
	public class LogSnapshotCpuInfos
	{
		[Flags]
		private enum PlcInfoReadStates
		{
			InitialState = 0x0,
			BasicInfoRead = 0x1,
			HardwareInfoRead = 0x2,
			RedundancyInfoRead = 0x4,
			TableOfContensRead = 0x8,
			TechnologyPackageInfoRead = 0x10,
			LogInfoRead = 0x20,
			InfoUpdateComplete = 0x3F
		}

		private const string TechPkgCommandDataV1 = "<Request><Command Class=\"SystemElement\" Service=\"GetTableOfContents\" Version=\"1\"/></Request>";

		private const string TechPkgCommandDataV2 = "<Request><Command Class=\"SystemElement\" Service=\"GetTableOfContents\" Version=\"2\"/></Request>";

		private const string KeyForCpuInfo = "CpuInfo";

		private Cpu _cpuParent;

		private Logger _sysLogModule;

		private PlcInfoReadStates _snapShotPlcInfoStateMachine;

		private int _technologyPackageRequestVersion;

		public string Name
		{
			get;
			private set;
		}

		public Hashtable InfoXmlStrings
		{
			get;
			private set;
		}

		public event PviEventHandler Updated;

		public LogSnapshotCpuInfos(string name)
		{
			_cpuParent = null;
			Name = name;
			InfoXmlStrings = null;
			_snapShotPlcInfoStateMachine = PlcInfoReadStates.InitialState;
		}

		private void ChekFireUpdatedEvent(PlcInfoReadStates newState)
		{
			_snapShotPlcInfoStateMachine |= newState;
			if (PlcInfoReadStates.InfoUpdateComplete == _snapShotPlcInfoStateMachine)
			{
				OnUpdated(new PviEventArgs(_cpuParent.Name, _cpuParent.Address, 0, _cpuParent.Service.Language, Action.LogBookSnapshotReadPlcInfos));
			}
		}

		private void AddPlcInfoString(string infoKey, string infoData, PlcInfoReadStates infoStep)
		{
			if (!Pvi.IsNullOrEmpty(infoData))
			{
				if (InfoXmlStrings.ContainsKey(infoKey))
				{
					InfoXmlStrings[infoKey] = infoData;
				}
				else
				{
					InfoXmlStrings.Add(infoKey, infoData);
				}
			}
			ChekFireUpdatedEvent(infoStep);
		}

		public string AppendCpuInfoFromLoggerInfo(string additionalCpuInfo)
		{
			if (Pvi.IsNullOrEmpty(additionalCpuInfo))
			{
				return null;
			}
			string text = "";
			if (InfoXmlStrings.ContainsKey("CpuInfo"))
			{
				text = InfoXmlStrings["CpuInfo"].ToString();
			}
			if (Pvi.IsNullOrEmpty(text))
			{
				return additionalCpuInfo;
			}
			if (text.IndexOf("<CpuInfo", StringComparison.Ordinal) != 0)
			{
				return text.Replace("<CpuInfo", "<CpuInfo " + additionalCpuInfo);
			}
			return additionalCpuInfo.Replace("<CpuInfo ", "<CpuInfo " + text + " ");
		}

		private void ParentCpuPviReadResponse(object sender, int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen)
		{
			string text = PviMarshal.PtrToStringAnsi(pData);
			switch (accessType)
			{
			case PVIReadAccessTypes.ANSL_CpuInfo:
				AddPlcInfoString("CpuInfo", AppendCpuInfoFromLoggerInfo(text), PlcInfoReadStates.BasicInfoRead);
				break;
			case PVIReadAccessTypes.ANSL_RedundancyInfo:
				AddPlcInfoString("CpuRedInfo", text, PlcInfoReadStates.RedundancyInfoRead);
				break;
			case PVIReadAccessTypes.ANSL_HardwareInfo:
				AddPlcInfoString("Hardware", text, PlcInfoReadStates.HardwareInfoRead);
				break;
			case PVIReadAccessTypes.ANSL_CpuExtendedInfo:
				AddPlcInfoString("TOC", text, PlcInfoReadStates.TableOfContensRead);
				break;
			}
		}

		private string DecodeBase64DataToString(string xmlData)
		{
			XmlReader xmlReader = null;
			string result = "";
			try
			{
				xmlReader = XmlReader.Create(new StringReader(xmlData));
				xmlReader.MoveToContent();
				do
				{
					if (xmlReader.NodeType == XmlNodeType.Element && !(xmlReader.Name != "Parameter"))
					{
						string attribute = xmlReader.GetAttribute("ID");
						if (!(attribute != "TableOfContents"))
						{
							byte[] bytes = Convert.FromBase64String(xmlReader.ReadElementString());
							result = Encoding.UTF8.GetString(bytes);
							return result;
						}
					}
				}
				while (xmlReader.Read());
				return result;
			}
			catch
			{
				return result;
			}
			finally
			{
				xmlReader?.Close();
			}
		}

		private string BuildTechPkgString(string tocResponse)
		{
			string result = "";
			Hashtable hashtable = new Hashtable();
			if (!Pvi.IsNullOrEmpty(tocResponse))
			{
				result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<TechnologyPackages>\n";
				XmlReader xmlReader = null;
				try
				{
					xmlReader = XmlReader.Create(new StringReader(tocResponse));
					xmlReader.MoveToContent();
					do
					{
						if (xmlReader.NodeType == XmlNodeType.Element && !(xmlReader.Name != "Element"))
						{
							string attribute = xmlReader.GetAttribute("TechnologyPackage");
							if (Pvi.IsNullOrEmpty(attribute))
							{
								attribute = xmlReader.GetAttribute("Domain");
							}
							if (!Pvi.IsNullOrEmpty(attribute) && !hashtable.ContainsKey(attribute))
							{
								string attribute2 = xmlReader.GetAttribute("Version");
								if (!Pvi.IsNullOrEmpty(attribute2))
								{
									hashtable.Add(attribute, attribute2);
									result += $"<Package Name=\"{attribute}\" Version=\"{attribute2}\" />\n";
								}
							}
						}
					}
					while (xmlReader.Read());
				}
				catch
				{
				}
				finally
				{
					xmlReader?.Close();
				}
				result += "</TechnologyPackages>";
			}
			return result;
		}

		private string GetTechnologyPackageData(string xmlData)
		{
			string result = "";
			if (!Pvi.IsNullOrEmpty(xmlData))
			{
				string tocResponse = DecodeBase64DataToString(xmlData);
				result = BuildTechPkgString(tocResponse);
			}
			return result;
		}

		private void ParentCpuPviWriteResponse(object sender, int errorCode, PVIWriteAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen)
		{
			if (accessType != PVIWriteAccessTypes.ANSL_COMMAND_Data)
			{
				return;
			}
			if (errorCode != 0 && 1 < _technologyPackageRequestVersion)
			{
				_technologyPackageRequestVersion = 1;
				_cpuParent.UpdateTechnologyPackages("<Request><Command Class=\"SystemElement\" Service=\"GetTableOfContents\" Version=\"1\"/></Request>");
				return;
			}
			string xmlData = "";
			if (dataLen != 0 && IntPtr.Zero != pData)
			{
				try
				{
					xmlData = PviMarshal.PtrToStringAnsi(pData);
				}
				catch
				{
					xmlData = "";
				}
			}
			string technologyPackageData = GetTechnologyPackageData(xmlData);
			AddPlcInfoString("TechnologyPackages", technologyPackageData, PlcInfoReadStates.TechnologyPackageInfoRead);
		}

		public int Update(Cpu parentCpu)
		{
			if (parentCpu == null)
			{
				return -1;
			}
			_cpuParent = parentCpu;
			parentCpu.PviReadResponse += ParentCpuPviReadResponse;
			parentCpu.PviWriteResponse += ParentCpuPviWriteResponse;
			InfoXmlStrings = new Hashtable();
			InfoXmlStrings.Clear();
			_snapShotPlcInfoStateMachine = PlcInfoReadStates.InitialState;
			_cpuParent.UpdateCpuBasicInfo();
			_cpuParent.UpdateHardwareInfo();
			_cpuParent.UpdateRedundancyInfo();
			_cpuParent.UpdateTableOfContens();
			_technologyPackageRequestVersion = 2;
			_cpuParent.UpdateTechnologyPackages("<Request><Command Class=\"SystemElement\" Service=\"GetTableOfContents\" Version=\"2\"/></Request>");
			_sysLogModule = new Logger(_cpuParent, "TemporaryLogSnapShotSysLoggerModule", doNotAddToCollections: true);
			_sysLogModule.Address = "$arlogsys";
			_sysLogModule.Connected += SysLogModuleConnected;
			_sysLogModule.Connect();
			return 0;
		}

		private void SysLogModuleConnected(object sender, PviEventArgs e)
		{
			_sysLogModule.Connected -= SysLogModuleConnected;
			if (e.ErrorCode != 0)
			{
				_sysLogModule.Disconnect(noResponse: true);
				AddPlcInfoString("CpuInfo", AppendCpuInfoFromLoggerInfo(null), PlcInfoReadStates.LogInfoRead);
				return;
			}
			_sysLogModule.LoggerInfoRead += SysLogModuleLoggerInfoRead;
			if (_sysLogModule.ReadLoggerInfo() != 0)
			{
				_sysLogModule.Disconnect(noResponse: true);
				_sysLogModule.LoggerInfoRead -= SysLogModuleLoggerInfoRead;
				AddPlcInfoString("CpuInfo", AppendCpuInfoFromLoggerInfo(null), PlcInfoReadStates.LogInfoRead);
			}
		}

		private string BuildExtraCpuInfoString(string logInfo)
		{
			string text = "";
			XmlReader xmlReader = null;
			try
			{
				xmlReader = XmlReader.Create(new StringReader(logInfo));
				xmlReader.MoveToContent();
				do
				{
					if (xmlReader.NodeType == XmlNodeType.Element && !(xmlReader.Name != "LoggerInfo"))
					{
						string attribute = xmlReader.GetAttribute("OffsetUtc");
						if (!Pvi.IsNullOrEmpty(attribute))
						{
							text += $"OffsetUtc=\"{attribute}\" ";
						}
						string attribute2 = xmlReader.GetAttribute("DaylightSaving");
						if (Pvi.IsNullOrEmpty(attribute2))
						{
							return text;
						}
						text += $"DaylightSaving=\"{attribute2}\" ";
						return text;
					}
				}
				while (xmlReader.Read());
				return text;
			}
			catch
			{
				return text;
			}
			finally
			{
				xmlReader?.Close();
			}
		}

		private void SysLogModuleLoggerInfoRead(object sender, PviEventArgsXML e)
		{
			string additionalCpuInfo = "";
			_sysLogModule.LoggerInfoRead -= SysLogModuleLoggerInfoRead;
			_sysLogModule.Disconnect(noResponse: true);
			_sysLogModule.Remove();
			_sysLogModule = null;
			if (e.ErrorCode == 0)
			{
				additionalCpuInfo = BuildExtraCpuInfoString(e.XMLData);
			}
			AddPlcInfoString("CpuInfo", AppendCpuInfoFromLoggerInfo(additionalCpuInfo), PlcInfoReadStates.LogInfoRead);
		}

		protected virtual void OnUpdated(PviEventArgs e)
		{
			PviEventHandler updated = this.Updated;
			_cpuParent.PviReadResponse -= ParentCpuPviReadResponse;
			_cpuParent.PviWriteResponse -= ParentCpuPviWriteResponse;
			updated?.Invoke(_cpuParent, e);
		}
	}
}
