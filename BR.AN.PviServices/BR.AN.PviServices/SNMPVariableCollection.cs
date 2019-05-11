using System;
using System.Collections.Generic;

namespace BR.AN.PviServices
{
	public class SNMPVariableCollection : SNMPCollectionBase
	{
		private int propWriteErrors;

		private int propReadErrors;

		private bool propFilteredVariableList;

		public Variable this[string indexer]
		{
			get
			{
				return (Variable)propItems[indexer];
			}
		}

		public event ErrorEventHandler ValuesRead;

		public event ErrorEventHandler ValuesReadFiltered;

		public event ErrorEventHandler ValuesWritten;

		internal SNMPVariableCollection(string name, SNMPBase parentObj)
			: base(name, parentObj)
		{
			propWriteErrors = 0;
			propReadErrors = 0;
		}

		internal override void OnPviRead(int errorCode, PVIReadAccessTypes accessType, PVIDataStates dataState, IntPtr pData, uint dataLen, int option)
		{
			if (accessType == PVIReadAccessTypes.SNMPListLocalVariables)
			{
				if (errorCode != 0)
				{
					OnSearchCompleted(new ErrorEventArgs(base.Name, "", errorCode, base.Service.Language, Action.SNMPListLocalVariables));
				}
			}
			else
			{
				base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
			}
		}

		public override int Disconnect()
		{
			return Disconnect(synchronous: true);
		}

		public override int Disconnect(bool synchronous)
		{
			if (0 < base.Count)
			{
				foreach (Variable value in base.Values)
				{
					value.CancelRequest(silent: true);
					value.Unlink();
				}
			}
			return 0;
		}

		public override void Cleanup()
		{
			if (0 < base.Count)
			{
				foreach (Variable value in base.Values)
				{
					value.CancelRequest(silent: true);
					value.Unlink();
					value.Remove();
					value.Dispose();
				}
			}
			base.Cleanup();
		}

		public int ReadFiltered(List<string> filter)
		{
			propFilteredVariableList = true;
			return ReadInternal(filter);
		}

		private int ReadInternal(List<string> filter)
		{
			int num = 0;
			if (propRequesting)
			{
				return -1;
			}
			propReadErrors = 0;
			propRequestCount = 0;
			if (0 < base.Count)
			{
				foreach (Variable value in base.Values)
				{
					if (filter == null || filter.Contains(value.Name))
					{
						propRequestCount++;
						value.ValueRead += tmpVar_ValueRead;
						if (value.IsConnected)
						{
							num = value.ReadValueEx();
							if (num != 0)
							{
								value.ValueRead -= tmpVar_ValueRead;
								propRequestCount--;
								propReadErrors++;
								OnError(new ErrorEventArgs(base.Name, "", num, base.Service.Language, Action.VariableRead, value.Name));
							}
						}
						else
						{
							value.Requests |= Actions.GetValue;
							value.Connected += tmpVar_Connected;
							value.Connect();
						}
					}
				}
			}
			if (propRequestCount == 0)
			{
				propRequesting = false;
				OnValuesRead(new ErrorEventArgs(base.Name, "", propReadErrors, base.Service.Language, Action.VariablesValuesRead));
			}
			return num;
		}

		public int Read()
		{
			propFilteredVariableList = false;
			return ReadInternal(null);
		}

		private void tmpVar_Connected(object sender, PviEventArgs e)
		{
			((Variable)sender).Connected -= tmpVar_Connected;
		}

		private void tmpVar_ValueRead(object sender, PviEventArgs e)
		{
			((Variable)sender).ValueRead -= tmpVar_ValueRead;
			propRequestCount--;
			if (e.ErrorCode != 0)
			{
				propReadErrors++;
				OnError(new ErrorEventArgs(base.Name, "", e.ErrorCode, base.Service.Language, e.Action, ((Variable)sender).Name));
			}
			if (propRequestCount == 0)
			{
				propRequesting = false;
				OnValuesRead(new ErrorEventArgs(base.Name, "", propReadErrors, base.Service.Language, Action.VariablesValuesRead));
			}
		}

		public int Write()
		{
			int num = 0;
			if (propRequesting)
			{
				return -1;
			}
			propWriteErrors = 0;
			propRequestCount = 0;
			if (0 < base.Count)
			{
				foreach (Variable value in base.Values)
				{
					propRequestCount++;
					value.ValueWritten += tmpVar_ValueWritten;
					num = value.WriteValue();
					if (num != 0)
					{
						value.ValueWritten -= tmpVar_ValueWritten;
						propRequestCount--;
						propWriteErrors++;
						OnError(new ErrorEventArgs(base.Name, "", num, base.Service.Language, Action.VariableWrite, value.Name));
					}
				}
			}
			if (propRequestCount == 0)
			{
				OnValuesWritten(new ErrorEventArgs(base.Name, "", propWriteErrors, base.Service.Language, Action.VariablesValuesWrite));
			}
			return num;
		}

		private void tmpVar_ValueWritten(object sender, PviEventArgs e)
		{
			((Variable)sender).ValueWritten -= tmpVar_ValueWritten;
			propRequestCount--;
			if (e.ErrorCode != 0)
			{
				propWriteErrors++;
				OnError(new ErrorEventArgs(base.Name, "", e.ErrorCode, base.Service.Language, e.Action));
			}
			if (propRequestCount == 0)
			{
				propRequesting = false;
				OnValuesWritten(new ErrorEventArgs(base.Name, "", propWriteErrors, base.Service.Language, Action.VariablesValuesWrite));
			}
		}

		protected virtual void OnValuesRead(ErrorEventArgs e)
		{
			propRequesting = false;
			if (propFilteredVariableList)
			{
				if (this.ValuesReadFiltered != null)
				{
					this.ValuesReadFiltered(this, e);
				}
			}
			else if (this.ValuesRead != null)
			{
				this.ValuesRead(this, e);
			}
		}

		protected virtual void OnValuesWritten(ErrorEventArgs e)
		{
			propRequesting = false;
			if (this.ValuesWritten != null)
			{
				this.ValuesWritten(this, e);
			}
		}
	}
}
