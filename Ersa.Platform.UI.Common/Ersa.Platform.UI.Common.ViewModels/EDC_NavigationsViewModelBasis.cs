using Ersa.Global.Common;
using Ersa.Global.Mvvm;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Logging;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Common.Interfaces;
using Prism.Events;
using Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common.ViewModels
{
	public abstract class EDC_NavigationsViewModelBasis : BindableBase, IConfirmNavigationRequest, INavigationAware, INF_SpeichernUndVerwerfenCommandSource, INF_SpeichernCommandSource, INF_BasisCommandSource, INF_VerwerfenCommandSource
	{
		private IEventAggregator m_fdcEventAggregator;

		private bool m_blnIsAktiv;

		private bool m_blnIstFehlerhaft;

		[Import]
		public IEventAggregator PRO_fdcEventAggregator
		{
			protected get
			{
				return m_fdcEventAggregator;
			}
			set
			{
				m_fdcEventAggregator = value;
				m_fdcEventAggregator.GetEvent<EDC_BenutzerGeaendertEvent>().Subscribe(delegate
				{
					EDC_Dispatch.SUB_AktionStarten(SUB_BerechtigungenAuswerten);
				});
			}
		}

		[Import]
		public INF_LokalisierungsDienst PRO_edcLokalisierungsDienst
		{
			protected get;
			set;
		}

		[Import]
		public INF_InteractionController PRO_edcInteractionController
		{
			get;
			set;
		}

		[Import(AllowDefault = true)]
		public INF_FortschrittsanzeigeViewModel PRO_edcFortschrittsanzeigeViewModel
		{
			private get;
			set;
		}

		[Import]
		public INF_Logger PRO_edcLogger
		{
			get;
			set;
		}

		[Import]
		public EDC_BasisCommandsViewModel PRO_edcBasisCommandsViewModel
		{
			get;
			set;
		}

		[Import]
		public INF_AktionsWiederholungsHelfer PRO_edcAktionsWiederholungsHelfer
		{
			get;
			set;
		}

		public bool PRO_blnIsAktiv
		{
			get
			{
				return m_blnIsAktiv;
			}
			protected set
			{
				m_blnIsAktiv = value;
				RaisePropertyChanged("PRO_blnIsAktiv");
			}
		}

		public bool PRO_blnIstFehlerhaft
		{
			get
			{
				return m_blnIstFehlerhaft;
			}
			set
			{
				SetProperty(ref m_blnIstFehlerhaft, value, "PRO_blnIstFehlerhaft");
			}
		}

		public abstract bool PRO_blnHatAenderung
		{
			get;
		}

		public async void OnNavigatedTo(NavigationContext i_fdcNavigationContext)
		{
			if (!PRO_blnIsAktiv)
			{
				PRO_blnIsAktiv = true;
				using (FUN_fdcFortschrittsAnzeigeEinblenden())
				{
					try
					{
						await FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext).ConfigureAwait(continueOnCapturedContext: true);
					}
					catch (Exception i_fdcEx)
					{
						SUB_NavigationsFehlerBehandeln(i_fdcEx);
					}
					SUB_AktualisiereZustand();
				}
			}
		}

		bool INavigationAware.IsNavigationTarget(NavigationContext i_fdcNavigationContext)
		{
			return true;
		}

		public async void OnNavigatedFrom(NavigationContext i_fdcNavigationContext)
		{
			if (PRO_blnIsAktiv)
			{
				PRO_blnIsAktiv = false;
				try
				{
					await FUN_fdcOnNavigatedFromAsync(i_fdcNavigationContext).ConfigureAwait(continueOnCapturedContext: true);
				}
				catch (Exception i_fdcEx)
				{
					SUB_NavigationsFehlerBehandeln(i_fdcEx);
				}
			}
		}

		public async void ConfirmNavigationRequest(NavigationContext i_fdcNavigationContext, Action<bool> i_delContinuationCallback)
		{
			try
			{
				bool flag = !PRO_blnIsAktiv;
				if (!flag)
				{
					flag = await FUN_blnKannVerlassenAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
				bool obj = flag;
				i_delContinuationCallback(obj);
			}
			catch (Exception i_fdcEx)
			{
				SUB_NavigationsFehlerBehandeln(i_fdcEx, i_blnDialogAnzeigen: false);
			}
		}

		public void SUB_SetzeIstAktiv(bool i_blnIstAktiv)
		{
			PRO_blnIsAktiv = i_blnIstAktiv;
		}

		public virtual Task FUN_fdcNavigationsKontextSetzenAsync(object i_objNavigationsKontext)
		{
			return Task.FromResult(0);
		}

		public virtual Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			return Task.FromResult(0);
		}

		public virtual Task FUN_fdcOnNavigatedFromAsync(NavigationContext i_fdcNavigationContext)
		{
			return Task.FromResult(0);
		}

		public virtual void SUB_AktualisiereZustand()
		{
			RaisePropertyChanged("PRO_blnHatAenderung");
		}

		public virtual Task FUN_fdcAenderungenVerwerfenAsync()
		{
			SUB_AktualisiereZustand();
			return Task.FromResult(result: true);
		}

		public virtual Task FUN_fdcAenderungenSpeichernAsync()
		{
			SUB_AktualisiereZustand();
			return Task.FromResult(result: true);
		}

		protected virtual void SUB_BerechtigungenAuswerten()
		{
		}

		protected IDisposable FUN_fdcFortschrittsAnzeigeEinblenden(bool i_blnUeberdeckendeAnzeige = false, string i_strAnzeigeText = null)
		{
			return PRO_edcFortschrittsanzeigeViewModel?.FUN_fdcFortschrittsAnzeigeEinblenden(i_blnUeberdeckendeAnzeige, i_strAnzeigeText) ?? EDC_Disposable.FUN_fdcEmpty();
		}

		protected virtual async Task<bool> FUN_blnKannVerlassenAsync()
		{
			ENUM_Eingabe enmEingabe = ENUM_Eingabe.Undefiniert;
			if (PRO_blnHatAenderung)
			{
				enmEingabe = PRO_edcInteractionController.FUN_enmJaNeinAbbrechenAbfrageAnzeigen(PRO_edcLokalisierungsDienst.FUN_strText("13_81"), PRO_edcLokalisierungsDienst.FUN_strText("13_82"));
				if (enmEingabe == ENUM_Eingabe.Ja)
				{
					await FUN_fdcAenderungenSpeichernAsync().ConfigureAwait(continueOnCapturedContext: true);
					if (PRO_blnHatAenderung)
					{
						enmEingabe = ENUM_Eingabe.Abbrechen;
					}
				}
				if (enmEingabe == ENUM_Eingabe.Nein)
				{
					await FUN_fdcAenderungenVerwerfenAsync().ConfigureAwait(continueOnCapturedContext: true);
				}
			}
			return enmEingabe != ENUM_Eingabe.Abbrechen;
		}

		private void SUB_NavigationsFehlerBehandeln(Exception i_fdcEx, bool i_blnDialogAnzeigen = true)
		{
			PRO_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Unexpected error during navigation", null, null, null, i_fdcEx);
			if (i_blnDialogAnzeigen)
			{
				string i_strText = string.Format("{0}{1}{1}{2}", i_fdcEx.Message, Environment.NewLine, PRO_edcLokalisierungsDienst.FUN_strText("13_762"));
				if (PRO_edcInteractionController.FUN_enmOkAbbrechenAbfrageAnzeigen(PRO_edcLokalisierungsDienst.FUN_strText("10_1558"), i_strText, PRO_edcLokalisierungsDienst.FUN_strText("13_80"), PRO_edcLokalisierungsDienst.FUN_strText("13_79")) != ENUM_Eingabe.Abbrechen)
				{
					PRO_edcInteractionController.SUB_OkHinweisAnzeigen(PRO_edcLokalisierungsDienst.FUN_strText("10_1558"), i_fdcEx.ToString());
				}
			}
		}
	}
}
