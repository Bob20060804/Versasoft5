using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Helfer
{
	public static class EDC_RechteHelfer
	{
		private static readonly IDictionary<int, string> ms_dicLegacyRechteMappings = new Dictionary<int, string>
		{
			{
				1024,
				"BerechtigungLoetprogrammEdit"
			},
			{
				1,
				"BerechtigungAdministration"
			},
			{
				16,
				"BerechtigungLoetprogramm"
			},
			{
				8,
				"BerechtigungBedienen"
			},
			{
				4,
				"BerechtigungMaschineEinrichten"
			},
			{
				2,
				"BerechtigungMaschinenkonfiguration"
			},
			{
				32,
				"BerechtigungMeldungenQuittieren"
			},
			{
				64,
				"BerechtigungMeldungsadministration"
			},
			{
				128,
				"BerechtigungZeitschaltuhr"
			},
			{
				256,
				"BerechtigungZyklischeMeldungenQuittieren"
			},
			{
				512,
				"BerechtigungProduktionSteuern"
			},
			{
				2048,
				"BerechtigungMaschineEinrichtenExperte"
			},
			{
				4096,
				"BerechtigungLoetprogrammQs"
			}
		};

		private static readonly IDictionary<string, int> ms_dicNeueRechteMappings = new Dictionary<string, int>
		{
			{
				"BerechtigungLoetprogrammEdit",
				1024
			},
			{
				"BerechtigungLoetprogramm",
				16
			},
			{
				"BerechtigungBedienen",
				8
			},
			{
				"BerechtigungMaschineEinrichten",
				4
			},
			{
				"BerechtigungMaschinenkonfiguration",
				2
			},
			{
				"BerechtigungMeldungenQuittieren",
				32
			},
			{
				"BerechtigungMeldungsadministration",
				64
			},
			{
				"BerechtigungZeitschaltuhr",
				128
			},
			{
				"BerechtigungZyklischeMeldungenQuittieren",
				256
			},
			{
				"BerechtigungProduktionSteuern",
				512
			},
			{
				"BerechtigungMaschineEinrichtenExperte",
				2048
			},
			{
				"BerechtigungLoetprogrammQs",
				4096
			}
		};

		public static IEnumerable<string> FUN_enuLegacyRechteKonvertieren(int i_i32Berechtigungen)
		{
			if ((i_i32Berechtigungen & 1) == 1)
			{
				return (from i32Recht in ms_dicLegacyRechteMappings.Keys
				select ms_dicLegacyRechteMappings[i32Recht]).ToList();
			}
			return (from i32Recht in ms_dicLegacyRechteMappings.Keys
			where (i32Recht & i_i32Berechtigungen) > 0
			select ms_dicLegacyRechteMappings[i32Recht]).ToList();
		}

		public static int FUN_i32NeueRechteKonvertieren(bool i_blnIstAdministrator, IEnumerable<string> i_lstBerechtigungen)
		{
			if (i_lstBerechtigungen == null)
			{
				return 0;
			}
			int num = (from i_strRecht in i_lstBerechtigungen.ToList()
			where ms_dicNeueRechteMappings.ContainsKey(i_strRecht)
			select i_strRecht).Aggregate(0, (int i_i32Current, string i_strRecht) => i_i32Current | ms_dicNeueRechteMappings[i_strRecht]);
			if (i_blnIstAdministrator)
			{
				num |= 1;
			}
			return num;
		}
	}
}
