using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace BR.AN.PviServices
{
    /// <summary>
    /// –Ú¡–ªØ¿©’π
    /// </summary>
	public static class SerializationExtensions
	{
		private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		public static string GetXmlRootAttributeNameSpace(this Type instance)
		{
			CustomAttributeData customAttributeData = instance.GetCustomAttributesData().FirstOrDefault((CustomAttributeData a) => a.AttributeType == typeof(XmlRootAttribute));
			if (customAttributeData != null && customAttributeData.NamedArguments != null)
			{
				CustomAttributeNamedArgument customAttributeNamedArgument = customAttributeData.NamedArguments.FirstOrDefault(delegate(CustomAttributeNamedArgument a)
				{
					if (a.MemberName == "Namespace")
					{
						return a.TypedValue.Value != null;
					}
					return false;
				});
				if (string.IsNullOrEmpty(customAttributeNamedArgument.TypedValue.Value.ToString()))
				{
					return null;
				}
				return customAttributeNamedArgument.TypedValue.Value.ToString();
			}
			return null;
		}

		public static string GetXmlTypeAttributeNameSpace(this Type instance)
		{
			CustomAttributeData customAttributeData = instance.GetCustomAttributesData().FirstOrDefault((CustomAttributeData a) => a.AttributeType == typeof(XmlTypeAttribute));
			if (customAttributeData != null && customAttributeData.NamedArguments != null)
			{
				CustomAttributeNamedArgument customAttributeNamedArgument = customAttributeData.NamedArguments.FirstOrDefault(delegate(CustomAttributeNamedArgument a)
				{
					if (a.MemberName == "Namespace")
					{
						return a.TypedValue.Value != null;
					}
					return false;
				});
				if (string.IsNullOrEmpty(customAttributeNamedArgument.TypedValue.Value.ToString()))
				{
					return null;
				}
				return customAttributeNamedArgument.TypedValue.Value.ToString();
			}
			return null;
		}

		public static string PrepareForDeserialize<T>(this string instance)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(instance);
				foreach (XmlElement item in xmlDocument.OfType<XmlElement>())
				{
					string xmlRootAttributeNameSpace = typeof(T).GetXmlRootAttributeNameSpace();
					if (string.IsNullOrEmpty(item.NamespaceURI) && xmlRootAttributeNameSpace != null)
					{
						item.SetAttribute("xmlns", xmlRootAttributeNameSpace);
					}
					string xmlTypeAttributeNameSpace = typeof(T).GetXmlTypeAttributeNameSpace();
					foreach (XmlElement item2 in item.ChildNodes.OfType<XmlElement>())
					{
						if (string.IsNullOrEmpty(item2.NamespaceURI) && xmlTypeAttributeNameSpace != null)
						{
							item2.SetAttribute("xmlns", xmlTypeAttributeNameSpace);
						}
					}
				}
				return xmlDocument.InnerXml;
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public static string ReplaceInvalidXmlChars(this string instance, string replaceChar = "?")
		{
			if (instance == null)
			{
				return null;
			}
			return Regex.Replace(instance, "[^\\x09\\x0A\\x0D\\x20-\\uD7FF\\uE000-\\uFFFD\\u10000-\\u10FFFF]", replaceChar);
		}

		public static void ReplaceStringPropertyInvalidXmlChars(this object instance, string replaceString = "?")
		{
			IEnumerable<PropertyInfo> enumerable = from prop in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
			where prop.MemberType == MemberTypes.Property
			select prop;
			foreach (PropertyInfo item in enumerable)
			{
				if (!item.GetCustomAttributes(typeof(XmlIgnoreAttribute), inherit: true).Any())
				{
					object value = item.GetValue(instance);
					if (value != null)
					{
						Array array = value as Array;
						if (array != null)
						{
							foreach (object item2 in array)
							{
								item2.ReplaceStringPropertyInvalidXmlChars(replaceString);
							}
						}
						else if (item.PropertyType.IsClass && item.PropertyType.HasElementType)
						{
							item.ReplaceStringPropertyInvalidXmlChars(replaceString);
						}
						string text = value as string;
						if (text != null)
						{
							text = text.ReplaceInvalidXmlChars(replaceString);
							item.SetValue(instance, text);
						}
					}
				}
			}
		}

		public static void SerializeToStream<T>(this T instance, Stream stream)
		{
			instance.ReplaceStringPropertyInvalidXmlChars();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			xmlSerializer.Serialize(stream, instance);
		}

		public static string SerializeToString<T>(this T instance)
		{
			try
			{
				instance.ReplaceStringPropertyInvalidXmlChars();
				using (StringWriter stringWriter = new StringWriter())
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					xmlSerializer.Serialize(stringWriter, instance);
					return stringWriter.ToString();
				}
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public static T ToObject<T>(this FileInfo fileInfo)
		{
			T result = default(T);
			if (fileInfo == null || !fileInfo.Exists)
			{
				return result;
			}
			using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
			{
				string xmlString = streamReader.ReadToEnd();
				return xmlString.ToObject<T>();
			}
		}

		public static T ToObject<T>(this string xmlString)
		{
			T result = default(T);
			string text = xmlString.PrepareForDeserialize<T>();
			if (text == null)
			{
				return result;
			}
			using (TextReader input = new StringReader(text))
			{
				using (XmlReader xmlReader = XmlReader.Create(input))
				{
					try
					{
						result = (T)new XmlSerializer(typeof(T), "").Deserialize(xmlReader);
						return result;
					}
					catch (InvalidOperationException)
					{
						return result;
					}
				}
			}
		}
	}
}
