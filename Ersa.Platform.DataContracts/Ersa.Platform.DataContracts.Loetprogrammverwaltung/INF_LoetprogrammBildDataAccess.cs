using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammBildDataAccess : INF_DataAccess
	{
		Task<Bitmap> FUN_fdcBildLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null);

		Task<byte[]> FUN_fdcBildArrayLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammBildData> FUN_fdcBildDatensatzLadenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleBildDataInDataTableAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBildAktualisierenAsync(EDC_LoetprogrammBildData i_edcBilddata, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBildVerwendungEntfernenAsync(long i_i64LoetprogrammId, ENUM_BildVerwendung i_enmVerwendung, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLpBilderEntfernenAsync(long i_i64LoetprogrammId, IEnumerable<ENUM_BildVerwendung> i_enuVerwendungen, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBilddatenImportierenAsync(DataSet i_fdcDataSet, long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);
	}
}
