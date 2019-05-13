using Ersa.Platform.Logging;
using System;

namespace Ersa.Platform.Infrastructure.MethodenUeberwachung
{
	[Obsolete("Die Methodenüberwachung wird zum Jaheswechsel 2017/2018 entfallen. Bitte die Projekte entsprechend davon 'befreien'")]
	public class EDC_MethodenUeberwachungAttribute : Attribute
	{
		public EDC_MethodenUeberwachungAttribute(ENUM_LogLevel i_edcLoggingLevel, bool i_blnArgumenteNichtLoggen = false, bool i_blnFehlerBehandlen = false, bool i_blnNichtLoggen = false)
		{
		}
	}
}
