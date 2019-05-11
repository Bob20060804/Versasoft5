using Ersa.Platform.Common.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Plc.Interfaces
{
	public interface INF_EnumAnfoQuittDienst
	{
		Task FUN_fdcAnfoSetzenUndQuittierenBehandelnAsync<TEnum>(EDC_ToggleEnumWert<TEnum> i_edcToggle, CancellationToken i_fdcCancellationToken = default(CancellationToken)) where TEnum : struct, IConvertible, IComparable, IFormattable;
	}
}
