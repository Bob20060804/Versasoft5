using Ersa.Platform.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
    /// <summary>
    /// Comnunication Helper
    /// </summary>
	public static class EDC_KommunikationsHelfer
	{
		private static readonly IDictionary<ENUM_ParameterTypen, float> ms_dicFaktoren = new Dictionary<ENUM_ParameterTypen, float>
		{
			{
				ENUM_ParameterTypen.enmTransportGeschwindigkeitInProzent,
				0.003052503f
			},
			{
				ENUM_ParameterTypen.enmZehntel,
				0.1f
			}
		};

		private static readonly IList<Func<string, ENUM_SpsTyp>> ms_lstParameterErmittlungsRegeln = new List<Func<string, ENUM_SpsTyp>>
		{
			FUN_enmErmittelParameterTypMitMehrerenUnterstrichenAmEnde,
			FUN_enmErmittelParameterTypMitMehrerenUnterstrichenAmAnfang
		};

		private static byte[] msa_bytIp = new byte[4];

		public static string PRO_strSpsIpAdresse => ConfigurationManager.AppSettings["SpsIpAdresse"];

		public static string PRO_strSpsTyp => ConfigurationManager.AppSettings["SpsTyp"];

		public static string PRO_strSpsFtpPartition => ConfigurationManager.AppSettings["SpsFtpPartition"];

		[Export("Ersa.Platform.Plc.KommunikationsDienst.EDC_KommunikationsHelfer.FUNa_bytIpAdressenFeststellen")]
		public static byte[] FUNa_bytIpAdressenFeststellen()
		{
			if (msa_bytIp[0] != 0)
			{
				return msa_bytIp;
			}
			IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			IPAddress iPAddress = new IPAddress(msa_bytIp);
			IPAddress[] array = hostAddresses;
			foreach (IPAddress iPAddress2 in array)
			{
				if (iPAddress2.GetAddressBytes().Length == 4)
				{
					iPAddress = iPAddress2;
					break;
				}
			}
			msa_bytIp = iPAddress.GetAddressBytes();
			return msa_bytIp;
		}

		public static float FUN_sngErmittelFaktor(EDC_PrimitivParameter i_edcParameter)
		{
			if (!ms_dicFaktoren.ContainsKey(i_edcParameter.PRO_enmParameterTyp))
			{
				return 1f;
			}
			return ms_dicFaktoren[i_edcParameter.PRO_enmParameterTyp];
		}

		public static ENUM_SpsTyp FUN_enmErmittelParameterTyp(this string i_strPhysischeAdresse)
		{
			ENUM_SpsTyp eNUM_SpsTyp = FUN_enmErmittelParameterTypDefault(i_strPhysischeAdresse);
			if (eNUM_SpsTyp != ENUM_SpsTyp.enmUndefined)
			{
				return eNUM_SpsTyp;
			}
			foreach (Func<string, ENUM_SpsTyp> item in ms_lstParameterErmittlungsRegeln)
			{
				eNUM_SpsTyp = item(i_strPhysischeAdresse);
				if (eNUM_SpsTyp != ENUM_SpsTyp.enmUndefined)
				{
					return eNUM_SpsTyp;
				}
			}
			throw new Exception($"EDC_KommunikationsHelfer.FUN_enmErmittelParameterTyp(): Format ist nicht bekannt fuer Adresse: {i_strPhysischeAdresse}");
		}

		private static ENUM_SpsTyp FUN_enmErmittelParameterTypDefault(string i_strPhysischeAdresse)
		{
			return FUN_enmErmittelParameterTypMitPunktAmEnde(i_strPhysischeAdresse);
		}

		private static ENUM_SpsTyp FUN_enmErmittelParameterTypMitPunktAmEnde(string i_strPhysischeAdresse)
		{
			int num = i_strPhysischeAdresse.LastIndexOf('.');
			if (num < 0)
			{
				return ENUM_SpsTyp.enmUndefined;
			}
			string text = i_strPhysischeAdresse.Substring(num + 1);
			if (string.IsNullOrWhiteSpace(text))
			{
				return ENUM_SpsTyp.enmUndefined;
			}
			if (text.Length < 3)
			{
				if (text == "X")
				{
					return ENUM_SpsTyp.enmSingle;
				}
				if (text == "Y")
				{
					return ENUM_SpsTyp.enmSingle;
				}
				if (text == "Z")
				{
					return ENUM_SpsTyp.enmSingle;
				}
				return ENUM_SpsTyp.enmUndefined;
			}
			return FUN_enmErmittelTyp(text.Substring(0, 3));
		}

		private static ENUM_SpsTyp FUN_enmErmittelParameterTypMitMehrerenUnterstrichenAmEnde(string i_strPhysischeAdresse)
		{
			return FUN_enmErmittelParameterTypMitRegex(i_strPhysischeAdresse, "([^_]*)$");
		}

		private static ENUM_SpsTyp FUN_enmErmittelParameterTypMitMehrerenUnterstrichenAmAnfang(string i_strPhysischeAdresse)
		{
			return FUN_enmErmittelParameterTypMitRegex(i_strPhysischeAdresse, "(?<=_)(.*?)(?=_)");
		}

		private static ENUM_SpsTyp FUN_enmErmittelParameterTypMitRegex(string i_strPhysischeAdresse, string i_strPattern)
		{
			string value = Regex.Match(i_strPhysischeAdresse, i_strPattern, RegexOptions.Compiled).Value;
			if (string.IsNullOrWhiteSpace(value))
			{
				return ENUM_SpsTyp.enmUndefined;
			}
			if (value.Length < 3)
			{
				if (value == "X")
				{
					return ENUM_SpsTyp.enmSingle;
				}
				if (value == "Y")
				{
					return ENUM_SpsTyp.enmSingle;
				}
				if (value == "Z")
				{
					return ENUM_SpsTyp.enmSingle;
				}
				return ENUM_SpsTyp.enmUndefined;
			}
			return FUN_enmErmittelTyp(value.Substring(0, 3));
		}

		private static ENUM_SpsTyp FUN_enmErmittelTyp(string i_strTyp)
		{
			if (i_strTyp == "bln")
			{
				return ENUM_SpsTyp.enmBool;
			}
			if (i_strTyp == "int")
			{
				return ENUM_SpsTyp.enmInt16;
			}
			if (i_strTyp == "wrd")
			{
				return ENUM_SpsTyp.enmUInt16;
			}
			if (i_strTyp == "lng")
			{
				return ENUM_SpsTyp.enmInt32;
			}
			if (i_strTyp == "dwd")
			{
				return ENUM_SpsTyp.enmUInt32;
			}
			if (i_strTyp == "byt")
			{
				return ENUM_SpsTyp.enmByte;
			}
			if (i_strTyp == "sng")
			{
				return ENUM_SpsTyp.enmSingle;
			}
			if (i_strTyp == "stf")
			{
				return ENUM_SpsTyp.enmString;
			}
			if (i_strTyp == "enm")
			{
				return ENUM_SpsTyp.enmInt16;
			}
			return ENUM_SpsTyp.enmUndefined;
		}
	}
}
