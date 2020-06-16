using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ersa.Platform.Common
{
    /// <summary>
    /// ×ª»»Öµ
    /// Convert Value
    /// </summary>
	public static class EDC_WertKonvertierung
	{
		private static readonly IDictionary<Type, Func<object, float>> ms_dicSngKonvertierungen;

		private static readonly IDictionary<Type, Func<object, int>> ms_dicIntKonvertierungen;

		private static readonly IDictionary<Type, Func<object, uint>> ms_dicU32Konvertierungen;

		private static readonly IDictionary<Type, Func<object, bool>> ms_dicBlnKonvertierungen;

		static EDC_WertKonvertierung()
		{
			ms_dicSngKonvertierungen = new Dictionary<Type, Func<object, float>>
			{
				{
					typeof(short),
					(object l_objWert) => (short)l_objWert
				},
				{
					typeof(ushort),
					(object l_objWert) => (int)(ushort)l_objWert
				},
				{
					typeof(int),
					(object l_objWert) => (int)l_objWert
				},
				{
					typeof(long),
					(object l_objWert) => (long)l_objWert
				},
				{
					typeof(float),
					(object l_objWert) => (float)l_objWert
				},
				{
					typeof(double),
					Convert.ToSingle
				},
				{
					typeof(string),
					(object i_strWert) => Convert.ToSingle(i_strWert, CultureInfo.InvariantCulture)
				}
			};
			ms_dicIntKonvertierungen = new Dictionary<Type, Func<object, int>>
			{
				{
					typeof(byte),
					(object l_objWert) => (byte)l_objWert
				},
				{
					typeof(short),
					(object l_objWert) => (short)l_objWert
				},
				{
					typeof(ushort),
					(object l_objWert) => (ushort)l_objWert
				},
				{
					typeof(int),
					(object l_objWert) => (int)l_objWert
				},
				{
					typeof(uint),
					Convert.ToInt32
				},
				{
					typeof(long),
					Convert.ToInt32
				},
				{
					typeof(string),
					Convert.ToInt32
				}
			};
			ms_dicU32Konvertierungen = new Dictionary<Type, Func<object, uint>>
			{
				{
					typeof(byte),
					(object l_objWert) => (byte)l_objWert
				},
				{
					typeof(short),
					(object l_objWert) => (ushort)l_objWert
				},
				{
					typeof(ushort),
					(object l_objWert) => (ushort)l_objWert
				},
				{
					typeof(int),
					Convert.ToUInt32
				},
				{
					typeof(uint),
					Convert.ToUInt32
				},
				{
					typeof(long),
					Convert.ToUInt32
				},
				{
					typeof(string),
					Convert.ToUInt32
				}
			};
			ms_dicBlnKonvertierungen = new Dictionary<Type, Func<object, bool>>
			{
				{
					typeof(bool),
					Convert.ToBoolean
				},
				{
					typeof(string),
					(object l_objWert) => (string)l_objWert != "0"
				}
			};
		}

		public static float? FUN_sngWertUmwandeln(object i_objWert, float i_sngFaktor = 1f, int? i_i32NachkommaStellen = default(int?))
		{
			if (i_objWert != null)
			{
				Type type = i_objWert.GetType();
				if (ms_dicSngKonvertierungen.TryGetValue(type, out Func<object, float> value))
				{
					if (type == typeof(string))
					{
						string text = Convert.ToString(i_objWert);
						text = text.Replace(',', '.');
						return FUN_sngWertFaktorisieren(value(text), i_sngFaktor, i_i32NachkommaStellen);
					}
					return FUN_sngWertFaktorisieren(value(i_objWert), i_sngFaktor, i_i32NachkommaStellen);
				}
			}
			return null;
		}

		public static float? FUN_sngWertFaktorisieren(float? i_sngWert, float i_sngFaktor, int? i_i32NachkommaStellen = default(int?))
		{
			if (i_sngWert.HasValue)
			{
				float num = i_sngWert.Value * i_sngFaktor;
				if (i_i32NachkommaStellen.HasValue && i_i32NachkommaStellen >= 0)
				{
					return (float)Math.Round(num, i_i32NachkommaStellen.Value);
				}
				return num;
			}
			return null;
		}

		public static int? FUN_intWertFaktorisieren(int? i_intWert, float i_sngFaktor)
		{
			if (i_intWert.HasValue)
			{
				return (int)((float)i_intWert.Value * i_sngFaktor);
			}
			return null;
		}

		public static uint? FUN_u32WertFaktorisieren(uint? i_intWert, float i_sngFaktor)
		{
			if (i_intWert.HasValue)
			{
				return (uint)((decimal)i_intWert.Value * (decimal)i_sngFaktor);
			}
			return null;
		}

		public static int? FUN_intWertUmwandeln(object i_objWert, float i_sngFaktor = 1f)
		{
			if (i_objWert != null)
			{
				Type type = i_objWert.GetType();
				if (ms_dicIntKonvertierungen.TryGetValue(type, out Func<object, int> value))
				{
					return FUN_intWertFaktorisieren(value(i_objWert), i_sngFaktor);
				}
			}
			return null;
		}

		public static uint? FUN_u32WertUmwandeln(object i_objWert, float i_sngFaktor = 1f)
		{
			if (i_objWert != null)
			{
				Type type = i_objWert.GetType();
				if (ms_dicU32Konvertierungen.TryGetValue(type, out Func<object, uint> value))
				{
					return FUN_u32WertFaktorisieren(value(i_objWert), i_sngFaktor);
				}
			}
			return null;
		}

		public static bool? FUN_blnWertUmwandeln(object i_objWert)
		{
			if (i_objWert != null)
			{
				Type type = i_objWert.GetType();
				if (ms_dicBlnKonvertierungen.TryGetValue(type, out Func<object, bool> value))
				{
					return value(i_objWert);
				}
			}
			return null;
		}
	}
}
