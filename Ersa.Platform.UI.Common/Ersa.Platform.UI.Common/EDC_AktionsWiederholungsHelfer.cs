using Ersa.Platform.Infrastructure;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Common.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.Common
{
	[Export(typeof(INF_AktionsWiederholungsHelfer))]
	public class EDC_AktionsWiederholungsHelfer : INF_AktionsWiederholungsHelfer
	{
		[Import]
		public INF_InteractionController PRO_edcInteractionController
		{
			get;
			set;
		}

		[Import]
		public INF_LokalisierungsDienst PRO_edcLokalisierungsDienst
		{
			get;
			set;
		}

		public bool FUN_blnFuehreWiederholebareDateiOperationAus(Action i_delAction, string i_strDialogTitel)
		{
			try
			{
				i_delAction();
				return true;
			}
			catch (ArgumentNullException i_fdcException)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException, i_strDialogTitel, "13_621");
			}
			catch (ArgumentException i_fdcException2)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException2, i_strDialogTitel, "13_616");
			}
			catch (FileNotFoundException i_fdcException3)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException3, i_strDialogTitel, "4_4710");
			}
			catch (DirectoryNotFoundException i_fdcException4)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException4, i_strDialogTitel, "10_1152");
			}
			catch (PathTooLongException i_fdcException5)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException5, i_strDialogTitel, "13_617");
			}
			catch (NotSupportedException i_fdcException6)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException6, i_strDialogTitel, "13_618");
			}
			catch (SecurityException i_fdcException7)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException7, i_strDialogTitel, "13_41");
			}
			catch (UnauthorizedAccessException i_fdcException8)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException8, i_strDialogTitel, "13_41");
			}
			catch (IOException i_fdcException9)
			{
				return FUN_fdcBehandleDateiOperationException(i_delAction, i_fdcException9, i_strDialogTitel, "13_619");
			}
		}

		public async Task<bool> FUN_fdcFuehreWiederholebareDateiOperationAusAsync(Func<Task> i_delAktionsErsteller, string i_strDialogTitel)
		{
			Exception fdcFehler;
			string strFehlerText;
			try
			{
				await i_delAktionsErsteller().ConfigureAwait(continueOnCapturedContext: true);
				return true;
			}
			catch (ArgumentNullException ex)
			{
				fdcFehler = ex;
				strFehlerText = "13_621";
			}
			catch (ArgumentException ex2)
			{
				fdcFehler = ex2;
				strFehlerText = "13_616";
			}
			catch (FileNotFoundException ex3)
			{
				fdcFehler = ex3;
				strFehlerText = "4_4710";
			}
			catch (DirectoryNotFoundException ex4)
			{
				fdcFehler = ex4;
				strFehlerText = "10_1152";
			}
			catch (PathTooLongException ex5)
			{
				fdcFehler = ex5;
				strFehlerText = "13_617";
			}
			catch (NotSupportedException ex6)
			{
				fdcFehler = ex6;
				strFehlerText = "13_618";
			}
			catch (SecurityException ex7)
			{
				fdcFehler = ex7;
				strFehlerText = "13_41";
			}
			catch (UnauthorizedAccessException ex8)
			{
				fdcFehler = ex8;
				strFehlerText = "13_41";
			}
			catch (IOException ex9)
			{
				fdcFehler = ex9;
				strFehlerText = "13_619";
			}
			return await FUN_fdcBehandleDateiOperationExceptionAsync(i_delAktionsErsteller, fdcFehler, i_strDialogTitel, strFehlerText).ConfigureAwait(continueOnCapturedContext: true);
		}

		private bool FUN_fdcBehandleDateiOperationException(Action i_delAction, Exception i_fdcException, string i_strTitel, string i_strText)
		{
			ENUM_Eingabe enmErgebnis = ENUM_Eingabe.Undefiniert;
			EDC_Dispatch.SUB_AktionStarten(delegate
			{
				enmErgebnis = FUN_enmZeigeWiederholenAbbrechenDialog(i_strTitel, i_strText, i_fdcException);
			});
			if (enmErgebnis == ENUM_Eingabe.Ok)
			{
				return FUN_blnFuehreWiederholebareDateiOperationAus(i_delAction, i_strTitel);
			}
			return false;
		}

		private async Task<bool> FUN_fdcBehandleDateiOperationExceptionAsync(Func<Task> i_delAktionsErsteller, Exception i_fdcException, string i_strTitel, string i_strText)
		{
			ENUM_Eingabe enmErgebnis = ENUM_Eingabe.Undefiniert;
			await EDC_Dispatch.FUN_fdcAwaitableAktion(delegate
			{
				enmErgebnis = FUN_enmZeigeWiederholenAbbrechenDialog(i_strTitel, i_strText, i_fdcException);
			}).ConfigureAwait(continueOnCapturedContext: true);
			if (enmErgebnis == ENUM_Eingabe.Ok)
			{
				return await FUN_fdcFuehreWiederholebareDateiOperationAusAsync(i_delAktionsErsteller, i_strTitel).ConfigureAwait(continueOnCapturedContext: true);
			}
			return false;
		}

		private ENUM_Eingabe FUN_enmZeigeWiederholenAbbrechenDialog(string i_strTitel, string i_strText, Exception i_fdcException)
		{
			return PRO_edcInteractionController.FUN_enmOkAbbrechenAbfrageAnzeigen(PRO_edcLokalisierungsDienst.FUN_strText(i_strTitel), string.Format("{0} {1} {1} {2}", PRO_edcLokalisierungsDienst.FUN_strText(i_strText), Environment.NewLine, i_fdcException.Message), PRO_edcLokalisierungsDienst.FUN_strText("13_40"));
		}
	}
}
