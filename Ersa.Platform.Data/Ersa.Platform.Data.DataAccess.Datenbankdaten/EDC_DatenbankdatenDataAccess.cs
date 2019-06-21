using Ersa.Global.Common.Helper;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Data.DatenModelle.Organisation;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Datenbankdaten;
using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Datenbankdaten
{
	public class EDC_DatenbankdatenDataAccess : EDC_DataAccess, INF_DatenbankdatenDataAccess, INF_DataAccess
	{
		public EDC_DatenbankdatenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task<string> FUN_fdcLeseLetztesBackupDatumAusDatenbankAsync()
		{
			EDC_Parameter eDC_Parameter = await FUN_i64LeseLetztesBackupDatumParameterAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			string text = (eDC_Parameter != null) ? eDC_Parameter.PRO_strInhalt : string.Empty;
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			if (DateTime.TryParseExact(text, "G", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
			{
				return result.ToString(Thread.CurrentThread.CurrentCulture);
			}
			return string.Empty;
		}

		public async Task FUN_fdcSpeicherAktuellesDatumAlsLetztesBackupDatumAsync()
		{
			EDC_Parameter edcParameter = await FUN_i64LeseLetztesBackupDatumParameterAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			bool flag = false;
			if (edcParameter == null)
			{
				edcParameter = new EDC_Parameter
				{
					PRO_strParameter = "last database backup"
				};
			}
			else
			{
				flag = true;
			}
			edcParameter.PRO_strInhalt = DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo);
			if (flag)
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcParameter).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcParameter).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<long> FUN_fdcLeseLoetprogrammVariablenVersionAusDatenbankAsync()
		{
			return (await FUN_i64LeseLoetprogrammVariablenVersionAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false))?.PRO_i64Wert ?? 0;
		}

		public async Task FUN_fdcSpeichereAktuelleLoetprogrammVariablenVersionAsync(long i_i64Version, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_Parameter edcParameter = await FUN_i64LeseLoetprogrammVariablenVersionAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			bool flag = false;
			if (edcParameter == null)
			{
				edcParameter = new EDC_Parameter
				{
					PRO_strParameter = "soldering program variables version"
				};
			}
			else
			{
				flag = true;
			}
			edcParameter.PRO_i64Wert = i_i64Version;
			if (flag)
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcParameter, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcParameter, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<long> FUN_fdcLeseProzessschreiberVariablenVersionAusDatenbankAsync()
		{
			return (await FUN_i64LeseProzessschreiberVariablenVersionAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false))?.PRO_i64Wert ?? 0;
		}

		public async Task FUN_fdcSpeichereAktuelleProzessschreiberVariablenVersionAsync(long i_i64Version, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_Parameter edcParameter = await FUN_i64LeseProzessschreiberVariablenVersionAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			bool flag = false;
			if (edcParameter == null)
			{
				edcParameter = new EDC_Parameter
				{
					PRO_strParameter = "recorder variables version"
				};
			}
			else
			{
				flag = true;
			}
			edcParameter.PRO_i64Wert = i_i64Version;
			if (flag)
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcParameter, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcParameter, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<long> FUN_fdcLeseLoetprotokollVariablenVersionAusDatenbankAsync()
		{
			return (await FUN_i64LeseLoetprotokollVariablenVersionAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false))?.PRO_i64Wert ?? 0;
		}

		public async Task FUN_fdcSpeichereAktuelleLoetprotokollVariablenVersionAsync(long i_i64Version, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_Parameter edcParameter = await FUN_i64LeseLoetprotokollVariablenVersionAusDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			bool flag = false;
			if (edcParameter == null)
			{
				edcParameter = new EDC_Parameter
				{
					PRO_strParameter = "soldering protocol variables version"
				};
			}
			else
			{
				flag = true;
			}
			edcParameter.PRO_i64Wert = i_i64Version;
			if (flag)
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(edcParameter, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcParameter, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public Task FUN_fdcSichereDieDatenbankAsync(string i_strSicherungspfad)
		{
			return m_edcDatenbankAdapter.FUN_fdcDatensicherungAsync(i_strSicherungspfad);
		}

		public async Task FUN_fdcFuehreDatenbankWartungDurchAsync()
		{
			await m_edcDatenbankAdapter.FUN_fdcReorganisiereDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcVerkleinereDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcKomprimiereDatenbankAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		public bool FUN_blnIstDieDatenbankLokalInstalliert()
		{
			if (m_edcDatenbankAdapter.PRO_edcDatenbankProvider.PRO_blnIstDateibasierendeDatenbank)
			{
				return true;
			}
			return EDC_HostHelfer.FUN_blnIstHostaAdressLokal(m_edcDatenbankAdapter.FUN_strHoleServerName());
		}

		private Task<EDC_Parameter> FUN_i64LeseLetztesBackupDatumParameterAusDatenbankAsync()
		{
			return FUN_fdcLadeParameterAsync(EDC_Parameter.FUN_strLetztesBackupWhereStatementErstellen());
		}

		private Task<EDC_Parameter> FUN_i64LeseLoetprogrammVariablenVersionAusDatenbankAsync()
		{
			return FUN_fdcLadeParameterAsync(EDC_Parameter.FUN_strLoetprogrammVariablenVersionWhereStatementErstellen());
		}

		private Task<EDC_Parameter> FUN_i64LeseProzessschreiberVariablenVersionAusDatenbankAsync()
		{
			return FUN_fdcLadeParameterAsync(EDC_Parameter.FUN_strProzessschreiberVariablenVersionWhereStatementErstellen());
		}

		private Task<EDC_Parameter> FUN_i64LeseLoetprotokollVariablenVersionAusDatenbankAsync()
		{
			return FUN_fdcLadeParameterAsync(EDC_Parameter.FUN_strLoetprotokollVariablenVersionWhereStatementErstellen());
		}

		private Task<EDC_Parameter> FUN_fdcLadeParameterAsync(string i_strWhereStatement)
		{
			EDC_Parameter i_edcSelectObjekt = new EDC_Parameter(i_strWhereStatement);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(i_edcSelectObjekt);
		}
	}
}
