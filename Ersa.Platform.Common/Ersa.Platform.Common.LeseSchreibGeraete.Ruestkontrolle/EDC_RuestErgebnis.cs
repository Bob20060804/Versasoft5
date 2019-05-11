using System;

namespace Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle
{
	public class EDC_RuestErgebnis : EDC_LeseErgebnis
	{
		public ENUM_RuestWerkzeug PRO_enmWerkzeug
		{
			get;
		}

		public ENUM_LeseFehler PRO_enmFehler
		{
			get;
			private set;
		}

		public bool PRO_blnIstFehlerhaft => PRO_enmFehler != ENUM_LeseFehler.KeinFehler;

		public string PRO_strRuestDaten
		{
			get;
			private set;
		}

		public EDC_RuestErgebnis(long i_i64ArrayIndex, ENUM_RuestWerkzeug i_enmWerkzeug, ENUM_LeseFehler i_enmFehler, Exception i_fdcException)
			: this(i_i64ArrayIndex, i_enmWerkzeug)
		{
			PRO_strRuestDaten = null;
			PRO_enmFehler = i_enmFehler;
			base.PRO_fdcException = i_fdcException;
		}

		public EDC_RuestErgebnis(long i_i64ArrayIndex, ENUM_RuestWerkzeug i_enmWerkzeug, string i_strRuestDaten)
			: this(i_i64ArrayIndex, i_enmWerkzeug)
		{
			PRO_strRuestDaten = i_strRuestDaten;
			PRO_enmFehler = ENUM_LeseFehler.KeinFehler;
			base.PRO_fdcException = null;
		}

		private EDC_RuestErgebnis(long i_i64ArrayIndex, ENUM_RuestWerkzeug i_enmWerkzeug)
			: base(i_i64ArrayIndex)
		{
			PRO_enmWerkzeug = i_enmWerkzeug;
		}

		public EDC_RuestErgebnis FUN_edcKopieErstellen()
		{
			return new EDC_RuestErgebnis(base.PRO_i64ArrayIndex, PRO_enmWerkzeug)
			{
				PRO_strRuestDaten = PRO_strRuestDaten,
				PRO_enmFehler = PRO_enmFehler,
				PRO_fdcException = base.PRO_fdcException,
				PRO_enmPipelineState = base.PRO_enmPipelineState,
				PRO_fdcTimeStampSignalisiert = base.PRO_fdcTimeStampSignalisiert
			};
		}
	}
}
