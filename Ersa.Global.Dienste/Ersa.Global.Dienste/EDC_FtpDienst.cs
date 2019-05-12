using Ersa.Global.Dienste.Exceptions;
using Ersa.Global.Dienste.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_FTPDienst))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDC_FtpDienst : INF_FTPDienst
	{
		private EDC_FtpKonfiguration m_edcFtpKonfiguration;

		public void SUB_Initialisieren(EDC_FtpKonfiguration i_edcFtpKonfiguration)
		{
			m_edcFtpKonfiguration = i_edcFtpKonfiguration;
		}

		public async Task SUB_DownloadAsync(string i_strQuellPfad, string i_strZielPfad)
		{
			try
			{
				using (WebResponse fdcResponse = await FUN_fdcFtpRequestFuerDownloadErstellen(i_strQuellPfad).GetResponseAsync())
				{
					using (Stream fdcResponseStream = fdcResponse.GetResponseStream())
					{
						using (StreamReader fdcReader = new StreamReader(fdcResponseStream))
						{
							using (StreamWriter fdcWriter = new StreamWriter(i_strZielPfad))
							{
								await fdcWriter.WriteAsync(await fdcReader.ReadToEndAsync());
							}
						}
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				Uri uri = FUN_fdcFtpUriErstellen(Path.GetFileName(i_strQuellPfad));
				throw new EDC_FtpKommunikationException("Error downloading file!", i_fdcInner)
				{
					PRO_strQuellDatei = (uri.IsAbsoluteUri ? uri.AbsoluteUri : i_strQuellPfad),
					PRO_strZielPfad = i_strZielPfad
				};
			}
		}

		public async Task SUB_UploadAsync(string i_strQuellPfad, string i_strZielPfad)
		{
			try
			{
				FtpWebRequest ftpWebRequest = FUN_fdcFtpRequestFuerUploadErstellen(i_strZielPfad);
				using (Stream fdcReqStream = ftpWebRequest.GetRequestStream())
				{
					byte[] array = File.ReadAllBytes(i_strQuellPfad);
					await fdcReqStream.WriteAsync(array, 0, array.Length);
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error uploading file!", i_fdcInner)
				{
					PRO_strQuellDatei = i_strQuellPfad,
					PRO_strZielPfad = i_strZielPfad
				};
			}
		}

		public async Task SUB_UploadAsync(IList<string> i_lstDateiInhalt, string i_strZielPfad)
		{
			try
			{
				byte[] array = FUNa_bytZeilenKonvertieren(i_lstDateiInhalt);
				FtpWebRequest ftpWebRequest = FUN_fdcFtpRequestFuerUploadErstellen(i_strZielPfad);
				using (Stream fdcReqStream = ftpWebRequest.GetRequestStream())
				{
					await fdcReqStream.WriteAsync(array, 0, array.Length);
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error uploading file content!", i_fdcInner)
				{
					PRO_strQuellDatei = string.Empty,
					PRO_strZielPfad = i_strZielPfad
				};
			}
		}

		public void SUB_Download(string i_strQuellPfad, string i_strZielPfad)
		{
			try
			{
				using (WebResponse webResponse = FUN_fdcFtpRequestFuerDownloadErstellen(i_strQuellPfad).GetResponse())
				{
					using (Stream stream = webResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(stream))
						{
							using (StreamWriter streamWriter = new StreamWriter(i_strZielPfad))
							{
								streamWriter.Write(streamReader.ReadToEnd());
							}
						}
					}
				}
			}
			catch (Exception i_fdcInner)
			{
				Uri uri = FUN_fdcFtpUriErstellen(Path.GetFileName(i_strQuellPfad));
				throw new EDC_FtpKommunikationException("Error downloading file!", i_fdcInner)
				{
					PRO_strQuellDatei = (uri.IsAbsoluteUri ? uri.AbsoluteUri : i_strQuellPfad),
					PRO_strZielPfad = i_strZielPfad
				};
			}
		}

		public void SUB_DownloadAlleFiles(string i_strQuellPfad, string i_strZielPfad)
		{
			foreach (string item in from i_strFile in FUN_enuRemoteDateiListeHolen(i_strQuellPfad)
			where !i_strFile.StartsWith(".")
			select i_strFile)
			{
				string i_strQuellPfad2 = Path.Combine(i_strQuellPfad, item);
				string i_strZielPfad2 = Path.Combine(i_strZielPfad, item);
				SUB_Download(i_strQuellPfad2, i_strZielPfad2);
			}
		}

		public void SUB_Upload(string i_strQuellPfad, string i_strZielPfad)
		{
			try
			{
				using (Stream stream = FUN_fdcFtpRequestFuerUploadErstellen(i_strZielPfad).GetRequestStream())
				{
					byte[] array = File.ReadAllBytes(i_strQuellPfad);
					stream.Write(array, 0, array.Length);
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error uploading file!", i_fdcInner)
				{
					PRO_strQuellDatei = i_strQuellPfad,
					PRO_strZielPfad = i_strZielPfad
				};
			}
		}

		public void SUB_Upload(IList<string> i_lstDateiInhalt, string i_strZielPfad)
		{
			try
			{
				byte[] array = FUNa_bytZeilenKonvertieren(i_lstDateiInhalt);
				using (Stream stream = FUN_fdcFtpRequestFuerUploadErstellen(i_strZielPfad).GetRequestStream())
				{
					stream.Write(array, 0, array.Length);
				}
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error uploading file content!", i_fdcInner)
				{
					PRO_strQuellDatei = string.Empty,
					PRO_strZielPfad = i_strZielPfad
				};
			}
		}

		public void SUB_DateiLoeschen(string i_strDateiPfad)
		{
			try
			{
				((FtpWebResponse)FUN_fdcFtpRequestFuerLoeschenErstellen(i_strDateiPfad).GetResponse()).Close();
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error deleting file!", i_fdcInner)
				{
					PRO_strQuellDatei = string.Empty,
					PRO_strZielPfad = i_strDateiPfad
				};
			}
		}

		public async Task FUN_fdcDateiLoeschen(string i_strDateiPfad)
		{
			try
			{
				((FtpWebResponse)(await FUN_fdcFtpRequestFuerLoeschenErstellen(i_strDateiPfad).GetResponseAsync())).Close();
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error deleting file!", i_fdcInner)
				{
					PRO_strQuellDatei = string.Empty,
					PRO_strZielPfad = i_strDateiPfad
				};
			}
		}

		public void SUB_VerzeichnisLeeren(string i_strVerzeichnisPfad)
		{
			foreach (string item in FUN_enuRemoteDateiListeHolen(i_strVerzeichnisPfad))
			{
				SUB_DateiLoeschen(Path.Combine(i_strVerzeichnisPfad, item));
			}
		}

		public async Task FUN_fdcVerzeichnisLeerenAsync(string i_strVerzeichnisPfad)
		{
			foreach (string item in await FUN_fdcRemoteDateiListeHolenAsync(i_strVerzeichnisPfad))
			{
				await FUN_fdcDateiLoeschen(Path.Combine(i_strVerzeichnisPfad, item));
			}
		}

		private IEnumerable<string> FUN_enuRemoteDateiListeHolen(string i_strQuellPfad)
		{
			List<string> list = new List<string>();
			try
			{
				using (FtpWebResponse ftpWebResponse = (FtpWebResponse)FUN_fdcFtpRequestFuerDateiListeErstellen(i_strQuellPfad).GetResponse())
				{
					using (Stream stream = ftpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(stream))
						{
							for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
							{
								list.Add(text);
							}
						}
					}
				}
				return list;
			}
			catch (WebException ex)
			{
				FtpWebResponse ftpWebResponse2 = ex.Response as FtpWebResponse;
				if (ftpWebResponse2 == null || ftpWebResponse2.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
				{
					throw new EDC_FtpKommunikationException("Error getting the file list!", ex)
					{
						PRO_strQuellDatei = i_strQuellPfad
					};
				}
				return Enumerable.Empty<string>();
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error getting the file list!", i_fdcInner)
				{
					PRO_strQuellDatei = i_strQuellPfad
				};
			}
		}

		private async Task<IEnumerable<string>> FUN_fdcRemoteDateiListeHolenAsync(string i_strQuellPfad)
		{
			List<string> lstFiles = new List<string>();
			try
			{
				using (FtpWebResponse fdcResponse = (FtpWebResponse)(await FUN_fdcFtpRequestFuerDateiListeErstellen(i_strQuellPfad).GetResponseAsync()))
				{
					using (Stream fdcResponseStream = fdcResponse.GetResponseStream())
					{
						using (StreamReader fdcReader = new StreamReader(fdcResponseStream))
						{
							for (string text = await fdcReader.ReadLineAsync(); text != null; text = await fdcReader.ReadLineAsync())
							{
								lstFiles.Add(text);
							}
						}
					}
				}
				return lstFiles;
			}
			catch (Exception i_fdcInner)
			{
				throw new EDC_FtpKommunikationException("Error getting the file list!", i_fdcInner)
				{
					PRO_strQuellDatei = i_strQuellPfad
				};
			}
		}

		private FtpWebRequest FUN_fdcFtpRequestFuerUploadErstellen(string i_strZielPfad)
		{
			FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(FUN_fdcFtpUriErstellen(i_strZielPfad));
			obj.Method = "STOR";
			obj.Credentials = new NetworkCredential(m_edcFtpKonfiguration.PRO_strRemoteUser, m_edcFtpKonfiguration.PRO_strRemotePass);
			obj.UsePassive = m_edcFtpKonfiguration.PRO_blnUsePassive;
			obj.UseBinary = m_edcFtpKonfiguration.PRO_blnUseBinary;
			obj.KeepAlive = m_edcFtpKonfiguration.PRO_blnKeepAlive;
			obj.Proxy = null;
			return obj;
		}

		private FtpWebRequest FUN_fdcFtpRequestFuerDownloadErstellen(string i_strQuellPfad)
		{
			FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(FUN_fdcFtpUriErstellen(i_strQuellPfad));
			obj.UseBinary = m_edcFtpKonfiguration.PRO_blnUseBinary;
			obj.Method = "RETR";
			obj.Credentials = new NetworkCredential(m_edcFtpKonfiguration.PRO_strRemoteUser, m_edcFtpKonfiguration.PRO_strRemotePass);
			obj.Proxy = null;
			return obj;
		}

		private FtpWebRequest FUN_fdcFtpRequestFuerDateiListeErstellen(string i_strPfad)
		{
			FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(FUN_fdcFtpUriErstellen(i_strPfad));
			obj.UseBinary = m_edcFtpKonfiguration.PRO_blnUseBinary;
			obj.Credentials = new NetworkCredential(m_edcFtpKonfiguration.PRO_strRemoteUser, m_edcFtpKonfiguration.PRO_strRemotePass);
			obj.Method = "NLST";
			obj.Proxy = null;
			return obj;
		}

		private FtpWebRequest FUN_fdcFtpRequestFuerLoeschenErstellen(string i_strDateiPfad)
		{
			FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(FUN_fdcFtpUriErstellen(i_strDateiPfad));
			obj.Method = "DELE";
			obj.Credentials = new NetworkCredential(m_edcFtpKonfiguration.PRO_strRemoteUser, m_edcFtpKonfiguration.PRO_strRemotePass);
			obj.UsePassive = m_edcFtpKonfiguration.PRO_blnUsePassive;
			obj.UseBinary = m_edcFtpKonfiguration.PRO_blnUseBinary;
			obj.KeepAlive = m_edcFtpKonfiguration.PRO_blnKeepAlive;
			obj.Proxy = null;
			return obj;
		}

		private Uri FUN_fdcFtpUriErstellen(string i_strPfad)
		{
			if (!string.IsNullOrWhiteSpace(m_edcFtpKonfiguration.PRO_strRemotePartition))
			{
				return new Uri($"ftp://{m_edcFtpKonfiguration.PRO_strRemoteHost}/{m_edcFtpKonfiguration.PRO_strRemotePartition}{i_strPfad}");
			}
			return new Uri($"ftp://{m_edcFtpKonfiguration.PRO_strRemoteHost}//{i_strPfad}");
		}

		private byte[] FUNa_bytZeilenKonvertieren(IEnumerable<string> i_enuZeilen)
		{
			string s = string.Join(Environment.NewLine, i_enuZeilen);
			return Encoding.UTF8.GetBytes(s);
		}
	}
}
