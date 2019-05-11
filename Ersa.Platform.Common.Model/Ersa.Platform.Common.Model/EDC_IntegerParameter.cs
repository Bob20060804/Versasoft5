using Ersa.Platform.Infrastructure;
using System;
using System.ComponentModel;

namespace Ersa.Platform.Common.Model
{
	public class EDC_IntegerParameter : EDC_PrimitivParameter, IDataErrorInfo
	{
		private Func<int?> m_delMaximalWertErmittler;

		private Func<int?> m_delMinimalWertErmittler;

		public Func<int?> PRO_delMaximalWertErmittler
		{
			private get
			{
				return m_delMaximalWertErmittler ?? ((Func<int?>)(() => int.MaxValue));
			}
			set
			{
				m_delMaximalWertErmittler = value;
				RaisePropertyChanged("PRO_intMaximalWert");
			}
		}

		public Func<int?> PRO_delMinimalWertErmittler
		{
			private get
			{
				return m_delMinimalWertErmittler ?? ((Func<int?>)(() => int.MinValue));
			}
			set
			{
				m_delMinimalWertErmittler = value;
				RaisePropertyChanged("PRO_intMinimalWert");
			}
		}

		public int PRO_intMaximalWert => PRO_delMaximalWertErmittler() ?? int.MaxValue;

		public int PRO_intMinimalWert => PRO_delMinimalWertErmittler() ?? int.MinValue;

		public override object PRO_objValue
		{
			get
			{
				return base.PRO_objValue;
			}
			set
			{
				int? num = EDC_WertKonvertierung.FUN_intWertUmwandeln(value);
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

		public int? PRO_intWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_intWertUmwandeln(PRO_objValue);
			}
			set
			{
				if (PRO_intWert != value)
				{
					PRO_objValue = value;
				}
			}
		}

		public int? PRO_intAnzeigeWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_intWertUmwandeln(PRO_objAnzeigeWert);
			}
			set
			{
				if (PRO_intAnzeigeWert != value)
				{
					PRO_objAnzeigeWert = value;
				}
			}
		}

		public string Error => string.Empty;

		public bool PRO_blnIsValide => string.IsNullOrEmpty(this["PRO_intAnzeigeWert"]);

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

		private string FUN_strValidiereWertGegenRange(int? i_i32Wert)
		{
			bool num = i_i32Wert < PRO_intMinimalWert;
			bool flag = i_i32Wert > PRO_intMaximalWert;
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
