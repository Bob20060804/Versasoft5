using Ersa.Platform.Common.Model;
using Ersa.Platform.Plc.Exceptions;
using Ersa.Platform.Plc.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc.AnfoQuitt
{
    /// <summary>
    /// 
    /// Request Quit Service
    /// </summary>
	[Export(typeof(INF_EnumAnfoQuittDienst))]
	public class EDC_EnumAnfoQuittDienst : INF_EnumAnfoQuittDienst
	{
		private const int mC_i32MaximaleVersucheAnfoRuecksetzung = 5;

		private readonly Inf_CommunicationService m_edcKommunikationsDienst;

		[ImportingConstructor]
		public EDC_EnumAnfoQuittDienst(Inf_CommunicationService i_edcKommunikationsDienst)
		{
			m_edcKommunikationsDienst = i_edcKommunikationsDienst;
		}

		public Task FUN_fdcAnfoSetzenUndQuittierenBehandelnAsync<TEnum>(EDC_ToggleEnumWert<TEnum> i_edcToggle, CancellationToken i_fdcCancellationToken = default(CancellationToken)) where TEnum : struct, IConvertible, IComparable, IFormattable
		{
			PropertyChangedEventManager.AddHandler(i_edcToggle, this.SUB_ToggleQuittPropertyChanged<TEnum>, "PRO_blnToggleQuittAenderung");
			return Task.Run(async delegate
			{
				await FUN_fdcTestUndInitAnfoWertAsync(i_edcToggle, i_fdcCancellationToken);
				SUB_EnumParameterWertSchreiben(i_edcToggle.PRO_edcAnfoParameter, i_edcToggle.PRO_enmAnfoSetzenWert);
			}, i_fdcCancellationToken);
		}

		private void SUB_ToggleQuittPropertyChanged<TEnum>(object i_objSender, PropertyChangedEventArgs i_objArgs) where TEnum : struct, IConvertible, IComparable, IFormattable
		{
			EDC_ToggleEnumWert<TEnum> eDC_ToggleEnumWert = i_objSender as EDC_ToggleEnumWert<TEnum>;
			if (eDC_ToggleEnumWert != null && eDC_ToggleEnumWert.PRO_edcQuittParameter.PRO_enmWert.Equals(eDC_ToggleEnumWert.PRO_enmQuittierWert))
			{
				SUB_RemoveHandler(eDC_ToggleEnumWert);
				SUB_EnumParameterWertSchreiben(eDC_ToggleEnumWert.PRO_edcAnfoParameter, eDC_ToggleEnumWert.PRO_enmAnfoZurueckSetzenWert);
			}
		}

		private void SUB_RemoveHandler<TEnum>(EDC_ToggleEnumWert<TEnum> i_edcToggle) where TEnum : struct, IConvertible, IComparable, IFormattable
		{
			PropertyChangedEventManager.RemoveHandler(i_edcToggle, this.SUB_ToggleQuittPropertyChanged<TEnum>, "PRO_blnToggleQuittAenderung");
		}

		private async Task FUN_fdcTestUndInitAnfoWertAsync<TEnum>(EDC_ToggleEnumWert<TEnum> i_edcToggle, CancellationToken i_fdcCancellationToken) where TEnum : struct, IConvertible, IComparable, IFormattable
		{
			m_edcKommunikationsDienst.Sub_ReadValue(i_edcToggle.PRO_edcAnfoParameter);
			if (i_edcToggle.PRO_edcAnfoParameter.PRO_enmWert.Equals(i_edcToggle.PRO_enmAnfoSetzenWert) && !(await FUN_fdcAnfoWertZurueckSetzenAsync(i_edcToggle, i_fdcCancellationToken)))
			{
				throw new EDC_AnfoQuittException($"Anfo-Parameter cannot be set to false. Parameter: {i_edcToggle.PRO_edcAnfoParameter.PRO_strAdresse}");
			}
		}

		private async Task<bool> FUN_fdcAnfoWertZurueckSetzenAsync<TEnum>(EDC_ToggleEnumWert<TEnum> i_edcToggle, CancellationToken i_fdcCancellationToken) where TEnum : struct, IConvertible, IComparable, IFormattable
		{
			int i32AnzahlVersuche = 0;
			SUB_EnumParameterWertSchreiben(i_edcToggle.PRO_edcAnfoParameter, i_edcToggle.PRO_enmAnfoZurueckSetzenWert);
			do
			{
				await Task.Delay(TimeSpan.FromMilliseconds(200.0), i_fdcCancellationToken);
				m_edcKommunikationsDienst.Sub_ReadValue(i_edcToggle.PRO_edcAnfoParameter);
				i32AnzahlVersuche++;
			}
			while (i_edcToggle.PRO_edcAnfoParameter.PRO_enmWert.Equals(i_edcToggle.PRO_enmAnfoSetzenWert) && i32AnzahlVersuche <= 5);
			return !i_edcToggle.PRO_edcAnfoParameter.PRO_enmWert.Equals(i_edcToggle.PRO_enmAnfoSetzenWert);
		}

		private void SUB_EnumParameterWertSchreiben<TEnum>(EDC_EnumParameter<TEnum> i_edcParameter, TEnum? i_enmWert) where TEnum : struct, IConvertible, IComparable, IFormattable
		{
			i_edcParameter.PRO_enmWert = i_enmWert;
			m_edcKommunikationsDienst.SUB_WertSchreiben(i_edcParameter);
		}
	}
}
