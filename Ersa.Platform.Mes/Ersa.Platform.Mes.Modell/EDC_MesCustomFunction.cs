using System;
using System.Collections.Generic;

namespace Ersa.Platform.Mes.Modell
{
	/// <summary>
	/// Mes定制方法
	/// Mes Custom Function
	/// </summary>
	[Serializable]
	public class EDC_MesCustomFunction
	{
		public string PRO_strId
		{
			get;
			set;
		}

		public string PRO_strName
		{
			get;
			set;
		}

		/// <summary>
		/// 论据
		/// Argument
		/// </summary>
		public List<EDC_MesFunctionArgument> PRO_lstArguments
		{
			get;
			set;
		}

		/// <summary>
		/// Mes函数参数
		/// Mes Function Argument
		/// </summary>
		/// <param name="i_strArgumentId"></param>
		/// <returns></returns>
		public EDC_MesFunctionArgument this[string i_strArgumentId]
		{
			get
			{
				return PRO_lstArguments.Find((EDC_MesFunctionArgument i_edcArgument) => i_edcArgument.PRO_strId == i_strArgumentId);
			}
		}
		

		/// <summary>
		/// Constructor
		/// </summary>
		public EDC_MesCustomFunction()
		{
			PRO_lstArguments = new List<EDC_MesFunctionArgument>();
		}
	}
}
