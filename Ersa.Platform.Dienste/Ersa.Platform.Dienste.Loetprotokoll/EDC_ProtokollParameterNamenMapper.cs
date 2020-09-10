using Ersa.Platform.Common;
using Ersa.Platform.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Export]
	public class EDC_ProtokollParameterNamenMapper
	{
		private readonly INF_Logger m_edcLogger;

		private Dictionary<string, string> m_dicMapping;

		[ImportingConstructor]
		public EDC_ProtokollParameterNamenMapper(INF_Logger i_edcLogger)
		{
			m_edcLogger = i_edcLogger;
			m_dicMapping = new Dictionary<string, string>();
			SUB_Initialisiere();
		}

		public void SUB_Initialisiere(string i_strDateiPfad = null)
		{
			try
			{
				using (FileStream i_fdcInputStream = new FileStream(i_strDateiPfad ?? Path.Combine(EDC_VerzeichnisHelfer.FUN_strDefaultResourcenVerzeichnisErmitteln(), "ParameterMappings.xml"), FileMode.Open))
				{
					SUB_DeserialisiereAusStream(i_fdcInputStream);
				}
			}
			catch (FileNotFoundException)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Info, "Keine Parameter-Mapping Datei gefunden.", null, "EDC_ProtokollParameterNamenMapper");
			}
			catch (Exception i_excException)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Fehler beim Deserialisieren der Parametermapping-Datei", null, "EDC_ProtokollParameterNamenMapper", null, i_excException);
			}
		}

		public string FUN_strHoleMapping(string i_strParameterIdentifier, string i_strParameterName)
		{
			if (i_strParameterIdentifier == null)
			{
				return i_strParameterName;
			}
			if (!m_dicMapping.TryGetValue(i_strParameterIdentifier, out string value))
			{
				return i_strParameterName;
			}
			return value;
		}

		private void SUB_DeserialisiereAusStream(Stream i_fdcInputStream)
		{
			XmlSerializer xmlSerializer = FUN_fdcErstelleSerialisierer();
			m_dicMapping = ((EDC_ProtokollParameterNamenMapperItem[])xmlSerializer.Deserialize(i_fdcInputStream)).ToDictionary((EDC_ProtokollParameterNamenMapperItem i_edcMapperItem) => i_edcMapperItem.key, (EDC_ProtokollParameterNamenMapperItem i_edcMapperItem) => i_edcMapperItem.value);
		}

		private XmlSerializer FUN_fdcErstelleSerialisierer()
		{
			return new XmlSerializer(typeof(EDC_ProtokollParameterNamenMapperItem[]), new XmlRootAttribute
			{
				ElementName = "items"
			});
		}
	}
}
