using System;
using System.Xml;

namespace BR.AN.PviServices
{
	public class TaskClass : IDisposable
	{
		private string propName;

		internal bool propDisposed;

		internal object propUserData;

		private TaskClassType propNumber;

		private ProgramState propState;

		public string Name => propName;

		public TaskClassType Type => propNumber;

		public ProgramState State => propState;

		public object UserData
		{
			get
			{
				return propUserData;
			}
			set
			{
				propUserData = value;
			}
		}

		public event DisposeEventHandler Disposing;

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public void Dispose()
		{
			if (!propDisposed)
			{
				Dispose(disposing: true, removeFromCollection: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing, bool removeFromCollection)
		{
			if (!propDisposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					propDisposed = true;
					propName = null;
					propUserData = null;
				}
			}
		}

		internal TaskClass(APIFC_TkInfoRes taskClassInfo)
		{
			propDisposed = false;
			propName = taskClassInfo.name.ToString();
			propNumber = taskClassInfo.number;
			propState = taskClassInfo.state;
		}

		internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
		{
			writer.WriteStartElement("TaskClass");
			writer.WriteAttributeString("Name", propName.ToString());
			if (propState != ProgramState.Running)
			{
				writer.WriteAttributeString("State", propState.ToString());
			}
			writer.WriteAttributeString("Type", propNumber.ToString());
			writer.WriteEndElement();
			return 0;
		}

		internal static int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, ref APIFC_TkInfoRes taskClassInfo)
		{
			string text = "";
			text = reader.GetAttribute("State");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "idle":
					taskClassInfo.state = ProgramState.Idle;
					break;
				case "nonexistent":
					taskClassInfo.state = ProgramState.NonExistent;
					break;
				case "resetting":
					taskClassInfo.state = ProgramState.Resetting;
					break;
				case "resuming":
					taskClassInfo.state = ProgramState.Resuming;
					break;
				case "running":
					taskClassInfo.state = ProgramState.Running;
					break;
				case "starting":
					taskClassInfo.state = ProgramState.Starting;
					break;
				case "stopped":
					taskClassInfo.state = ProgramState.Stopped;
					break;
				case "stopping":
					taskClassInfo.state = ProgramState.Stopping;
					break;
				case "unrunnable":
					taskClassInfo.state = ProgramState.Unrunnable;
					break;
				}
			}
			text = reader.GetAttribute("Type");
			if (text != null && text.Length > 0)
			{
				switch (text.ToLower())
				{
				case "cyclic1":
					taskClassInfo.number = TaskClassType.Cyclic1;
					break;
				case "cyclic2":
					taskClassInfo.number = TaskClassType.Cyclic2;
					break;
				case "cyclic3":
					taskClassInfo.number = TaskClassType.Cyclic3;
					break;
				case "cyclic4":
					taskClassInfo.number = TaskClassType.Cyclic4;
					break;
				case "cyclic5":
					taskClassInfo.number = TaskClassType.Cyclic5;
					break;
				case "cyclic6":
					taskClassInfo.number = TaskClassType.Cyclic6;
					break;
				case "cyclic7":
					taskClassInfo.number = TaskClassType.Cyclic7;
					break;
				case "cyclic8":
					taskClassInfo.number = TaskClassType.Cyclic8;
					break;
				case "timer1":
					taskClassInfo.number = TaskClassType.Timer1;
					break;
				case "timer2":
					taskClassInfo.number = TaskClassType.Timer2;
					break;
				case "timer3":
					taskClassInfo.number = TaskClassType.Timer3;
					break;
				case "timer4":
					taskClassInfo.number = TaskClassType.Timer4;
					break;
				case "exception":
					taskClassInfo.number = TaskClassType.Exception;
					break;
				case "interrupt":
					taskClassInfo.number = TaskClassType.Interrupt;
					break;
				case "notvalid":
					taskClassInfo.number = TaskClassType.NotValid;
					break;
				}
			}
			text = "";
			text = reader.GetAttribute("Name");
			if (text != null && text.Length > 0)
			{
				taskClassInfo.name = text;
			}
			reader.Read();
			return 0;
		}
	}
}
