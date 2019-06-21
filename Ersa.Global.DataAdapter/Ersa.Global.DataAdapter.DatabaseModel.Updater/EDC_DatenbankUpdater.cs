using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.DatabaseModel.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.DatabaseModel.Updater
{
	public class EDC_DatenbankUpdater : EDC_DatenbankErstellungsBasis
	{
		public EDC_DatenbankUpdater(INF_DatenbankModel i_edcDatenbankModel, INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			base.PRO_edcDatenbankDatenbankModel = i_edcDatenbankModel;
			base.PRO_edcDatenbankAdapter = i_edcDatenbankAdapter;
		}

		public void SUB_FuehreDatenbankUpdatesDurch(int i_i32StartVersion, int i_i32EndVersion, Action<long> i_delParameterUpdateAction)
		{
			SUB_ErstelleTablespaces(base.PRO_edcDatenbankDatenbankModel);
			SUB_DatenbankUpdateListeErstellen();
			int num = i_i32StartVersion + 1;
			Func<INF_DatenbankAdapter, Task> delMigrationsAction;
			do
			{
				try
				{
					string i_strSkript = base.PRO_edcDatenbankDatenbankModel.FUN_strHoleDatenbankUpdateAnweisungen(num, i_i32StartVersion);
					SUB_FuehreSkriptAus(i_strSkript);
					i_delParameterUpdateAction?.Invoke(num);
					delMigrationsAction = base.PRO_edcDatenbankDatenbankModel.FUN_delHoleDateMigrationsAction(num);
					if (delMigrationsAction != null)
					{
						Stopwatch stopwatch = Stopwatch.StartNew();
						bool flag = Task.Factory.StartNew((Func<Task>)async delegate
						{
							await delMigrationsAction(base.PRO_edcDatenbankAdapter).ConfigureAwait(continueOnCapturedContext: true);
						}).Wait(60000);
						stopwatch.Stop();
						Console.WriteLine($"Update step {num} ends after {stopwatch.Elapsed.Milliseconds} ms with result = {flag.ToString()}");
					}
					num++;
				}
				catch (Exception innerException)
				{
					throw new Exception($"Database update error in versionstep {num}", innerException);
				}
			}
			while (num <= i_i32EndVersion);
		}

		private void SUB_DatenbankUpdateListeErstellen()
		{
			IEnumerable<string> source = base.PRO_edcDatenbankAdapter.FUN_enuHoleListeAllerTabellen();
			base.PRO_edcDatenbankDatenbankModel.PRO_lstTabellenNamen = source.ToList();
			base.PRO_edcDatenbankDatenbankModel.SUB_ErstelleDatenbankUpdateliste();
		}
	}
}
