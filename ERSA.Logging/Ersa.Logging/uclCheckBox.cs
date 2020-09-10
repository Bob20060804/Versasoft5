using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Ersa.Logging
{
	[DesignerGenerated]
	internal class uclCheckBox : UserControl
	{
		public delegate void EVT_intCharSetChangeEventHandler();

		public delegate void EVT_strToolTipTextChangeEventHandler();

		public delegate void EVT_blnEnabledChangeEventHandler();

		public delegate void EVT_CheckStateChangedEventHandler();

		public delegate void EVT_strCaptionChangeEventHandler();

		public delegate void EVT_sdcHintergrundFarbeGeaendertEventHandler();

		private IContainer components;

		public ToolTip ToolTip1;

		[AccessedThroughProperty("chkBox")]
		private CheckBox _chkBox;

		private EVT_intCharSetChangeEventHandler EVT_intCharSetChangeEvent;

		private EVT_strToolTipTextChangeEventHandler EVT_strToolTipTextChangeEvent;

		private EVT_blnEnabledChangeEventHandler EVT_blnEnabledChangeEvent;

		private EVT_CheckStateChangedEventHandler EVT_CheckStateChangedEvent;

		private EVT_strCaptionChangeEventHandler EVT_strCaptionChangeEvent;

		private EVT_sdcHintergrundFarbeGeaendertEventHandler EVT_sdcHintergrundFarbeGeaendertEvent;

		internal virtual CheckBox chkBox
		{
			get
			{
				return _chkBox;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = chkBox_Click;
				if (_chkBox != null)
				{
					_chkBox.Click -= value2;
				}
				_chkBox = value;
				if (_chkBox != null)
				{
					_chkBox.Click += value2;
				}
			}
		}

		public short PRO_intCharSet
		{
			get
			{
				return chkBox.Font.GdiCharSet;
			}
			set
			{
				chkBox.Font = new Font(chkBox.Font.FontFamily, chkBox.Font.Size, chkBox.Font.Style, chkBox.Font.Unit, checked((byte)value));
				EVT_intCharSetChangeEvent?.Invoke();
			}
		}

		public string PRO_strBeschreibung
		{
			get
			{
				return chkBox.Text;
			}
			set
			{
				chkBox.Text = Strings.Trim(value);
				EVT_strCaptionChangeEvent?.Invoke();
			}
		}

		public CheckState PRO_intCheckState
		{
			get
			{
				return chkBox.CheckState;
			}
			set
			{
				chkBox.CheckState = value;
				EVT_CheckStateChangedEvent?.Invoke();
			}
		}

		public string PRO_strToolTip
		{
			get
			{
				return ToolTip1.GetToolTip(chkBox);
			}
			set
			{
				ToolTip1.SetToolTip(chkBox, Strings.Trim(value));
				EVT_strToolTipTextChangeEvent?.Invoke();
			}
		}

		public bool PRO_blnEnabled
		{
			get
			{
				return chkBox.Enabled;
			}
			set
			{
				chkBox.Enabled = value;
				EVT_blnEnabledChangeEvent?.Invoke();
			}
		}

		public Color PRO_sdcBackColor
		{
			get
			{
				return chkBox.BackColor;
			}
			set
			{
				chkBox.BackColor = value;
				EVT_sdcHintergrundFarbeGeaendertEvent?.Invoke();
			}
		}

		public event EVT_intCharSetChangeEventHandler EVT_intCharSetChange
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				EVT_intCharSetChangeEvent = (EVT_intCharSetChangeEventHandler)Delegate.Combine(EVT_intCharSetChangeEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				EVT_intCharSetChangeEvent = (EVT_intCharSetChangeEventHandler)Delegate.Remove(EVT_intCharSetChangeEvent, value);
			}
		}

		public event EVT_strToolTipTextChangeEventHandler EVT_strToolTipTextChange
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				EVT_strToolTipTextChangeEvent = (EVT_strToolTipTextChangeEventHandler)Delegate.Combine(EVT_strToolTipTextChangeEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				EVT_strToolTipTextChangeEvent = (EVT_strToolTipTextChangeEventHandler)Delegate.Remove(EVT_strToolTipTextChangeEvent, value);
			}
		}

		public event EVT_blnEnabledChangeEventHandler EVT_blnEnabledChange
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				EVT_blnEnabledChangeEvent = (EVT_blnEnabledChangeEventHandler)Delegate.Combine(EVT_blnEnabledChangeEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				EVT_blnEnabledChangeEvent = (EVT_blnEnabledChangeEventHandler)Delegate.Remove(EVT_blnEnabledChangeEvent, value);
			}
		}

		public event EVT_CheckStateChangedEventHandler EVT_CheckStateChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				EVT_CheckStateChangedEvent = (EVT_CheckStateChangedEventHandler)Delegate.Combine(EVT_CheckStateChangedEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				EVT_CheckStateChangedEvent = (EVT_CheckStateChangedEventHandler)Delegate.Remove(EVT_CheckStateChangedEvent, value);
			}
		}

		public event EVT_strCaptionChangeEventHandler EVT_strCaptionChange
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				EVT_strCaptionChangeEvent = (EVT_strCaptionChangeEventHandler)Delegate.Combine(EVT_strCaptionChangeEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				EVT_strCaptionChangeEvent = (EVT_strCaptionChangeEventHandler)Delegate.Remove(EVT_strCaptionChangeEvent, value);
			}
		}

		public event EVT_sdcHintergrundFarbeGeaendertEventHandler EVT_sdcHintergrundFarbeGeaendert
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				EVT_sdcHintergrundFarbeGeaendertEvent = (EVT_sdcHintergrundFarbeGeaendertEventHandler)Delegate.Combine(EVT_sdcHintergrundFarbeGeaendertEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				EVT_sdcHintergrundFarbeGeaendertEvent = (EVT_sdcHintergrundFarbeGeaendertEventHandler)Delegate.Remove(EVT_sdcHintergrundFarbeGeaendertEvent, value);
			}
		}

		[DebuggerNonUserCode]
		public uclCheckBox()
		{
			base.Resize += uclCheckBoxTouch_Resize;
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		protected override void Dispose(bool Disposing)
		{
			if (Disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(Disposing);
		}

		[System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			ToolTip1 = new System.Windows.Forms.ToolTip(components);
			chkBox = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			chkBox.BackColor = System.Drawing.Color.Transparent;
			chkBox.Cursor = System.Windows.Forms.Cursors.Default;
			chkBox.Font = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			chkBox.ForeColor = System.Drawing.SystemColors.ControlText;
			chkBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			System.Drawing.Point point2 = chkBox.Location = new System.Drawing.Point(4, 2);
			chkBox.Name = "chkBox";
			System.Windows.Forms.Padding padding2 = chkBox.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
			chkBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			System.Drawing.Size size2 = chkBox.Size = new System.Drawing.Size(160, 48);
			chkBox.TabIndex = 0;
			chkBox.Text = "CheckBoxText";
			chkBox.UseVisualStyleBackColor = false;
			AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			BackColor = System.Drawing.Color.Transparent;
			Controls.Add(chkBox);
			ForeColor = System.Drawing.SystemColors.ControlText;
			padding2 = (Margin = new System.Windows.Forms.Padding(8));
			Name = "uclCheckBox";
			size2 = (Size = new System.Drawing.Size(160, 48));
			ResumeLayout(performLayout: false);
		}

		private void chkBox_Click(object sender, EventArgs e)
		{
			OnClick(e);
		}

		private void uclCheckBoxTouch_Resize(object eventSender, EventArgs eventArgs)
		{
			int num = default(int);
			int num3 = default(int);
			try
			{
				IL_0000:
				ProjectData.ClearProjectError();
				num = 1;
				goto IL_0007;
				IL_0007:
				int num2 = 2;
				base.Height = 48;
				goto IL_0011;
				IL_0011:
				num2 = 3;
				chkBox.SetBounds(0, 0, base.Width, base.Height);
				goto IL_002c;
				IL_002c:
				num2 = 4;
				chkBox.Font = new Font("Arial", 12f);
				goto IL_0048;
				IL_0048:
				ProjectData.ClearProjectError();
				num = 0;
				goto end_IL_0000;
				IL_0051:
				int num4 = num3 + 1;
				num3 = 0;
				switch (num4)
				{
				case 6:
					goto end_IL_0000;
				case 1:
					goto IL_0000;
				case 2:
					goto IL_0007;
				case 3:
					goto IL_0011;
				case 4:
					goto IL_002c;
				case 5:
					goto IL_0048;
				default:
					goto IL_00ab;
				}
				IL_0079:
				num3 = num2;
				switch (num)
				{
				case 1:
					goto IL_0051;
				default:
					goto IL_00ab;
				}
				end_IL_0000:;
			}
			catch (Exception obj) /* when ((obj is Exception && num != 0) & (num3 == 0))*/
			{
				ProjectData.SetProjectError(obj);
				/*Error near IL_00a9: Could not find block for branch target IL_0079*/;
			}
			if (num3 != 0)
			{
				ProjectData.ClearProjectError();
			}
			return;
			IL_00ab:
			throw ProjectData.CreateProjectError(-2146828237);
		}
	}
}
