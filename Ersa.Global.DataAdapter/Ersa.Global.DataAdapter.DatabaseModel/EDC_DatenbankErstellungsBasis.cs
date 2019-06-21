using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.DatabaseModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.DatabaseModel
{
	public class EDC_DatenbankErstellungsBasis
	{
		protected const int mC_i32MaxConnectionTries = 25;

		protected const int mC_i32ConnectTime = 1000;

		protected const int mC_i32SynchTime = 1000;

		protected const int mC_i32ShortTime = 2000;

		protected const int mC_i32LongTime = 5000;

		protected const int mC_i32LongRunnerTime = 60000;

		protected INF_DatenbankModel PRO_edcDatenbankDatenbankModel
		{
			get;
			set;
		}

		protected INF_DatenbankAdapter PRO_edcDatenbankAdapter
		{
			get;
			set;
		}

		protected void SUB_FuehreSkriptAus(string i_strSkript)
		{
			if (string.IsNullOrWhiteSpace(i_strSkript))
			{
				return;
			}
			string[] array = i_strSkript.Split(';');
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					PRO_edcDatenbankAdapter.SUB_ExecuteStatement(text);
				}
			}
		}

		protected void SUB_ErstelleTablespaces(INF_DatenbankModel i_edcModell)
		{
			foreach (KeyValuePair<string, string> item in i_edcModell.PRO_dicTablespacesErstellungsliste)
			{
				if (!PRO_edcDatenbankAdapter.FUN_blnExistiertTablespace(item.Key))
				{
					try
					{
						string path = Path.Combine(i_edcModell.PRO_strDatenbankDatenVerzeichnis, item.Key);
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
							SUB_WarteInMillicekunden(1000);
						}
					}
					catch (Exception)
					{
					}
					string value = item.Value;
					SUB_FuehreSkriptAus(value);
					SUB_WarteInMillicekunden(1000);
				}
			}
		}

		protected void SUB_WarteInMillicekunden(int i_i32Millisekunden)
		{
			TimeSpan fdcSpan = TimeSpan.FromMilliseconds(i_i32Millisekunden);
			Task.Factory.StartNew(() => Task.Delay(fdcSpan)).Wait(fdcSpan);
		}
	}
}
