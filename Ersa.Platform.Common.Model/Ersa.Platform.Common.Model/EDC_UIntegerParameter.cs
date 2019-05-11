using Ersa.Platform.Infrastructure;
using System;
using System.ComponentModel;

namespace Ersa.Platform.Common.Model
{
	public class EDC_UIntegerParameter : EDC_PrimitivParameter, IDataErrorInfo
	{
		private Func<uint?> m_delMaximalWertErmittler;

		private Func<uint?> m_delMinimalWertErmittler;

		public Func<uint?> PRO_delMaximalWertErmittler
		{
			private get
			{
				return m_delMaximalWertErmittler ?? ((Func<uint?>)(() => uint.MaxValue));
			}
			set
			{
				m_delMaximalWertErmittler = value;
				RaisePropertyChanged("PRO_intMaximalWert");
			}
		}

		public Func<uint?> PRO_delMinimalWertErmittler
		{
			private get
			{
				return m_delMinimalWertErmittler ?? ((Func<uint?>)(() => 0u));
			}
			set
			{
				m_delMinimalWertErmittler = value;
				RaisePropertyChanged("PRO_intMinimalWert");
			}
		}

		public uint PRO_intMaximalWert => (uint)(((int?)PRO_delMaximalWertErmittler()) ?? (-1));

		public uint PRO_intMinimalWert => PRO_delMinimalWertErmittler() ?? 0;

		public override object PRO_objValue
		{
			get
			{
				return base.PRO_objValue;
			}
			set
			{
				uint? num = EDC_WertKonvertierung.FUN_u32WertUmwandeln(value);
				if (num != PRO_intWert)
				{
					base.PRO_objValue = num;
					EDC_Dispatch.SUB_AktionStarten(delegate
					{
						RaisePropertyChanged("PRO_intWert");
						RaisePropertyChanged("PRO_intAnzeigeWert");
					});
				}
			}
		}

		public override object PRO_objAnzeigeWert
		{
			get
			{
				return base.PRO_objAnzeigeWert;
			}
			set
			{
				base.PRO_objAnzeigeWert = value;
				RaisePropertyChanged("PRO_intAnzeigeWert");
			}
		}

		public override bool PRO_blnHatAenderung => PRO_intWert != PRO_intAnzeigeWert;

		public uint? PRO_intWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_u32WertUmwandeln(PRO_objValue);
			}
			set
			{
				if (PRO_intWert != value)
				{
					PRO_objValue = value;
				}
			}
		}

		public uint? PRO_intAnzeigeWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_u32WertUmwandeln(PRO_objAnzeigeWert);
			}
			set
			{
				if (PRO_intAnzeigeWert != value)
				{
					PRO_objAnzeigeWert = value;
				}
			}
		}

		public bool PRO_blnIsValide => string.IsNullOrEmpty(this["PRO_intAnzeigeWert"]);

		public string Error => string.Empty;

		public string this[string i_strPropertyName]
		{
			get
			{
				if (i_strPropertyName == "PRO_intAnzeigeWert")
				{
					return FUN_strValidiereWertGegenRange(PRO_intAnzeigeWert);
				}
				if (i_strPropertyName == "PRO_intWert")
				{
					return FUN_strValidiereWertGegenRange(PRO_intWert);
				}
				return string.Empty;
			}
		}

		public void SUB_RangeAenderungenSignalisieren()
		{
			RaisePropertyChanged("PRO_intMaximalWert");
			RaisePropertyChanged("PRO_intMinimalWert");
			RaisePropertyChanged("PRO_intAnzeigeWert");
			RaisePropertyChanged("PRO_intWert");
		}

		private string FUN_strValidiereWertGegenRange(uint? i_u32Wert)
		{
			bool num = i_u32Wert < PRO_intMinimalWert;
			bool flag = i_u32Wert > PRO_intMaximalWert;
			if (!num)
			{
				if (!flag)
				{
					return string.Empty;
				}
				return "4_9180";
			}
			return "4_3014";
		}
	}
}
