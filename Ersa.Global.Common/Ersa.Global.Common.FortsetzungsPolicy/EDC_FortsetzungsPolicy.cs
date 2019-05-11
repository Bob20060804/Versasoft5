using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Global.Common.FortsetzungsPolicy
{
	public class EDC_FortsetzungsPolicy
	{
		private IList<Func<Task>> m_enuOperationen;

		private Func<Task> m_delLetzteOperation;

		public async Task<STRUCT_OperationsErgebnis> FUN_fdcOperationenAusfuehrenAsync(IEnumerable<Func<Task>> i_enuOperationen)
		{
			if (i_enuOperationen == null)
			{
				throw new ArgumentNullException("i_enuOperationen");
			}
			foreach (Func<Task> item in m_enuOperationen = ((i_enuOperationen as IList<Func<Task>>) ?? i_enuOperationen.ToList()))
			{
				try
				{
					m_delLetzteOperation = item;
					await item();
				}
				catch (Exception ex)
				{
					if (!(ex is OperationCanceledException))
					{
						return new STRUCT_OperationsErgebnis
						{
							PRO_fdcException = ex
						};
					}
				}
			}
			return await Task.FromResult(STRUCT_OperationsErgebnis.PROs_sttErfolgreichesOperationsErgebnis);
		}

		public Task<STRUCT_OperationsErgebnis> FUN_fdcOperationFortsetzenAsync()
		{
			IEnumerable<Func<Task>> i_enuOperationen = m_enuOperationen.Skip(m_enuOperationen.IndexOf(m_delLetzteOperation) + 1);
			return FUN_fdcOperationenAusfuehrenAsync(i_enuOperationen);
		}

		public Task<STRUCT_OperationsErgebnis> FUN_fdcOperationWiederholenUndFortsetzenAsync()
		{
			IEnumerable<Func<Task>> i_enuOperationen = m_enuOperationen.Skip(m_enuOperationen.IndexOf(m_delLetzteOperation));
			return FUN_fdcOperationenAusfuehrenAsync(i_enuOperationen);
		}
	}
}
