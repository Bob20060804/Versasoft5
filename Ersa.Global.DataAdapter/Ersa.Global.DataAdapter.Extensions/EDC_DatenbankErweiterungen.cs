using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.Extensions
{
	public static class EDC_DatenbankErweiterungen
	{
		private const string mC_strFehler = "unkown error";

		public static string FUN_blnPruefeVerbindung(this DbConnection i_fdcConnection, int i_i16Timeout, int i_i16AnzahlVerusche)
		{
			int i = 0;
			string text = "unkown error";
			for (; i < i_i16AnzahlVerusche; i++)
			{
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				text = i_fdcConnection.FUN_blnPruefeVerbindung(i_i16Timeout);
				if (!string.IsNullOrEmpty(text))
				{
					Task.Delay(1000).Wait(1000);
				}
			}
			return text;
		}

		public static string FUN_blnPruefeVerbindung(this DbConnection i_fdcConnection, int i_i16Timeout)
		{
			Task<string> task = Task.Run(async delegate
			{
				try
				{
					await i_fdcConnection.OpenAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					return ex.Message;
				}
				finally
				{
					try
					{
						i_fdcConnection.Close();
					}
					catch (Exception)
					{
					}
				}
				return string.Empty;
			});
			task.Wait(i_i16Timeout);
			if (task.IsCompleted)
			{
				return task.Result;
			}
			return "unkown error";
		}
	}
}
