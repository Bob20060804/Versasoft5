using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
	[Export(typeof(INF_SpsProvider))]
	public class EDC_SpsProvider : INF_SpsProvider
	{
		[ImportMany]
		public IEnumerable<Lazy<INF_Sps, INF_SpsMetadaten>> PRO_fdcSpsProvider
		{
			get;
			set;
		}

		public INF_Sps FUN_edcAktiveSps()
		{
			if (EDC_KommunikationsHelfer.PRO_strSpsTyp != "M1Com" && EDC_KommunikationsHelfer.PRO_strSpsTyp != "PviServices")
			{
                // ÎÞÐ§Öµ
				throw new InvalidDataException("Invalid value: EDC_KommunikationsHelfer.PRO_strSpsTyp=#" + EDC_KommunikationsHelfer.PRO_strSpsTyp + "#" + "\n expect #M1Com#" + "\n or     #PviServices#");
			}
			return PRO_fdcSpsProvider.First((Lazy<INF_Sps, INF_SpsMetadaten> i_fdcProvider) => i_fdcProvider.Metadata.PRO_strSpsTyp == EDC_KommunikationsHelfer.PRO_strSpsTyp).Value;
		}
	}
}
