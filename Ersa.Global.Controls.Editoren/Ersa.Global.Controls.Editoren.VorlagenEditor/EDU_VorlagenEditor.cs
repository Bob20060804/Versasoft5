using Ersa.Global.Controls.Editoren.VorlagenEditor.DragAndDrop;
using Ersa.Global.Controls.Editoren.VorlagenEditor.VorlageElemente;
using Ersa.Global.Controls.Extensions;
using GongSolutions.Wpf.DragDrop;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Editoren.VorlagenEditor
{
	public class EDU_VorlagenEditor : UserControl, IComponentConnector
	{
		public static readonly DependencyProperty PRO_enuAblaeufeProperty = DependencyProperty.Register("PRO_enuAblaeufe", typeof(IEnumerable<EDC_AblaufSchritte>), typeof(EDU_VorlagenEditor), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_enuVorlagenProperty = DependencyProperty.Register("PRO_enuVorlagen", typeof(IEnumerable<EDC_VorlageElement>), typeof(EDU_VorlagenEditor), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_blnIstReadonlyProperty = DependencyProperty.Register("PRO_blnIstReadonly", typeof(bool), typeof(EDU_VorlagenEditor), new PropertyMetadata(false));

		public static readonly DependencyProperty PRO_fdcAblaufListeDragHandlerProperty = DependencyProperty.Register("PRO_fdcAblaufListeDragHandler", typeof(IDragSource), typeof(EDU_VorlagenEditor), new PropertyMetadata(new EDC_DefaultAblaufListeDragHandler()));

		public static readonly DependencyProperty PRO_fdcAblaufListeDropHandlerProperty = DependencyProperty.Register("PRO_fdcAblaufListeDropHandler", typeof(IDropTarget), typeof(EDU_VorlagenEditor), new PropertyMetadata(new EDC_DefaultAblaufListeDropHandler()));

		public static readonly DependencyProperty PRO_fdcVorlageListeDropHandlerProperty = DependencyProperty.Register("PRO_fdcVorlageListeDropHandler", typeof(IDropTarget), typeof(EDU_VorlagenEditor), new PropertyMetadata(new EDC_DefaultVorlageListeDropHandler()));

		public static readonly DependencyProperty PRO_fdcAblaufListeContextMenuProperty = DependencyProperty.Register("PRO_fdcAblaufListeContextMenu", typeof(ContextMenu), typeof(EDU_VorlagenEditor), new PropertyMetadata((object)null));

		private bool _contentLoaded;

		public IEnumerable<EDC_AblaufSchritte> PRO_enuAblaeufe
		{
			get
			{
				return (IEnumerable<EDC_AblaufSchritte>)GetValue(PRO_enuAblaeufeProperty);
			}
			set
			{
				SetValue(PRO_enuAblaeufeProperty, value);
			}
		}

		public IEnumerable<EDC_VorlageElement> PRO_enuVorlagen
		{
			get
			{
				return (IEnumerable<EDC_VorlageElement>)GetValue(PRO_enuVorlagenProperty);
			}
			set
			{
				SetValue(PRO_enuVorlagenProperty, value);
			}
		}

		public bool PRO_blnIstReadonly
		{
			get
			{
				return (bool)GetValue(PRO_blnIstReadonlyProperty);
			}
			set
			{
				SetValue(PRO_blnIstReadonlyProperty, value);
			}
		}

		public IDragSource PRO_fdcAblaufListeDragHandler
		{
			get
			{
				return (IDragSource)GetValue(PRO_fdcAblaufListeDragHandlerProperty);
			}
			set
			{
				SetValue(PRO_fdcAblaufListeDragHandlerProperty, value);
			}
		}

		public IDropTarget PRO_fdcAblaufListeDropHandler
		{
			get
			{
				return (IDropTarget)GetValue(PRO_fdcAblaufListeDropHandlerProperty);
			}
			set
			{
				SetValue(PRO_fdcAblaufListeDropHandlerProperty, value);
			}
		}

		public IDropTarget PRO_fdcVorlageListeDropHandler
		{
			get
			{
				return (IDropTarget)GetValue(PRO_fdcVorlageListeDropHandlerProperty);
			}
			set
			{
				SetValue(PRO_fdcVorlageListeDropHandlerProperty, value);
			}
		}

		public ContextMenu PRO_fdcAblaufListeContextMenu
		{
			get
			{
				return (ContextMenu)GetValue(PRO_fdcAblaufListeContextMenuProperty);
			}
			set
			{
				SetValue(PRO_fdcAblaufListeContextMenuProperty, value);
			}
		}

		public EDU_VorlagenEditor()
		{
			InitializeComponent();
		}

		private void SUB_PreviewKeyDown(object i_objSender, KeyEventArgs i_fdcArgs)
		{
			if (i_fdcArgs.IsDown && !PRO_blnIstReadonly && ((i_fdcArgs.Key == Key.System) ? i_fdcArgs.SystemKey : i_fdcArgs.Key) == Key.Delete)
			{
				EDC_RoutedCommands.ms_cmdAblaufschrittElementeEntfernen.SUB_Execute(null, this);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls.Editoren;component/vorlageneditor/edu_vorlageneditor.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				((Grid)target).PreviewKeyDown += SUB_PreviewKeyDown;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
