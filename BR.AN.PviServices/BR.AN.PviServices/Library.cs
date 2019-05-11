using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
	public class Library : Base
	{
		internal Hashtable propFunctions;

		internal LibraryType propType;

		internal Cpu propCpu;

		public Hashtable Functions => propFunctions;

		public LibraryType Type => propType;

		public override string FullName
		{
			get
			{
				if (propName != null && 0 < base.Name.Length)
				{
					return Parent.FullName + "." + base.Name;
				}
				if (Parent != null)
				{
					return Parent.FullName;
				}
				return "";
			}
		}

		public override string PviPathName
		{
			get
			{
				if (base.Name != null && 0 < base.Name.Length)
				{
					return Parent.PviPathName + "/\"" + propName + "\"";
				}
				return Parent.PviPathName;
			}
		}

		public Cpu Cpu => propCpu;

		public event PviEventHandler FunctionsUploaded;

		public Library(Cpu cpu, string name)
			: base(cpu, name)
		{
			propFunctions = new Hashtable();
			propCpu = cpu;
			propParent = cpu;
			propCpu.Libraries.Add(this);
		}

		public int Disconnect(int internalAction)
		{
			return UnlinkRequest((uint)internalAction);
		}

		public virtual void UploadFunctions()
		{
			int num = 0;
			string text = "LIB=" + propName + " QU=0";
			Service.BuildRequestBuffer(text);
			num = ReadArgumentRequest(Service.hPvi, ((Cpu)propParent).LinkId, AccessTypes.LibraryList, Service.RequestBuffer, text.Length, 614u, base.InternId);
			if (num != 0)
			{
				OnError(new PviEventArgs(base.Name, base.Address, num, Service.Language, Action.LibFunctionsUpload, Service));
			}
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			propErrorCode = errorCode;
			if (accessType == PVIReadAccessTypes.LibraryList)
			{
				if (base.ErrorCode == 0 && 0 < dataLen)
				{
					string text = "";
					text = PviMarshal.PtrToStringAnsi(pData, dataLen);
					string[] array = null;
					if (!(text != "") || 1 >= text.Length)
					{
						return;
					}
					array = text.Split("\t".ToCharArray());
					for (int i = 0; i < array.Length; i++)
					{
						Function function = null;
						string text2 = array[i].Substring(0, array[i].IndexOf(" "));
						if (!propFunctions.ContainsKey(text2))
						{
							function = new Function(this, text2);
							int num = array[i].IndexOf("REF=");
							if (-1 != num)
							{
								function.propReference = Convert.ToByte(array[i].Substring(num + 4));
							}
						}
					}
					OnFunctionsUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.LibFunctionsUpload, Service));
				}
				else if (base.ErrorCode != 0)
				{
					OnFunctionsUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.LibFunctionsUpload, Service));
					OnError(new PviEventArgs(base.Name, base.Address, base.ErrorCode, Service.Language, Action.LibFunctionsUpload, Service));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		protected virtual void OnFunctionsUploaded(PviEventArgs e)
		{
			if (this.FunctionsUploaded != null)
			{
				this.FunctionsUploaded(this, e);
			}
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			base.ToXMLTextWriter(ref writer, flags);
			writer.WriteAttributeString("Type", propType.ToString());
			if (propFunctions.Count > 0)
			{
				writer.WriteStartElement("Functions");
				foreach (Function value in propFunctions.Values)
				{
					writer.WriteStartElement("Function");
					writer.WriteAttributeString("Name", value.Name);
					if (value.propReference != 0)
					{
						writer.WriteAttributeString("Reference", value.Reference.ToString());
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			return 0;
		}

		public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Base baseObj)
		{
			int result = base.FromXmlTextReader(ref reader, flags, baseObj);
			Library library = (Library)baseObj;
			if (library == null)
			{
				return -1;
			}
			string text = "";
			text = reader.GetAttribute("Type");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "extern":
					library.propType = LibraryType.Extern;
					break;
				case "intern":
					library.propType = LibraryType.Intern;
					break;
				}
			}
			reader.Read();
			if (reader.Name == "Functions")
			{
				while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
				{
					string text2 = "";
					byte result2 = 0;
					text = "";
					text = reader.GetAttribute("Name");
					if (text != null && text.Length > 0)
					{
						text2 = text;
					}
					text = "";
					text = reader.GetAttribute("Reference");
					if (text != null && text.Length > 0)
					{
						PviParse.TryParseByte(text, out result2);
					}
					if (text2 != "")
					{
						new Function(library, text2);
					}
				}
			}
			reader.Read();
			if (reader.Name == "Library" && reader.NodeType == XmlNodeType.EndElement)
			{
				reader.Read();
			}
			return result;
		}
	}
}
