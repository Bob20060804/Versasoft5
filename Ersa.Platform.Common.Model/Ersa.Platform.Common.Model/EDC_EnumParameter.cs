using Ersa.Platform.Infrastructure;
using System;

namespace Ersa.Platform.Common.Model
{
	public class EDC_EnumParameter<T> : EDC_PrimitivParameter where T : struct
	{
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
					RaisePropertyChanged("PRO_enmWert");
					RaisePropertyChanged("PRO_enmAnzeigeWert");
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
				RaisePropertyChanged("PRO_enmAnzeigeWert");
			}
		}

		public T? PRO_enmWert
		{
			get
			{
				if (PRO_objValue == null)
				{
					return null;
				}
				return (T)Enum.Parse(typeof(T), PRO_objValue.ToString(), ignoreCase: true);
			}
			set
			{
				if (!object.Equals(PRO_enmWert, value))
				{
					PRO_objValue = (value.HasValue ? new int?(Convert.ToInt32(value.Value)) : null);
				}
			}
		}

		public T? PRO_enmAnzeigeWert
		{
			get
			{
				if (PRO_objAnzeigeWert == null)
				{
					return null;
				}
				return (T)Enum.Parse(typeof(T), PRO_objAnzeigeWert.ToString(), ignoreCase: true);
			}
			set
			{
				if (!object.Equals(PRO_enmAnzeigeWert, value))
				{
					PRO_objAnzeigeWert = (value.HasValue ? new int?(Convert.ToInt32(value.Value)) : null);
				}
			}
		}

		public override bool PRO_blnHatAenderung
		{
			get
			{
				if (PRO_enmWert.HasValue && PRO_enmAnzeigeWert.HasValue)
				{
					return PRO_enmWert.ToString() != PRO_enmAnzeigeWert.ToString();
				}
				return PRO_enmWert.HasValue != PRO_enmAnzeigeWert.HasValue;
			}
		}
	}
}
