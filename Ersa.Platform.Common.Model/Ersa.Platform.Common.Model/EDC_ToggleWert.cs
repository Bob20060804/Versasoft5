using Ersa.Global.Mvvm;
using System.ComponentModel;

namespace Ersa.Platform.Common.Model
{
    /// <summary>
    /// 切换值
    /// Toggle value
    /// </summary>
	public class EDC_ToggleWert : BindableBase
	{
		private bool m_blnToggleQuittAenderung;

		public EDC_BooleanParameter PRO_edcEin
		{
			get;
			private set;
		}

		public EDC_BooleanParameter PRO_edcToggle
		{
			get;
			private set;
		}

		public EDC_BooleanParameter PRO_edcToggleQuitt
		{
			get;
		}

		public bool PRO_blnToggleQuittVorgangAktiv
		{
			get;
			set;
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

		public EDC_ToggleWert(EDC_BooleanParameter i_edcEin, EDC_BooleanParameter i_edcToggle, EDC_BooleanParameter i_edcToggleQuitt)
		{
			PRO_edcEin = i_edcEin;
			PRO_edcToggle = i_edcToggle;
			PRO_edcToggleQuitt = i_edcToggleQuitt;

			if (PRO_edcToggleQuitt != null)
			{
				PropertyChangedEventManager.AddHandler(PRO_edcToggleQuitt, SUB_PropertyChanged, "PRO_blnWert");
			}
		}

        /// <summary>
        /// 属性改变
        /// </summary>
        /// <param name="i_objSender"></param>
        /// <param name="i_fdcPropertyChangedEventArgs"></param>
		private void SUB_PropertyChanged(object i_objSender, PropertyChangedEventArgs i_fdcPropertyChangedEventArgs)
		{
			if (i_fdcPropertyChangedEventArgs.PropertyName == "PRO_blnWert")
			{
				PRO_blnToggleQuittAenderung = true;
			}
		}
	}
}
