using Ersa.Global.Mvvm;
using System;
using System.Collections.Generic;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente
{
	public abstract class EDC_VorlageElement : BindableBase, ICloneable
	{
		private bool m_blnKannVerschobenWerden;

		private bool m_blnAuswaehlbar;

		private bool m_blnAusgewaehlt;

		private bool m_blnKannDeaktiviertWerden;

		private bool m_blnIstAktive;

		public bool PRO_blnKannVerschobenWerden
		{
			get
			{
				return m_blnKannVerschobenWerden;
			}
			set
			{
				SetProperty(ref m_blnKannVerschobenWerden, value, "PRO_blnKannVerschobenWerden");
			}
		}

		public bool PRO_blnKannDeaktiviertWerden
		{
			get
			{
				return m_blnKannDeaktiviertWerden;
			}
			set
			{
				SetProperty(ref m_blnKannDeaktiviertWerden, value, "PRO_blnKannDeaktiviertWerden");
			}
		}

		public bool PRO_blnIstAktiv
		{
			get
			{
				return m_blnIstAktive;
			}
			set
			{
				SetProperty(ref m_blnIstAktive, value, "PRO_blnIstAktiv");
			}
		}

		public bool PRO_blnAuswaehlbar
		{
			get
			{
				return m_blnAuswaehlbar;
			}
			set
			{
				SetProperty(ref m_blnAuswaehlbar, value, "PRO_blnAuswaehlbar");
			}
		}

		public bool PRO_blnAusgewaehlt
		{
			get
			{
				return m_blnAusgewaehlt;
			}
			set
			{
				if (!(!PRO_blnAuswaehlbar && value))
				{
					SetProperty(ref m_blnAusgewaehlt, value, "PRO_blnAusgewaehlt");
				}
			}
		}

		public abstract IEnumerable<EDC_VorlageParameter> PRO_enuParameter
		{
			get;
		}

		public virtual string PRO_strIconUri
		{
			get;
		}

		public virtual bool PRO_blnKannDavorEingefuegtWerden
		{
			get;
		} = true;


		public virtual bool PRO_blnKannDanachEingefuegtWerden
		{
			get;
		} = true;


		public object PRO_objDaten
		{
			get;
			set;
		}

		protected EDC_VorlageElement()
		{
			m_blnKannVerschobenWerden = true;
			m_blnAuswaehlbar = true;
			m_blnIstAktive = true;
		}

		public object Clone()
		{
			EDC_VorlageElement eDC_VorlageElement = FUN_edcClone();
			eDC_VorlageElement.PRO_blnAuswaehlbar = PRO_blnAuswaehlbar;
			eDC_VorlageElement.PRO_blnKannVerschobenWerden = PRO_blnKannVerschobenWerden;
			eDC_VorlageElement.PRO_blnKannDeaktiviertWerden = PRO_blnKannDeaktiviertWerden;
			eDC_VorlageElement.PRO_blnIstAktiv = PRO_blnIstAktiv;
			return eDC_VorlageElement;
		}

		protected abstract EDC_VorlageElement FUN_edcClone();
	}
}
