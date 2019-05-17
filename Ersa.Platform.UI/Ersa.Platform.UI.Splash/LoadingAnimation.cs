using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Ersa.Platform.UI.Splash
{
	public class LoadingAnimation : UserControl, IComponentConnector
	{
		internal BeginStoryboard ProgressAnimationBeginStoryboard;

		internal EDU_AnimationsBlock Block;

		internal EDU_AnimationsBlock Block1;

		internal EDU_AnimationsBlock Block2;

		internal EDU_AnimationsBlock Block3;

		internal EDU_AnimationsBlock Block4;

		internal EDU_AnimationsBlock Block5;

		internal EDU_AnimationsBlock Block6;

		internal EDU_AnimationsBlock Block7;

		internal EDU_AnimationsBlock Block8;

		internal EDU_AnimationsBlock Block9;

		internal EDU_AnimationsBlock Block10;

		internal EDU_AnimationsBlock Block11;

		internal EDU_AnimationsBlock Block12;

		internal EDU_AnimationsBlock Block13;

		internal EDU_AnimationsBlock Block14;

		internal EDU_AnimationsBlock Block15;

		internal EDU_AnimationsBlock Block16;

		internal EDU_AnimationsBlock Block17;

		private bool _contentLoaded;

		public LoadingAnimation()
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
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/splash/edu_splashladenanimation.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				ProgressAnimationBeginStoryboard = (BeginStoryboard)target;
				break;
			case 2:
				Block = (EDU_AnimationsBlock)target;
				break;
			case 3:
				Block1 = (EDU_AnimationsBlock)target;
				break;
			case 4:
				Block2 = (EDU_AnimationsBlock)target;
				break;
			case 5:
				Block3 = (EDU_AnimationsBlock)target;
				break;
			case 6:
				Block4 = (EDU_AnimationsBlock)target;
				break;
			case 7:
				Block5 = (EDU_AnimationsBlock)target;
				break;
			case 8:
				Block6 = (EDU_AnimationsBlock)target;
				break;
			case 9:
				Block7 = (EDU_AnimationsBlock)target;
				break;
			case 10:
				Block8 = (EDU_AnimationsBlock)target;
				break;
			case 11:
				Block9 = (EDU_AnimationsBlock)target;
				break;
			case 12:
				Block10 = (EDU_AnimationsBlock)target;
				break;
			case 13:
				Block11 = (EDU_AnimationsBlock)target;
				break;
			case 14:
				Block12 = (EDU_AnimationsBlock)target;
				break;
			case 15:
				Block13 = (EDU_AnimationsBlock)target;
				break;
			case 16:
				Block14 = (EDU_AnimationsBlock)target;
				break;
			case 17:
				Block15 = (EDU_AnimationsBlock)target;
				break;
			case 18:
				Block16 = (EDU_AnimationsBlock)target;
				break;
			case 19:
				Block17 = (EDU_AnimationsBlock)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
