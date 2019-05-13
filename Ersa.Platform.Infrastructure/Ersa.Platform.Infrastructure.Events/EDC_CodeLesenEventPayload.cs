using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle;

namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_CodeLesenEventPayload
	{
		public long PRO_i64ArrayIndex
		{
			get;
		}

		public ENUM_CodeLesenZustand PRO_enmZustand
		{
			get;
		}

		public ENUM_LsgVerwendung PRO_enmVerwendung
		{
			get;
		}

		public EDC_CodeLeseErgebnis PRO_edcLeseErgebnis
		{
			get;
		}

		public EDC_RuestErgebnis PRO_edcRuestErgebnis
		{
			get;
		}

		public EDC_CodeLesenEventPayload(long i_i64ArrayIndex, ENUM_CodeLesenZustand i_enmZustand, ENUM_LsgVerwendung i_enmVerwendung, EDC_CodeLeseErgebnis i_edcLeseErgebnis = null, EDC_RuestErgebnis i_edcRuestErgebnis = null)
		{
			PRO_i64ArrayIndex = i_i64ArrayIndex;
			PRO_enmZustand = i_enmZustand;
			PRO_enmVerwendung = i_enmVerwendung;
			PRO_edcLeseErgebnis = i_edcLeseErgebnis;
			PRO_edcRuestErgebnis = i_edcRuestErgebnis;
		}
	}
}
