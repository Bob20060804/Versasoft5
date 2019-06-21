using Ersa.Global.Controls.NavigationsTabControl;
using Ersa.Global.Mvvm.Commands;
using Ersa.Platform.UI.Common.TabItem;
using Ersa.Platform.UI.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_AllgemeineEinstellungenViewModel : EDC_NavigationsViewModelMitTabs
	{
		[ImportMany("AllgemeineEinstellungenTabContainer", AllowRecomposition = true)]
		public override IEnumerable<IEnumerable<EDC_TabItemSpezifikation>> PRO_enuTabItemSpezifikationen
		{
			get
			{
				return base.PRO_enuTabItemSpezifikationen;
			}
			set
			{
				base.PRO_enuTabItemSpezifikationen = value;
			}
		}

		public AsyncCommand<EDC_NavigationsElementAenderungsDaten> PRO_cmdEinstellungsTabGeandert
		{
			get;
			private set;
		}

		[ImportingConstructor]
		public EDC_AllgemeineEinstellungenViewModel()
		{
			PRO_cmdEinstellungsTabGeandert = new AsyncCommand<EDC_NavigationsElementAenderungsDaten>(base.FUN_fdcTabGeandertAsync);
		}
	}
}
