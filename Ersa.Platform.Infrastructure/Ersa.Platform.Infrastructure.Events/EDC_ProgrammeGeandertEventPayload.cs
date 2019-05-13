using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Infrastructure.Events
{
	public class EDC_ProgrammeGeandertEventPayload
	{
		public string PRO_strAenderungsQuelle
		{
			get;
			private set;
		}

		public IEnumerable<long> PRO_enuGeaenderteProgramme
		{
			get;
			set;
		}

		public IEnumerable<long> PRO_enuGeloeschteProgramme
		{
			get;
			set;
		}

		public IEnumerable<long> PRO_enuHinzugefuegteProgramme
		{
			get;
			set;
		}

		public EDC_ProgrammeGeandertEventPayload(string i_strAenderungsQuelle)
		{
			PRO_strAenderungsQuelle = i_strAenderungsQuelle;
			PRO_enuGeaenderteProgramme = Enumerable.Empty<long>();
			PRO_enuGeloeschteProgramme = Enumerable.Empty<long>();
			PRO_enuHinzugefuegteProgramme = Enumerable.Empty<long>();
		}
	}
}
