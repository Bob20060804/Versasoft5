using Ersa.Global.Mvvm;
using Ersa.Platform.Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.ViewModels
{
	public abstract class EDC_LoetprogrammViewModelBasis<TLoetprogramm> : BindableBase, IDataErrorInfo
	{
		private readonly IEventAggregator m_fdcEventAggregator;

		private readonly SemaphoreSlim m_fdcLoetprogrammSemaphore = new SemaphoreSlim(1);

		private bool m_blnIstValide = true;

		private bool m_blnCodebetriebVerfuegbar;

		public bool PRO_blnIstValide
		{
			get
			{
				return m_blnIstValide;
			}
			protected set
			{
				SetProperty(ref m_blnIstValide, value, "PRO_blnIstValide");
			}
		}

		public virtual string Error => string.Empty;

		public DelegateCommand PRO_cmdWertGeaendert
		{
			get;
		}

		public virtual bool PRO_blnHatAenderung => false;

		public virtual bool PRO_blnHatDaten => true;

		public bool PRO_blnCodebetriebVerfuegbar
		{
			get
			{
				return m_blnCodebetriebVerfuegbar;
			}
			private set
			{
				SetProperty(ref m_blnCodebetriebVerfuegbar, value, "PRO_blnCodebetriebVerfuegbar");
			}
		}

		protected TLoetprogramm PRO_edcLoetprogramm
		{
			get;
			private set;
		}

		public virtual string this[string i_strPropertyName]
		{
			get
			{
				if (i_strPropertyName == "PRO_blnIstValide")
				{
					if (!PRO_blnIstValide)
					{
						return "5_5";
					}
					return string.Empty;
				}
				return string.Empty;
			}
		}

		protected EDC_LoetprogrammViewModelBasis(IEventAggregator i_fdcEventAggregator)
		{
			m_fdcEventAggregator = i_fdcEventAggregator;
			PRO_cmdWertGeaendert = new DelegateCommand(SUB_WertAenderungVerarbeiten);
			m_fdcEventAggregator.GetEvent<EDC_SoftwareFeatureGeaendertEvent>().Subscribe(SUB_SoftwareFeaturesAuswerten);
		}

		public virtual void SUB_BerechtigungenAuswerten()
		{
		}

		public void SUB_ValidierungErneuern()
		{
			m_fdcLoetprogrammSemaphore.Wait();
			try
			{
				if (PRO_edcLoetprogramm != null)
				{
					SUB_Invalidate();
				}
			}
			finally
			{
				m_fdcLoetprogrammSemaphore.Release();
			}
		}

		public virtual Task FUN_fdcViewModelInitialisierenAsync(TLoetprogramm i_edcLoetprogramm, CancellationToken i_fdcToken)
		{
			i_fdcToken.ThrowIfCancellationRequested();
			SUB_LoetprogrammSetzen(i_edcLoetprogramm);
			RaisePropertyChanged("PRO_blnHatDaten");
			PRO_blnIstValide = true;
			return Task.FromResult(0);
		}

		public virtual void SUB_AenderungenVerwerfen()
		{
		}

		[Obsolete("FUN_fdcAufraeumenAsync verwenden")]
		public virtual void SUB_Aufraeumen()
		{
			SUB_LoetprogrammSetzen(default(TLoetprogramm));
			PRO_blnIstValide = true;
		}

		public virtual Task FUN_fdcAufraeumenAsync()
		{
			SUB_LoetprogrammSetzen(default(TLoetprogramm));
			PRO_blnIstValide = true;
			return Task.FromResult(0);
		}

		protected virtual void SUB_WertAenderungVerarbeiten()
		{
			SUB_ValidierungErneuern();
		}

		protected abstract void SUB_Invalidate();

		private void SUB_LoetprogrammSetzen(TLoetprogramm i_edcLoetprogramm)
		{
			m_fdcLoetprogrammSemaphore.Wait();
			try
			{
				PRO_edcLoetprogramm = i_edcLoetprogramm;
			}
			finally
			{
				m_fdcLoetprogrammSemaphore.Release();
			}
		}

		private void SUB_SoftwareFeaturesAuswerten(List<EDC_SoftwareFeatureGeaendertPayload> i_lstPayloads)
		{
			foreach (EDC_SoftwareFeatureGeaendertPayload i_lstPayload in i_lstPayloads)
			{
				if (ENUM_SoftwareFeatures.CodelesenFeature.Equals(i_lstPayload.PRO_enmSoftwareFeature))
				{
					PRO_blnCodebetriebVerfuegbar = i_lstPayload.PRO_blnSoftwareFeatureVorhanden;
				}
			}
		}
	}
}
