using System;
using System.Xml;

namespace BR.AN.PviServices
{
	internal class ModuleInfoDescription
	{
		internal ushort dm_index;

		internal ushort pi_index;

		internal byte instP_valid;

		internal byte instP_value;

		internal DomainState dm_state;

		internal ProgramState pi_state;

		internal byte pi_count;

		internal uint address;

		internal uint length;

		internal string name;

		internal byte version;

		internal byte revision;

		internal uint erz_time;

		internal uint and_time;

		internal string erz_name;

		internal string and_name;

		internal TaskClassType task_class;

		internal byte install_no;

		internal ushort pv_idx;

		internal ushort pv_cnt;

		internal uint mem_ana_adr;

		internal uint mem_dig_adr;

		internal MemoryType mem_location;

		internal ModuleType type;

		internal uint createTime;

		internal uint andTime;

		internal uint progInvocation;

		internal int modListed;

		internal ModuleInfoDescription()
		{
			Initialize();
		}

		internal void Initialize()
		{
			dm_index = 0;
			pi_index = 0;
			instP_valid = 0;
			instP_value = 0;
			dm_state = DomainState.NonExistent;
			pi_state = ProgramState.NonExistent;
			pi_count = 0;
			address = 0u;
			length = 0u;
			name = "";
			version = 0;
			revision = 0;
			erz_time = 0u;
			and_time = 0u;
			erz_name = "";
			and_name = "";
			task_class = TaskClassType.NotValid;
			install_no = 0;
			pv_idx = 0;
			pv_cnt = 0;
			mem_ana_adr = 0u;
			mem_dig_adr = 0u;
			mem_location = MemoryType.SystemRom;
			type = ModuleType.Unknown;
			createTime = 0u;
			andTime = 0u;
			progInvocation = 0u;
		}

		internal void Init(APIFC_ModulInfoRes apifcInfo)
		{
			Initialize();
			dm_index = apifcInfo.dm_index;
			instP_valid = apifcInfo.instP_valid;
			instP_value = apifcInfo.instP_value;
			dm_state = apifcInfo.dm_state;
			pi_state = apifcInfo.pi_state;
			pi_count = apifcInfo.pi_count;
			address = apifcInfo.address;
			length = apifcInfo.length;
			name = apifcInfo.name;
			version = apifcInfo.version;
			revision = apifcInfo.revision;
			erz_time = apifcInfo.erz_time;
			and_time = apifcInfo.and_time;
			erz_name = apifcInfo.erz_name;
			and_name = apifcInfo.and_name;
			task_class = apifcInfo.task_class;
			install_no = apifcInfo.install_no;
			pv_idx = apifcInfo.pv_idx;
			pv_cnt = apifcInfo.pv_cnt;
			mem_ana_adr = apifcInfo.mem_ana_adr;
			mem_dig_adr = apifcInfo.mem_dig_adr;
			mem_location = apifcInfo.mem_location;
			type = apifcInfo.type;
			createTime = apifcInfo.erz_time;
			andTime = apifcInfo.and_time;
		}

		internal void UpdateAPIFCModulInfoRes(ref APIFC_ModulInfoRes apifcInfo)
		{
			apifcInfo.dm_index = dm_index;
			apifcInfo.instP_valid = instP_valid;
			apifcInfo.instP_value = instP_value;
			apifcInfo.dm_state = dm_state;
			apifcInfo.pi_state = pi_state;
			apifcInfo.pi_count = pi_count;
			apifcInfo.address = address;
			apifcInfo.length = length;
			apifcInfo.name = name;
			apifcInfo.version = version;
			apifcInfo.revision = revision;
			apifcInfo.erz_time = erz_time;
			apifcInfo.and_time = and_time;
			apifcInfo.erz_name = erz_name;
			apifcInfo.and_name = and_name;
			apifcInfo.task_class = task_class;
			apifcInfo.install_no = install_no;
			apifcInfo.pv_idx = pv_idx;
			apifcInfo.pv_cnt = pv_cnt;
			apifcInfo.mem_ana_adr = mem_ana_adr;
			apifcInfo.mem_dig_adr = mem_dig_adr;
			apifcInfo.mem_location = mem_location;
			apifcInfo.type = type;
			apifcInfo.erz_time = createTime;
			apifcInfo.and_time = andTime;
		}

		internal int ReadFromXML(XmlTextReader xmlTReader)
		{
			string text = "";
			string text2 = "";
			int result = 0;
			Initialize();
			try
			{
				text = xmlTReader.GetAttribute("Name");
				if (text != null && 0 < text.Length)
				{
					name = text;
				}
				text = xmlTReader.GetAttribute("Listed");
				if (text != null && 0 < text.Length)
				{
					modListed = Convert.ToInt32(text);
				}
				else
				{
					modListed = 3;
				}
				text = xmlTReader.GetAttribute("Size");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						length = Convert.ToUInt32(text);
					}
					else
					{
						length = Convert.ToUInt32(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("MemType");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						mem_location = (MemoryType)Convert.ToUInt32(text);
					}
					else
					{
						mem_location = (MemoryType)Convert.ToUInt32(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("Version");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						version = Convert.ToByte(text);
					}
					else
					{
						version = Convert.ToByte(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("Revision");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						revision = Convert.ToByte(text);
					}
					else
					{
						revision = Convert.ToByte(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("ModulType");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						type = (ModuleType)Convert.ToUInt32(text);
					}
					else
					{
						type = (ModuleType)Convert.ToUInt32(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("TaskClass");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						task_class = (TaskClassType)Convert.ToUInt32(text);
					}
					else
					{
						task_class = (TaskClassType)Convert.ToUInt32(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("InstallNo");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						install_no = Convert.ToByte(text);
					}
					else
					{
						install_no = Convert.ToByte(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("ModulState");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						pi_state = (ProgramState)Convert.ToUInt32(text);
					}
					else
					{
						pi_state = (ProgramState)Convert.ToUInt32(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("DomainOvIndex");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						dm_index = Convert.ToUInt16(text);
					}
					else
					{
						dm_index = Convert.ToUInt16(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("InvocationOvIndex");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						pi_index = Convert.ToUInt16(text);
					}
					else
					{
						pi_index = Convert.ToUInt16(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("DomainModulState");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						dm_state = (DomainState)Convert.ToUInt32(text);
					}
					else
					{
						dm_state = (DomainState)Convert.ToUInt32(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("ProgInvocation");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
					{
						progInvocation = Convert.ToUInt32(text);
					}
					else
					{
						progInvocation = Convert.ToUInt32(text, 16);
					}
				}
				text = xmlTReader.GetAttribute("Time");
				if (text != null && 0 < text.Length)
				{
					if (-1 == text.IndexOf('-') && -1 == text.IndexOf('.'))
					{
						and_time = Convert.ToUInt32(text);
						createTime = (erz_time = and_time);
					}
					else
					{
						int num = 0;
						text = text.Replace('-', '/');
						num = text.IndexOf('/', 0);
						num = text.IndexOf('/', num + 1);
						num = text.IndexOf('/', num + 1);
						text2 = text.Substring(num + 1);
						text = text.Substring(0, num);
						text2 = text2.Replace('/', ':');
						text = text + " " + text2;
						and_time = Pvi.DateTimeToUInt32(DateTime.Parse(text));
						createTime = (erz_time = and_time);
					}
				}
				text = xmlTReader.GetAttribute("RawTimeErzT5");
				if (text != null && 0 < text.Length && -1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
				{
					erz_time = (createTime = PviMarshal.TimeToUInt32(text));
				}
				text = xmlTReader.GetAttribute("RawTimeAenT5");
				if (text != null && 0 < text.Length && -1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
				{
					and_time = (andTime = PviMarshal.TimeToUInt32(text));
				}
				text = xmlTReader.GetAttribute("Address");
				if (text == null)
				{
					return result;
				}
				if (0 >= text.Length)
				{
					return result;
				}
				if (-1 == text.IndexOf('x') && -1 == text.IndexOf('X'))
				{
					address = Convert.ToUInt32(text);
					return result;
				}
				address = Convert.ToUInt32(text, 16);
				return result;
			}
			catch
			{
				return 12054;
			}
		}
	}
}
