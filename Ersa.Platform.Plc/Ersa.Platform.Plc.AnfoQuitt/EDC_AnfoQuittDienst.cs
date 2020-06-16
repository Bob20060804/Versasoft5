using Ersa.Platform.Common.Model;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Plc.Exceptions;
using Ersa.Platform.Plc.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ersa.Platform.Plc.AnfoQuitt
{
    /// <summary>
    /// Request Service
    /// </summary>
	[Export(typeof(INF_AnfoQuittDienst))]
	public class EDC_AnfoQuittDienst : INF_AnfoQuittDienst
	{
        /// <summary>
        /// 重新尝试的最大请求数
        /// Maximum tries request reset
        /// </summary>
		private const int mC_i32MaximaleVersucheAnfoRuecksetzung = 5;

        /// <summary>
        /// 通讯服务器接口
        /// </summary>
		private readonly INF_KommunikationsDienst m_edcKommunikationsDienst;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i_edcKommunikationsDienst"></param>
		[ImportingConstructor]
		public EDC_AnfoQuittDienst(INF_KommunikationsDienst i_edcKommunikationsDienst)
		{
			m_edcKommunikationsDienst = i_edcKommunikationsDienst;
		}

        /// <summary>
        /// 确认request
        /// Request acknowledge handle Async
        /// </summary>
        /// <param name="i_edcToggle"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		public Task FUN_fdcAnfoSetzenUndQuittierenBehandelnAsync(EDC_ToggleWert i_edcToggle, CancellationToken i_fdcCancellationToken = default(CancellationToken))
		{
			if (i_edcToggle.PRO_blnToggleQuittVorgangAktiv)
			{
				return Task.CompletedTask;
			}
			i_edcToggle.PRO_blnToggleQuittVorgangAktiv = true;
			EDC_Dispatch.SUB_AktionStarten(delegate
			{
				PropertyChangedEventManager.AddHandler(i_edcToggle, SUB_ToggleQuittPropertyChanged, "PRO_blnToggleQuittAenderung");
			}, DispatcherPriority.Send);
			return Task.Run(async delegate
			{
				await FUN_fdcTestUndInitAnfoWertAsync(i_edcToggle, i_fdcCancellationToken).ConfigureAwait(continueOnCapturedContext: true);
				SUB_BooleanParameterWertSchreiben(i_edcToggle.PRO_edcToggle, i_blnWert: true);
			}, i_fdcCancellationToken);
		}

        /// <summary>
        /// Begin Set and Acknowledge Treat Async
        /// </summary>
        /// <param name="i_edcToggle"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		public Task FUN_fdcAnfoSetzenUndQuittierenBehandelnAsync(EDC_ToggleZustandWert i_edcToggle, CancellationToken i_fdcCancellationToken = default(CancellationToken))
		{
			if (i_edcToggle.PRO_blnToggleQuittVorgangAktiv)
			{
				return Task.CompletedTask;
			}
			i_edcToggle.PRO_blnToggleQuittVorgangAktiv = true;
			EDC_Dispatch.SUB_AktionStarten(delegate
			{
				PropertyChangedEventManager.AddHandler(i_edcToggle, SUB_ToggleZustandQuittPropertyChanged, "PRO_blnToggleQuittAenderung");
			}, DispatcherPriority.Send);
			return Task.Run(async delegate
			{
				await FUN_fdcTestUndInitAnfoWertAsync(i_edcToggle, i_fdcCancellationToken).ConfigureAwait(continueOnCapturedContext: true);
				SUB_BooleanParameterWertSchreiben(i_edcToggle.PRO_edcToggle, i_blnWert: true);
			}, i_fdcCancellationToken);
		}

        /// <summary>
        /// 
        /// Toggle Quit Property Changed
        /// </summary>
        /// <param name="i_objSender"></param>
        /// <param name="i_objArgs"></param>
		private void SUB_ToggleQuittPropertyChanged(object i_objSender, PropertyChangedEventArgs i_objArgs)
		{
			EDC_ToggleWert eDC_ToggleWert = i_objSender as EDC_ToggleWert;
			if (eDC_ToggleWert != null && eDC_ToggleWert.PRO_edcToggleQuitt.PRO_blnWert)
			{
				eDC_ToggleWert.PRO_blnToggleQuittVorgangAktiv = false;
				SUB_RemoveHandler(eDC_ToggleWert);
				SUB_BooleanParameterWertSchreiben(eDC_ToggleWert.PRO_edcToggle, i_blnWert: false);
			}
		}

		private void SUB_ToggleZustandQuittPropertyChanged(object i_objSender, PropertyChangedEventArgs i_objArgs)
		{
			EDC_ToggleZustandWert eDC_ToggleZustandWert = i_objSender as EDC_ToggleZustandWert;
			if (eDC_ToggleZustandWert != null && eDC_ToggleZustandWert.PRO_blnToggleQuit)
			{
				eDC_ToggleZustandWert.PRO_blnToggleQuittVorgangAktiv = false;
				SUB_RemoveHandler(eDC_ToggleZustandWert);
				SUB_BooleanParameterWertSchreiben(eDC_ToggleZustandWert.PRO_edcToggle, i_blnWert: false);
			}
		}

		private void SUB_RemoveHandler(EDC_ToggleWert i_edcToggle)
		{
			EDC_Dispatch.SUB_AktionStarten(delegate
			{
				PropertyChangedEventManager.RemoveHandler(i_edcToggle, SUB_ToggleQuittPropertyChanged, "PRO_blnToggleQuittAenderung");
			});
		}

		private void SUB_RemoveHandler(EDC_ToggleZustandWert i_edcToggle)
		{
			EDC_Dispatch.SUB_AktionStarten(delegate
			{
				PropertyChangedEventManager.RemoveHandler(i_edcToggle, SUB_ToggleZustandQuittPropertyChanged, "PRO_blnToggleQuittAenderung");
			});
		}

        /// <summary>
        /// 测试并初始化Request
        /// Test And Init Request Value Async
        /// </summary>
        /// <param name="i_edcToggleWert"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		private async Task FUN_fdcTestUndInitAnfoWertAsync(EDC_ToggleWert i_edcToggleWert, CancellationToken i_fdcCancellationToken)
		{
            // 读
			m_edcKommunikationsDienst.SUB_WertLesen(i_edcToggleWert.PRO_edcToggle);
			if (i_edcToggleWert.PRO_edcToggle.PRO_blnWert && !(await FUN_fdcAnfoWertZurueckSetzenAsync(i_edcToggleWert.PRO_edcToggle, i_fdcCancellationToken)))
			{
				i_edcToggleWert.PRO_blnToggleQuittVorgangAktiv = false;
				throw new EDC_AnfoQuittException($"Anfo-Parameter cannot be set to false. Parameter: {i_edcToggleWert.PRO_edcToggle.PRO_strAdresse}");
			}
		}
        // 测试并初始化Request
        private async Task FUN_fdcTestUndInitAnfoWertAsync(EDC_ToggleZustandWert i_edcToggleWert, CancellationToken i_fdcCancellationToken)
		{
			m_edcKommunikationsDienst.SUB_WertLesen(i_edcToggleWert.PRO_edcToggle);
			if (i_edcToggleWert.PRO_edcToggle.PRO_blnWert && !(await FUN_fdcAnfoWertZurueckSetzenAsync(i_edcToggleWert.PRO_edcToggle, i_fdcCancellationToken)))
			{
				i_edcToggleWert.PRO_blnToggleQuittVorgangAktiv = false;
				throw new EDC_AnfoQuittException($"Anfo-Parameter cannot be set to false. Parameter: {i_edcToggleWert.PRO_edcToggle.PRO_strAdresse}");
			}
		}

        /// <summary>
        /// Reset value
        /// </summary>
        /// <param name="i_edcAnfoParameter"></param>
        /// <param name="i_fdcCancellationToken"></param>
        /// <returns></returns>
		private async Task<bool> FUN_fdcAnfoWertZurueckSetzenAsync(EDC_BooleanParameter i_edcAnfoParameter, CancellationToken i_fdcCancellationToken)
		{
			int i32AnzahlVersuche = 0;
			SUB_BooleanParameterWertSchreiben(i_edcAnfoParameter, i_blnWert: false);
			do
			{
				await Task.Delay(TimeSpan.FromMilliseconds(200.0), i_fdcCancellationToken);
				m_edcKommunikationsDienst.SUB_WertLesen(i_edcAnfoParameter);
				i32AnzahlVersuche++;
			}
			while (i_edcAnfoParameter.PRO_blnWert && i32AnzahlVersuche <= 5);
			return !i_edcAnfoParameter.PRO_blnWert;
		}

        /// <summary>
        /// write value Boolean parameter 
        /// </summary>
        /// <param name="i_edcParameter"></param>
        /// <param name="i_blnWert"></param>
		private void SUB_BooleanParameterWertSchreiben(EDC_BooleanParameter i_edcParameter, bool i_blnWert)
		{
			i_edcParameter.PRO_blnWert = i_blnWert;
			m_edcKommunikationsDienst.SUB_WertSchreiben(i_edcParameter);
		}
	}
}
