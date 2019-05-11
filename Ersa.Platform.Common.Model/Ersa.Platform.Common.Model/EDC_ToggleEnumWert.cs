using Ersa.Global.Mvvm;
using System;
using System.ComponentModel;

namespace Ersa.Platform.Common.Model
{
	public class EDC_ToggleEnumWert<TEnum> : BindableBase where TEnum : struct, IConvertible, IComparable, IFormattable
	{
		private bool m_blnToggleQuittAenderung;

		public EDC_EnumParameter<TEnum> PRO_edcAnfoParameter
		{
			get;
			private set;
		}

		public TEnum? PRO_enmAnfoSetzenWert
		{
			get;
			private set;
		}

		public TEnum? PRO_enmAnfoZurueckSetzenWert
		{
			get;
			private set;
		}

		public EDC_EnumParameter<TEnum> PRO_edcQuittParameter
		{
			get;
		}

		public TEnum? PRO_enmQuittierWert
		{
			get;
		}

		public bool PRO_blnToggleQuittAenderung
		{
			get
			{
				return m_blnToggleQuittAenderung;
			}
			set
			{
				m_blnToggleQuittAenderung = value;
				RaisePropertyChanged("PRO_blnToggleQuittAenderung");
			}
		}

		public EDC_ToggleEnumWert(EDC_EnumParameter<TEnum> i_blnAnfoParameter, TEnum? i_blnAnfoSetzenWert, TEnum? i_blnAnfoZuruecksetzenWert, EDC_EnumParameter<TEnum> i_blnQuittParameter, TEnum? i_blnQuittierWert)
		{
			PRO_edcAnfoParameter = i_blnAnfoParameter;
			PRO_enmAnfoSetzenWert = i_blnAnfoSetzenWert;
			PRO_enmAnfoZurueckSetzenWert = i_blnAnfoZuruecksetzenWert;
			PRO_edcQuittParameter = i_blnQuittParameter;
			PRO_enmQuittierWert = i_blnQuittierWert;
			if (PRO_edcQuittParameter != null)
			{
				PropertyChangedEventManager.AddHandler(PRO_edcQuittParameter, SUB_PropertyChanged, "PRO_enmWert");
			}
		}

		private void SUB_PropertyChanged(object i_objSender, PropertyChangedEventArgs i_fdcPropertyChangedEventArgs)
		{
			if (i_fdcPropertyChangedEventArgs.PropertyName == "PRO_enmWert" && PRO_edcQuittParameter.PRO_enmWert.Equals(PRO_enmQuittierWert))
			{
				PRO_blnToggleQuittAenderung = true;
			}
		}
	}
}
