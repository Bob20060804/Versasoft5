using Ersa.Global.Dienste.Exceptions;
using Ersa.Global.Dienste.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ersa.Global.Dienste
{
	[Export("Ersa.XmlSerialisierer", typeof(INF_SerialisierungsDienst))]
	public class EDC_XmlSerialisierungsDienst : INF_SerialisierungsDienst
	{
		public string FUN_strSerialisieren<T>(T i_objObjekt)
		{
			try
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					XmlWriterSettings settings = new XmlWriterSettings
					{
						Indent = true,
						NewLineOnAttributes = false
					};
					using (XmlWriter i_fdcWriter = XmlWriter.Create(stringWriter, settings))
					{
						SUB_Serialisieren(i_objObjekt, i_fdcWriter);
						return stringWriter.ToString().Replace("utf-16", "utf-8");
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error serializing object!", i_fdcInner);
			}
		}

		public void SUB_ValidiereGegenSchemaDatei(string i_strInhalt, string i_strSchemaDatei)
		{
			StringBuilder fdcValidierungsFehlerBuilder = new StringBuilder();
			StringBuilder fdcValidierungsWarnungenBuilder = new StringBuilder();
			try
			{
				XDocument xDocument = XDocument.Parse(i_strInhalt);
				XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
				xmlSchemaSet.Add(null, i_strSchemaDatei);
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
				{
					ValidationType = ValidationType.Schema
				};
				xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
				xmlReaderSettings.Schemas = xmlSchemaSet;
				xmlReaderSettings.ValidationEventHandler += delegate(object i_objSender, ValidationEventArgs i_objValidationEventArgs)
				{
					string value = $"{i_objValidationEventArgs.Severity}: {i_objValidationEventArgs.Message}";
					if (XmlSeverityType.Error.Equals(i_objValidationEventArgs.Severity))
					{
						fdcValidierungsFehlerBuilder.Append(value).AppendLine();
					}
					if (XmlSeverityType.Warning.Equals(i_objValidationEventArgs.Severity))
					{
						fdcValidierungsWarnungenBuilder.Append(value).AppendLine();
					}
				};
				using (XmlReader xmlReader = XmlReader.Create(xDocument.CreateReader(), xmlReaderSettings))
				{
					while (xmlReader.Read())
					{
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_ValidierungsException("Error validating object!", i_fdcInner);
			}
			string text = fdcValidierungsFehlerBuilder.ToString();
			string i_strWarnungenText = fdcValidierungsWarnungenBuilder.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				throw new EDC_ValidierungsException(text, i_strWarnungenText, ENU_ValidierungsStatus.enmValidierungMitFehler);
			}
			if (!string.IsNullOrEmpty(text))
			{
				throw new EDC_ValidierungsException(text, i_strWarnungenText, ENU_ValidierungsStatus.enmValidierungMitWarnung);
			}
		}

		public byte[] FUNa_bytSerialisieren<T>(T i_objObjekt)
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (XmlWriter i_fdcWriter = XmlWriter.Create(memoryStream))
					{
						SUB_Serialisieren(i_objObjekt, i_fdcWriter);
						return memoryStream.ToArray();
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error serializing object!", i_fdcInner);
			}
		}

		public T FUN_objDeserialisieren<T>(string i_strInhalt)
		{
			try
			{
				using (StringReader i_fdcReader = new StringReader(i_strInhalt))
				{
					return FUN_objDeserialisieren<T>(i_fdcReader);
				}
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
					using (StreamReader i_fdcReader = new StreamReader(stream))
					{
						return FUN_objDeserialisieren<T>(i_fdcReader);
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_SerialisierungsException("Error deserializing object!", i_fdcInner);
			}
		}

		private T FUN_objDeserialisieren<T>(TextReader i_fdcReader)
		{
			return (T)new XmlSerializer(typeof(T)).Deserialize(i_fdcReader);
		}

		private void SUB_Serialisieren<T>(T i_objObjekt, XmlWriter i_fdcWriter)
		{
			new XmlSerializer(typeof(T)).Serialize(i_fdcWriter, i_objObjekt);
		}
	}
}
