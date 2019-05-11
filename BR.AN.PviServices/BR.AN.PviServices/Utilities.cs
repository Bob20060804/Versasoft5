using System;
using System.Reflection;
using System.Resources;

namespace BR.AN.PviServices
{
	public class Utilities : IDisposable
	{
		private ResourceManager _pviResourceManager;

		private ResourceManager _pccResourceManager;

		internal bool propDisposed;

		private string _activeResourceManagerCulture;

		public string ActiveCulture
		{
			get
			{
				return _activeResourceManagerCulture;
			}
			set
			{
				_activeResourceManagerCulture = value;
				if (_activeResourceManagerCulture != "")
				{
					_pviResourceManager = null;
					_pccResourceManager = null;
					if (_activeResourceManagerCulture.ToLower().IndexOf("de") == 0)
					{
						_pviResourceManager = new ResourceManager("BR.AN.PviServices.Resources.de.PviErrors", Assembly.GetExecutingAssembly());
						_pccResourceManager = new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly());
					}
					else if (_activeResourceManagerCulture.ToLower().IndexOf("fr") == 0)
					{
						_pviResourceManager = new ResourceManager("BR.AN.PviServices.Resources.fr.PviErrors", Assembly.GetExecutingAssembly());
						_pccResourceManager = new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly());
					}
					else if (_activeResourceManagerCulture.ToLower().IndexOf("en") == 0)
					{
						_pviResourceManager = new ResourceManager("BR.AN.PviServices.Resources.en.PviErrors", Assembly.GetExecutingAssembly());
						_pccResourceManager = new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly());
					}
				}
			}
		}

		public event DisposeEventHandler Disposing;

		public Utilities()
		{
			propDisposed = true;
			_pviResourceManager = null;
			_pccResourceManager = null;
			_activeResourceManagerCulture = "undefined";
		}

		public void Dispose()
		{
			if (!propDisposed)
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}
		}

		internal virtual void Dispose(bool disposing)
		{
			if (!propDisposed)
			{
				OnDisposing(disposing);
				if (disposing)
				{
					propDisposed = true;
					_activeResourceManagerCulture = null;
					_pviResourceManager = null;
					_pccResourceManager = null;
				}
			}
		}

		internal virtual void OnDisposing(bool disposing)
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, new DisposeEventArgs(disposing));
			}
		}

		public string GetErrorText(int error)
		{
			string text = "";
			if (error == 0)
			{
				return "";
			}
			if (_pviResourceManager != null)
			{
				try
				{
					text = _pviResourceManager.GetString($"{error:0000}");
					if (text == null)
					{
						text = _pccResourceManager.GetString($"{error:0000}");
					}
					return text;
				}
				catch (System.Exception)
				{
				}
			}
			return error.ToString();
		}

		public string GetErrorText(int error, string culture)
		{
			string text = "";
			if (error == 0)
			{
				return "";
			}
			if (culture != null && culture != "")
			{
				if (culture.CompareTo(_activeResourceManagerCulture) == 0)
				{
					return GetErrorText(error);
				}
				try
				{
					ResourceManager resourceManager = (culture.ToLower().IndexOf("de") == 0) ? new ResourceManager("BR.AN.PviServices.Resources.de.PviErrors", Assembly.GetExecutingAssembly()) : ((culture.ToLower().IndexOf("fr") != 0) ? new ResourceManager("BR.AN.PviServices.Resources.en.PviErrors", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PviErrors", Assembly.GetExecutingAssembly()));
					text = resourceManager.GetString($"{error:0000}");
					if (text != null)
					{
						return text;
					}
					resourceManager = null;
					resourceManager = ((culture.ToLower().IndexOf("de") == 0) ? new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly()) : ((culture.ToLower().IndexOf("fr") != 0) ? new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly())));
					text = resourceManager.GetString($"{error:0000}");
					return text;
				}
				catch (System.Exception ex)
				{
					string message = ex.Message;
					return text;
				}
			}
			return text;
		}

		public string GetErrorTextPCC(int error)
		{
			string text = "";
			if (error == 0)
			{
				return "0";
			}
			if (_pccResourceManager != null)
			{
				try
				{
					return _pccResourceManager.GetString($"{error:0000}");
				}
				catch (System.Exception ex)
				{
					string message = ex.Message;
				}
			}
			return error.ToString();
		}

		[CLSCompliant(false)]
		public string GetErrorTextPCC(uint error)
		{
			string text = "";
			if (error == 0)
			{
				return "0";
			}
			if (_pccResourceManager != null)
			{
				try
				{
					return _pccResourceManager.GetString($"{error:0000}");
				}
				catch (System.Exception ex)
				{
					string message = ex.Message;
				}
			}
			return error.ToString();
		}
	}
}
