using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
	public class LibraryCollection : BaseCollection
	{
		public override Service Service => ((Cpu)propParent).Service;

		public Library this[string name]
		{
			get
			{
				if (propCollectionType == CollectionType.HashTable)
				{
					return (Library)base[name];
				}
				return null;
			}
		}

		public LibraryCollection(Cpu cpu, string name)
			: base(cpu, name)
		{
			propCollectionType = CollectionType.HashTable;
		}

		public virtual void Add(Library library)
		{
			base.Add(library.Name, library);
		}

		public virtual void Upload()
		{
			string text = "LIB=* QU=0";
			Service.BuildRequestBuffer(text);
			PInvokePvicom.PviComReadArgumentRequest(Service, ((Cpu)propParent).LinkId, AccessTypes.LibraryList, Service.RequestBuffer, text.Length, 1202u, base.InternId);
		}

		public override void Connect()
		{
			Connect(ConnectionType.CreateAndLink);
		}

		public override void Connect(ConnectionType connectionType)
		{
			Fire_Error(this, new PviEventArgs(propName, "", 12058, Service.Language, Action.LibrariesConnect, Service));
			OnCollectionConnected(new CollectionEventArgs(propName, "", 12058, Service.Language, Action.LibrariesConnect, null));
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (PVIReadAccessTypes.LibraryList == accessType)
			{
				if (errorCode == 0 && dataLen != 0)
				{
					string text = "";
					text = PviMarshal.PtrToStringAnsi(pData, dataLen);
					string[] array = null;
					if (text != "")
					{
						array = text.Split("\t".ToCharArray());
						for (int i = 0; i < array.Length; i++)
						{
							Library library = null;
							string text2 = array[i].Substring(0, array[i].IndexOf(" "));
							if (!((Cpu)propParent).Libraries.ContainsKey(text2))
							{
								library = new Library((Cpu)propParent, text2);
								int num = array[i].IndexOf("TYP=");
								if (-1 != num)
								{
									library.propType = (LibraryType)Convert.ToByte(array[i].Substring(num + 4));
								}
							}
						}
					}
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.LibraryUpload, Service));
				}
				else
				{
					OnUploaded(new PviEventArgs(propName, "", errorCode, Service.Language, Action.LibraryUpload, Service));
					OnError(new PviEventArgs(propName, "", errorCode, Service.Language, Action.LibraryUpload, Service));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		protected virtual void OnError(PviEventArgs e)
		{
			Fire_Error(this, e);
		}

		internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			int result = 0;
			int num = 0;
			if (Count > 0)
			{
				writer.WriteStartElement(GetType().Name);
				foreach (object value in Values)
				{
					writer.WriteStartElement("Library");
					num = ((Library)value).ToXMLTextWriter(ref writer, flags);
					if (num != 0)
					{
						result = num;
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			return result;
		}

		internal override void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				object propParent = base.propParent;
				object propUserData = base.propUserData;
				string propName = base.propName;
				CleanUp(disposing);
				base.propParent = propParent;
				base.propUserData = propUserData;
				base.propName = propName;
				base.Dispose(disposing, removeFromCollection);
				base.propParent = null;
				base.propUserData = null;
				base.propName = null;
			}
		}

		internal void CleanUp(bool disposing)
		{
			ArrayList arrayList = new ArrayList();
			propCounter = 0;
			if (Values != null)
			{
				foreach (Library value in Values)
				{
					arrayList.Add(value);
					if (value.LinkId != 0)
					{
						value.Disconnect(0);
					}
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				object obj = arrayList[i];
				((Library)obj).Dispose(disposing, removeFromCollection: true);
				obj = null;
			}
			Clear();
		}
	}
}
