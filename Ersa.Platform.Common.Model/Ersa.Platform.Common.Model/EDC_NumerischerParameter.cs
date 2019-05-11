using Ersa.Platform.Infrastructure;
using System;
using System.ComponentModel;

namespace Ersa.Platform.Common.Model
{
	public class EDC_NumerischerParameter : EDC_PrimitivParameter, IDataErrorInfo
	{
		private Func<float?> m_delMaximalWertErmittler;

		private Func<float?> m_delMinimalWertErmittler;

		public Func<float?> PRO_delMaximalWertErmittler
		{
			private get
			{
				return m_delMaximalWertErmittler ?? ((Func<float?>)(() => 3.40282347E+38f));
			}
			set
			{
				m_delMaximalWertErmittler = value;
				RaisePropertyChanged("PRO_sngMaximalWert");
			}
		}

		public Func<float?> PRO_delMinimalWertErmittler
		{
			private get
			{
				return m_delMinimalWertErmittler ?? ((Func<float?>)(() => -3.40282347E+38f));
			}
			set
			{
				m_delMinimalWertErmittler = value;
				RaisePropertyChanged("PRO_sngMinimalWert");
			}
		}

		public float PRO_sngMaximalWert => PRO_delMaximalWertErmittler() ?? 3.40282347E+38f;

		public float PRO_sngMinimalWert => PRO_delMinimalWertErmittler() ?? (-3.40282347E+38f);

		public int PRO_int32Nachkommastellen
		{
			get;
			set;
		}

		public override object PRO_objValue
		{
			get
			{
				return base.PRO_objValue;
			}
			set
			{
				base.PRO_objValue = value;
				EDC_Dispatch.SUB_AktionStarten(delegate
				{
					RaisePropertyChanged("PRO_sngWert");
					RaisePropertyChanged("PRO_sngAnzeigeWert");
				});
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
				RaisePropertyChanged("PRO_sngAnzeigeWert");
			}
		}

		public override bool PRO_blnHatAenderung
		{
			get
			{
				if (float.IsNaN(PRO_sngWert.GetValueOrDefault()) || float.IsNaN(PRO_sngAnzeigeWert.GetValueOrDefault()))
				{
					if (float.IsNaN(PRO_sngWert.GetValueOrDefault()) && float.IsNaN(PRO_sngAnzeigeWert.GetValueOrDefault()))
					{
						return false;
					}
					return true;
				}
				if (float.IsInfinity(PRO_sngWert.GetValueOrDefault()) || float.IsInfinity(PRO_sngAnzeigeWert.GetValueOrDefault()))
				{
					if (float.IsInfinity(PRO_sngWert.GetValueOrDefault()) && float.IsInfinity(PRO_sngAnzeigeWert.GetValueOrDefault()))
					{
						return false;
					}
					return true;
				}
				decimal d = Math.Round((decimal)PRO_sngWert.GetValueOrDefault(), PRO_int32Nachkommastellen);
				decimal d2 = Math.Round((decimal)PRO_sngAnzeigeWert.GetValueOrDefault(), PRO_int32Nachkommastellen);
				return d != d2;
			}
		}

		public float? PRO_sngWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_sngWertUmwandeln(PRO_objValue);
			}
			set
			{
				if (PRO_sngWert != value)
				{
					PRO_objValue = value;
				}
			}
		}

		public float? PRO_sngAnzeigeWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_sngWertUmwandeln(PRO_objAnzeigeWert);
			}
			set
			{
				if (PRO_sngAnzeigeWert != value)
				{
					PRO_objAnzeigeWert = value;
				}
			}
		}

		public string Error => string.Empty;

		public bool PRO_blnIsValide => string.IsNullOrEmpty(this["PRO_sngAnzeigeWert"]);

		public string this[string i_strPropertyName]
		{
			get
			{
				if (i_strPropertyName == "PRO_sngAnzeigeWert")
				{
					return FUN_strValidiereWertGegenRange(PRO_sngAnzeigeWert);
				}
				if (i_strPropertyName == "PRO_sngWert")
				{
					return FUN_strValidiereWertGegenRange(PRO_sngWert);
				}
				return string.Empty;
			}
		}

		public void SUB_RangeAenderungenSignalisieren()
		{
			RaisePropertyChanged("PRO_sngMaximalWert");
			RaisePropertyChanged("PRO_sngMinimalWert");
			RaisePropertyChanged("PRO_sngAnzeigeWert");
			RaisePropertyChanged("PRO_sngWert");
		}

		private string FUN_strValidiereWertGegenRange(float? i_sngWert)
		{
			bool num = i_sngWert < PRO_sngMinimalWert;
			bool flag = i_sngWert > PRO_sngMaximalWert;
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
