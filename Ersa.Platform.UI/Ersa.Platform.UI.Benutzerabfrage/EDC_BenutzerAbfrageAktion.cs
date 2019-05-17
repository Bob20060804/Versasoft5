using Prism.Interactivity.InteractionRequest;
using System.Windows;
using System.Windows.Interactivity;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public abstract class EDC_BenutzerAbfrageAktion<TAbfrage, TEingabe, TDialog> : TriggerAction<Window> where TAbfrage : EDC_BenutzerAbfrage<TEingabe> where TDialog : Window
	{
		protected abstract TDialog FUN_edcDialogErzeugen(TAbfrage i_edcKontext);

		protected abstract TEingabe FUN_edcErgebnisErmitteln(bool? i_blnDialogErgebnis, TDialog i_edcDialogFenster);

		protected override void Invoke(object i_objParameter)
		{
			InteractionRequestedEventArgs interactionRequestedEventArgs = i_objParameter as InteractionRequestedEventArgs;
			if (interactionRequestedEventArgs != null && interactionRequestedEventArgs.Context is TAbfrage)
			{
				TAbfrage val = (TAbfrage)interactionRequestedEventArgs.Context;
				TDialog val2 = FUN_edcDialogErzeugen(val);
				bool? i_blnDialogErgebnis = val2.ShowDialog();
				val.PRO_edcErgebnis = FUN_edcErgebnisErmitteln(i_blnDialogErgebnis, val2);
				interactionRequestedEventArgs.Callback();
			}
		}
	}
}
