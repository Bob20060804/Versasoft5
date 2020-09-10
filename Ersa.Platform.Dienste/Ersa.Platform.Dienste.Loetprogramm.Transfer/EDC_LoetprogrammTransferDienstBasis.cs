using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common.Exceptions;
using Ersa.Platform.Common.Model;
using Ersa.Platform.Dienste.Loetprogramm.Interfaces;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Extensions;
using Ersa.Platform.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Loetprogramm.Transfer
{
	public abstract class EDC_LoetprogrammTransferDienstBasis<TLoetprogramm> : INF_LoetprogrammTransferDienst<TLoetprogramm> where TLoetprogramm : class, IComparable<TLoetprogramm>
	{
		protected readonly SemaphoreSlim m_fdcSemaphore = new SemaphoreSlim(1);

		protected readonly INF_Logger m_edcLogger;

		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcMaschinenBasisDatenCapability;

		private readonly INF_LoetprogrammUebertragungsDienst<TLoetprogramm> m_edcLpUebertragungsDienst;

		private readonly INF_AppSettingsDienst m_edcAppSettingsDienst;

		private readonly IEventAggregator m_fdcEventAggregator;

		private readonly bool m_blnUebertrageImmerVollstaendig;

		private readonly IEnumerable<INF_LoetprogrammAnpassungsStartegie<TLoetprogramm>> m_enuAnpassungsStrategien;

		private TLoetprogramm m_edcAktivesLoetprogramm;

		private TaskCompletionSource<bool> m_fdcLoetprogrammTransferiertCompletionSource;

		private CancellationTokenSource m_fdcCancelLoetprogrammTransfer;

		private bool m_blnIstInitialisiert;

		private bool m_blnIstInAutomatik;

		private bool m_blnNaechstesLoetprogrammVollstaendigUebertragen;

		private bool m_blnManuellEingebenesProgrammVerwenden;

		private ENUM_LpTransferModus m_enmLpTransferModus;

		private ENUM_LpTransferStrategie m_enmLpTransferStrategie;

		private IDisposable m_fdcBetriebsartBenachrichtigungDisposable;

		protected EDC_LoetprogrammTransferDienstBasis(Lazy<INF_MaschinenBasisDatenCapability> i_edcMaschinenBasisDatenCapability, INF_LoetprogrammUebertragungsDienst<TLoetprogramm> i_edcLpUebertragungsDienst, IEnumerable<INF_LoetprogrammAnpassungsStartegie<TLoetprogramm>> i_enuAnpassungsStrategien, INF_AppSettingsDienst i_edcAppSettingsDienst, INF_Logger i_edcLogger, IEventAggregator i_fdcEventAggregator)
		{
			m_edcMaschinenBasisDatenCapability = i_edcMaschinenBasisDatenCapability;
			m_edcLpUebertragungsDienst = i_edcLpUebertragungsDienst;
			m_enuAnpassungsStrategien = i_enuAnpassungsStrategien;
			m_edcAppSettingsDienst = i_edcAppSettingsDienst;
			m_edcLogger = i_edcLogger;
			m_fdcEventAggregator = i_fdcEventAggregator;
			m_blnIstInitialisiert = false;
			bool.TryParse(i_edcAppSettingsDienst.FUN_strAppSettingErmitteln("PrgImmerVollstaendigUebertragen"), out m_blnUebertrageImmerVollstaendig);
		}

		public async Task FUN_fdcLoetprogrammStartAnfordernAsync(ENUM_LpTransferStrategie i_enmLpTransferStrategie)
		{
			SUB_Initialisieren(i_enmLpTransferStrategie);
			TLoetprogramm edcLoetprogramm = FUN_edcAktivesLoetprogrammErmitteln();
			if (edcLoetprogramm == null)
			{
				throw new InvalidOperationException("No soldering program specified! Cannot start production");
			}
			bool blnIstInEinrichten = m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstInEinrichten();
			bool flag = m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstInProduktion();
			if (!blnIstInEinrichten && !flag)
			{
				throw new EDC_MaschineInFalscherBetriebsartException("Maintenance mode", "Automatic mode");
			}
			try
			{
				await m_fdcSemaphore.WaitAsync();
				if (m_fdcLoetprogrammTransferiertCompletionSource != null)
				{
					throw new InvalidOperationException("Soldering program is already being transfered");
				}
				m_fdcLoetprogrammTransferiertCompletionSource = new TaskCompletionSource<bool>();
			}
			finally
			{
				m_fdcSemaphore.Release();
			}
			await FUN_fdcLoetprogrammStartAnforderungVorbereitenAsync(edcLoetprogramm).ConfigureAwait(continueOnCapturedContext: false);
			if (blnIstInEinrichten)
			{
				await FUN_fdcFordereBetriebsartAutomatikAnAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				SUB_SetzeTriggerProgrammAenderung();
			}
		}

		public async Task<bool> FUN_fdcLoetprogrammTransferAbschliessenAsync(bool i_blnTransferNichtAbwarten)
		{
			TaskCompletionSource<bool> fdcLoetprogrammTransferiertCompletionSource = m_fdcLoetprogrammTransferiertCompletionSource;
			if (fdcLoetprogrammTransferiertCompletionSource == null)
			{
				return false;
			}
			bool result;
			try
			{
				result = (i_blnTransferNichtAbwarten || await fdcLoetprogrammTransferiertCompletionSource.Task);
			}
			finally
			{
				try
				{
					await m_fdcSemaphore.WaitAsync();
					m_fdcLoetprogrammTransferiertCompletionSource = null;
				}
				finally
				{
					m_fdcSemaphore.Release();
				}
			}
			return result;
		}

		public abstract void SUB_LoetprogrammExistiertNichtSignalisieren();

		public void SUB_ManuellEingegebenenCodeHinzufuegen(int i_i32GeraeteIndex, string i_strCode)
		{
			FUN_edcHoleLoetProgrammAnfoStrategie().SUB_ManuellEingegebenenCodeHinzufuegen(i_i32GeraeteIndex, i_strCode);
		}

		public void SUB_SetzeManuellEingegebenesProgramm(TLoetprogramm i_edcLoetprogramm)
		{
			SUB_AktivesProgrammSetzen(i_edcLoetprogramm);
			m_blnManuellEingebenesProgrammVerwenden = true;
		}

		public void SUB_SetzeLoetprogrammTransferModus(ENUM_LpTransferModus i_enmLpTransferModus)
		{
			m_enmLpTransferModus = i_enmLpTransferModus;
		}

		public async Task FUN_fdcVersucheLoetprogrammZuUebertragenAsync()
		{
			bool blnErstmaligeUebertragung = m_blnNaechstesLoetprogrammVollstaendigUebertragen || !m_blnIstInAutomatik;
			switch ((await FUN_fdcAngefordertesLoetprogrammUebertragenAsync(blnErstmaligeUebertragung)).PRO_enmAnforderungsErgebnis)
			{
			case ENUM_LpAnforderungsErgebnis.LoetprogrammExistiertNicht:
				SUB_LoetprogrammExistiertNichtSignalisieren();
				break;
			case ENUM_LpAnforderungsErgebnis.CodeLesefehler:
				if (!blnErstmaligeUebertragung)
				{
					SUB_CodelesefehlerSignalisieren();
				}
				break;
			case ENUM_LpAnforderungsErgebnis.CodeNichtGefunden:
				if (!blnErstmaligeUebertragung)
				{
					SUB_CodeInterpretationsfehlerSignalisieren();
				}
				break;
			}
		}

		public async Task FUN_fdcCodebetriebNeuStarten()
		{
			if (m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstInProduktion())
			{
				await FUN_edcBetriebsartAenderungStrategieErmitteln().FUN_fdcCodebetriebNeuStarten();
			}
		}

		public INF_LpAnfoStrategie<TLoetprogramm> FUN_edcHoleLoetProgrammAnfoStrategie()
		{
			SUB_Initialisieren();
			return FUN_edcHoleLoetProgrammAnfoStrategie(m_enmLpTransferStrategie);
		}

		public void SUB_GeheInAutomatikBetrieb()
		{
			if (!m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstInProduktion())
			{
				throw new InvalidOperationException("The machine must be in Automatic mode");
			}
			m_blnIstInAutomatik = true;
		}

		public async Task FUN_fdcVerlasseDenAutomatikBetriebAsync()
		{
			if (m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstInProduktion())
			{
				throw new InvalidOperationException("The machine must not be in Automatic mode");
			}
			FUN_edcBetriebsartAenderungStrategieErmitteln().SUB_VerlasseDenAutomatikBetrieb();
			SUB_SchreibeLsgParameter();
			if (m_blnIstInAutomatik)
			{
				await FUN_fdcAutomatikVerlassenBehandelnAsync();
			}
			FUN_edcHoleLoetProgrammAnfoStrategie().SUB_ManuellEingegebeneCodesLeeren();
		}

		public void SUB_SetzeUnitTestUmgebung()
		{
			m_blnIstInAutomatik = true;
			m_blnNaechstesLoetprogrammVollstaendigUebertragen = false;
		}

		protected abstract EDC_BooleanParameter FUN_edcLoetprogrammAnforderungsParameterErmittlen();

		protected abstract Task FUN_fdcLoetprogrammStartAnforderungVorbereitenAsync(TLoetprogramm i_edcLoetprogramm);

		protected abstract ENUM_LpTransferStrategie FUN_enmAktuelleLpTransferStrategieErmitteln();

		protected abstract INF_LpAnfoStrategie<TLoetprogramm> FUN_edcHoleLoetProgrammAnfoStrategie(ENUM_LpTransferStrategie i_enmLpTransferStrategie);

		protected abstract INF_BaAenderungStrategie FUN_edcBetriebsartAenderungStrategieErmitteln(ENUM_LpTransferStrategie i_enmLpTransferStrategie);

		protected abstract TLoetprogramm FUN_edcAktivesLoetprogrammErmitteln();

		protected abstract void SUB_AktivesProgrammSetzen(TLoetprogramm i_edcLoetprogramm);

		protected abstract Task<TLoetprogramm> FUN_edcLoetprogrammAnhandMaschinenStatusLadenAsync();

		protected abstract IDisposable FUN_fdcFordereBetriebsartAutomatikAn(Func<Task> i_delBenachrichtigunsTask);

		protected abstract void SUB_SetzeTriggerProgrammAenderung();

		protected abstract void SUB_CodelesefehlerSignalisieren();

		protected abstract void SUB_CodeInterpretationsfehlerSignalisieren();

		protected abstract void SUB_SchreibeLsgParameter();

		protected virtual Func<Task>[] FUNa_delAktionenVorLoetprogrammUebertragungErmitteln(TLoetprogramm i_edcLoetprogramm, CancellationToken i_fdcCancellationToken)
		{
			return new Func<Task>[0];
		}

		protected void SUB_AufLoetprogrammAnforderungRegistrieren()
		{
			m_edcAktivesLoetprogramm = FUN_edcAktivesLoetprogrammErmitteln();
			PropertyChangedEventManager.AddHandler((INotifyPropertyChanged)FUN_edcLoetprogrammAnforderungsParameterErmittlen(), async delegate(object s, PropertyChangedEventArgs a)
			{
				await FUN_fdcLoetprogrammAnforderungChangedAsync(s, a).ConfigureAwait(continueOnCapturedContext: true);
			}, "PRO_blnWert");
		}

		private Task FUN_fdcLoetprogrammAnforderungChangedAsync(object i_objSender, PropertyChangedEventArgs i_fdcArgs)
		{
			EDC_BooleanParameter eDC_BooleanParameter = i_objSender as EDC_BooleanParameter;
			if (eDC_BooleanParameter == null || !eDC_BooleanParameter.PRO_blnWert)
			{
				return Task.CompletedTask;
			}
			return FUN_fdcVersucheLoetprogrammZuUebertragenAsync();
		}

		private void SUB_Initialisieren(ENUM_LpTransferStrategie? i_enmLpTransferStrategie = default(ENUM_LpTransferStrategie?))
		{
			if (i_enmLpTransferStrategie.HasValue)
			{
				m_enmLpTransferStrategie = i_enmLpTransferStrategie.Value;
			}
			else if (!m_blnIstInitialisiert)
			{
				m_enmLpTransferStrategie = FUN_enmAktuelleLpTransferStrategieErmitteln();
			}
			m_blnIstInitialisiert = true;
		}

		private async Task FUN_fdcFordereBetriebsartAutomatikAnAsync()
		{
			INF_BaAenderungStrategie iNF_BaAenderungStrategie = FUN_edcBetriebsartAenderungStrategieErmitteln();
			iNF_BaAenderungStrategie.SUB_SetzeLsgSollwerte();
			SUB_SchreibeLsgParameter();
			await iNF_BaAenderungStrategie.FUN_fdcGeheInAutomatikBetrieb();
			m_fdcBetriebsartBenachrichtigungDisposable = FUN_fdcFordereBetriebsartAutomatikAn(async delegate
			{
				await FUN_fdcAufBetriebsArtAenderungReagierenAsync().ConfigureAwait(continueOnCapturedContext: false);
			});
			await FUN_fdcTimeoutWennNichtNachAutomatikGewechseltWurdeAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task FUN_fdcTimeoutWennNichtNachAutomatikGewechseltWurdeAsync()
		{
			if (int.TryParse(m_edcAppSettingsDienst.FUN_strAppSettingErmitteln("TransferTimeInMilliSek"), out int result) && result >= 0)
			{
				await m_fdcLoetprogrammTransferiertCompletionSource.Task.FUN_fdcMitTimeout(result, delegate
				{
					if (!m_blnIstInAutomatik)
					{
						m_fdcLoetprogrammTransferiertCompletionSource.TrySetResult(result: false);
						FUN_edcBetriebsartAenderungStrategieErmitteln().SUB_VerlasseDenAutomatikBetrieb();
					}
				}).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task FUN_fdcAufBetriebsArtAenderungReagierenAsync()
		{
			m_blnNaechstesLoetprogrammVollstaendigUebertragen = true;
			if (m_edcMaschinenBasisDatenCapability.Value.FUN_blnIstInProduktion())
			{
				SUB_GeheInAutomatikBetrieb();
			}
			else
			{
				await FUN_fdcVerlasseDenAutomatikBetriebAsync();
			}
		}

		private async Task FUN_fdcAutomatikVerlassenBehandelnAsync()
		{
			if (m_fdcBetriebsartBenachrichtigungDisposable != null)
			{
				m_fdcBetriebsartBenachrichtigungDisposable.Dispose();
			}
			m_blnIstInAutomatik = false;
			TLoetprogramm val = FUN_edcAktivesLoetprogrammErmitteln();
			if (val != null)
			{
				if (m_fdcCancelLoetprogrammTransfer != null)
				{
					m_fdcCancelLoetprogrammTransfer.Cancel();
				}
				await m_edcLpUebertragungsDienst.FUN_fdcLoetprogrammAdressenDeregistrierenAsync(val);
			}
		}

		private async Task<EDC_LoetprogrammAnforderungsErgebnis<TLoetprogramm>> FUN_fdcAngefordertesLoetprogrammUebertragenAsync(bool i_blnIgnoriereCodeFehler)
		{
			INF_LpAnfoStrategie<TLoetprogramm> edcLpAnfoStrategie = FUN_edcHoleLoetProgrammAnfoStrategie();
			TaskCompletionSource<bool> fdcTsc;
			try
			{
				await m_fdcSemaphore.WaitAsync();
				fdcTsc = m_fdcLoetprogrammTransferiertCompletionSource;
				m_fdcCancelLoetprogrammTransfer = new CancellationTokenSource();
			}
			finally
			{
				m_fdcSemaphore.Release();
			}
			try
			{
				EDC_LoetprogrammAnforderungsErgebnis<TLoetprogramm> edcAnforderungsErgebnis2 = await edcLpAnfoStrategie.FUN_fdcAufLoetprogrammAnforderungReagierenAsync(FUN_edcAktivesLoetprogrammErmitteln() ?? (await FUN_edcLoetprogrammAnhandMaschinenStatusLadenAsync().ConfigureAwait(continueOnCapturedContext: false)), m_blnManuellEingebenesProgrammVerwenden, m_enmLpTransferModus).ConfigureAwait(continueOnCapturedContext: false);
				if (m_enuAnpassungsStrategien != null && m_enuAnpassungsStrategien.Any())
				{
					foreach (INF_LoetprogrammAnpassungsStartegie<TLoetprogramm> item in m_enuAnpassungsStrategien)
					{
						await item.FUN_fdcAnpassenAsync(edcAnforderungsErgebnis2.PRO_edcLoetprogramm).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				m_blnManuellEingebenesProgrammVerwenden = false;
				m_enmLpTransferModus = ENUM_LpTransferModus.Normal;
				if (FUN_blnKannLoetprogrammUebertragen(edcAnforderungsErgebnis2.PRO_enmAnforderungsErgebnis, i_blnIgnoriereCodeFehler))
				{
					edcLpAnfoStrategie.SUB_ManuellEingegebeneCodesLeeren();
					await FUN_fdcLoetprogrammUebertragenAsync(m_edcAktivesLoetprogramm, edcAnforderungsErgebnis2.PRO_edcLoetprogramm).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					string i_strEintrag = $"soldering program can not be transfered cause program request result = {edcAnforderungsErgebnis2.PRO_enmAnforderungsErgebnis}! Value for ignore code reading error = {i_blnIgnoriereCodeFehler}";
					m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, i_strEintrag, "Ersa.Platform.Dienste.Loetprogramm.Transfer", "EDC_LoetprogrammTransferDienstBasis", "FUN_fdcAngefordertesLoetprogrammUebertragenAsync");
				}
				SUB_OperationAbschliessen(fdcTsc, TaskStatus.RanToCompletion);
				return edcAnforderungsErgebnis2;
			}
			catch (Exception ex)
			{
				m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, ex.StackTrace, "Ersa.Platform.Dienste.Loetprogramm.Transfer", "EDC_LoetprogrammTransferDienstBasis", "FUN_fdcAngefordertesLoetprogrammUebertragenAsync");
				EDC_LoetprogrammAnforderungsErgebnis<TLoetprogramm> edcAnforderungsErgebnis2 = new EDC_LoetprogrammAnforderungsErgebnis<TLoetprogramm>(ENUM_LpAnforderungsErgebnis.AusnahmeFehler);
				SUB_OperationAbschliessen(fdcTsc, (ex is OperationCanceledException) ? TaskStatus.Canceled : TaskStatus.Faulted, ex);
				return edcAnforderungsErgebnis2;
			}
		}

		private bool FUN_blnKannLoetprogrammUebertragen(ENUM_LpAnforderungsErgebnis i_enmErgebnis, bool i_blnIgnoriereCodeFehler)
		{
			if (i_enmErgebnis != 0 && !i_blnIgnoriereCodeFehler)
			{
				return false;
			}
			if (i_enmErgebnis != 0 && i_enmErgebnis != ENUM_LpAnforderungsErgebnis.CodeLesefehler)
			{
				return i_enmErgebnis == ENUM_LpAnforderungsErgebnis.CodeNichtGefunden;
			}
			return true;
		}

		private async Task FUN_fdcLoetprogrammUebertragenAsync(TLoetprogramm i_edcAktivesLoetprogramm, TLoetprogramm i_edcLoetprogrammZuVerwenden)
		{
			if (m_blnUebertrageImmerVollstaendig || i_edcAktivesLoetprogramm == null || m_blnNaechstesLoetprogrammVollstaendigUebertragen || i_edcAktivesLoetprogramm.CompareTo(i_edcLoetprogrammZuVerwenden) < 0)
			{
				m_blnNaechstesLoetprogrammVollstaendigUebertragen = false;
				m_edcAktivesLoetprogramm = i_edcLoetprogrammZuVerwenden;
				if (!(await m_edcLpUebertragungsDienst.FUN_blnLoetprogrammVollstaendigUebertragenAsync(i_edcLoetprogrammZuVerwenden, m_fdcCancelLoetprogrammTransfer.Token, FUNa_delAktionenVorLoetprogrammUebertragungErmitteln(i_edcLoetprogrammZuVerwenden, m_fdcCancelLoetprogrammTransfer.Token))))
				{
					SUB_LoetprogrammExistiertNichtSignalisieren();
				}
			}
			else
			{
				await m_edcLpUebertragungsDienst.FUN_fdcLoetprogrammKopfdatenUebertragen(i_edcLoetprogrammZuVerwenden, m_fdcCancelLoetprogrammTransfer.Token);
			}
		}

		private void SUB_OperationAbschliessen(TaskCompletionSource<bool> i_fdcTsc, TaskStatus i_enmTaskStatus, Exception i_fdcEx = null)
		{
			if (i_fdcTsc != null)
			{
				switch (i_enmTaskStatus)
				{
				case TaskStatus.RanToCompletion:
					i_fdcTsc.TrySetResult(result: true);
					break;
				case TaskStatus.Faulted:
					if (i_fdcEx != null)
					{
						i_fdcTsc.TrySetException(i_fdcEx);
					}
					break;
				case TaskStatus.Canceled:
					i_fdcTsc.TrySetCanceled();
					break;
				default:
					throw new NotSupportedException($"{i_enmTaskStatus} Task status is not supported");
				}
			}
			else if (i_enmTaskStatus != TaskStatus.Canceled && i_fdcEx != null)
			{
				m_fdcEventAggregator.GetEvent<EDC_LoetprogrammTransferFehlgeschlagenEvent>().Publish(new EDC_LoetprogrammTransferFehlgeschlagenEventPayload
				{
					PRO_fdcException = i_fdcEx
				});
			}
		}

		private INF_BaAenderungStrategie FUN_edcBetriebsartAenderungStrategieErmitteln()
		{
			SUB_Initialisieren();
			return FUN_edcBetriebsartAenderungStrategieErmitteln(m_enmLpTransferStrategie);
		}
	}
}
