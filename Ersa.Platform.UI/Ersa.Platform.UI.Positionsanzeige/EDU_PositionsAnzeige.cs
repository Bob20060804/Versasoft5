using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Positionsanzeige
{
	public class EDU_PositionsAnzeige : UserControl, IComponentConnector
	{
		public static readonly DependencyProperty PRO_edcModellProperty = DependencyProperty.Register("PRO_edcModell", typeof(EDC_PositionsAnzeigeModell), typeof(EDU_PositionsAnzeige));

		public static readonly DependencyProperty PRO_fdcElementStyleProperty = DependencyProperty.Register("PRO_fdcElementStyle", typeof(Style), typeof(EDU_PositionsAnzeige));

		private bool _contentLoaded;

		public EDC_PositionsAnzeigeModell PRO_edcModell
		{
			get
			{
				return (EDC_PositionsAnzeigeModell)GetValue(PRO_edcModellProperty);
			}
			set
			{
				SetValue(PRO_edcModellProperty, value);
			}
		}

		public Style PRO_fdcElementStyle
		{
			get
			{
				return (Style)GetValue(PRO_fdcElementStyleProperty);
			}
			set
			{
				SetValue(PRO_fdcElementStyleProperty, value);
			}
		}

		public EDU_PositionsAnzeige()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/positionsanzeige/edu_positionsanzeige.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
}
