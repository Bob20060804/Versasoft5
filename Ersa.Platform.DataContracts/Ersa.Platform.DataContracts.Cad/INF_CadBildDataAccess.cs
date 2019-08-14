using Ersa.Platform.Common.Data.Cad;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Cad
{
	public interface INF_CadBildDataAccess : INF_DataAccess
	{
		Task FUN_fdcBildDatensatzSpeichernAsync(long i_i64ProgrammId, EDC_CadBildData i_edcBilddata, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBilderSpeichernAsync(long i_i64ProgrammId, IEnumerable<EDC_CadBildData> i_enuBilddata, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_CadBildData> FUN_fdcBildDatensatzLadenAsync(long i_i64ProgrammId, ENUM_CadBildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CadBildData>> FUN_fdcAlleBilddatenLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBildVerwendungLoeschenAsync(long i_i64ProgrammId, ENUM_CadBildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBilderLoeschenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleBilddatenDataTableAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcImportiereCadBildDatenAusLoetprogrammAsync(DataSet i_fdcDataSet, long i_i64NeueProgrammId, IDbTransaction i_fdcTransaktion);
	}
}
