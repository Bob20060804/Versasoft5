using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Prism.Mvvm
{
	public static class ViewModelLocationProvider
	{
		private static Dictionary<string, Func<object>> _factories = new Dictionary<string, Func<object>>();

		private static Dictionary<string, Type> _typeFactories = new Dictionary<string, Type>();

		private static Func<Type, object> _defaultViewModelFactory = (Type type) => Activator.CreateInstance(type);

		private static Func<object, Type, object> _defaultViewModelFactoryWithViewParameter;

		private static Func<Type, Type> _defaultViewTypeToViewModelTypeResolver = delegate(Type viewType)
		{
			string fullName = viewType.FullName;
			fullName = fullName.Replace(".Views.", ".ViewModels.");
			string fullName2 = viewType.GetTypeInfo().get_Assembly().FullName;
			string text = fullName.EndsWith("View") ? "Model" : "ViewModel";
			return Type.GetType(string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", new object[3]
			{
				fullName,
				text,
				fullName2
			}));
		};

		public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
		{
			_defaultViewModelFactory = viewModelFactory;
		}

		public static void SetDefaultViewModelFactory(Func<object, Type, object> viewModelFactory)
		{
			_defaultViewModelFactoryWithViewParameter = viewModelFactory;
		}

		public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
		{
			_defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
		}

		public static void AutoWireViewModelChanged(object view, Action<object, object> setDataContextCallback)
		{
			object obj = GetViewModelForView(view);
			if (obj == null)
			{
				Type type = GetViewModelTypeForView(view.GetType());
				if ((object)type == null)
				{
					type = _defaultViewTypeToViewModelTypeResolver(view.GetType());
				}
				if ((object)type == null)
				{
					return;
				}
				obj = ((_defaultViewModelFactoryWithViewParameter != null) ? _defaultViewModelFactoryWithViewParameter(view, type) : _defaultViewModelFactory(type));
			}
			setDataContextCallback(view, obj);
		}

		private static object GetViewModelForView(object view)
		{
			string key = view.GetType().ToString();
			if (_factories.ContainsKey(key))
			{
				return _factories[key]();
			}
			return null;
		}

		private static Type GetViewModelTypeForView(Type view)
		{
			string key = view.ToString();
			if (_typeFactories.ContainsKey(key))
			{
				return _typeFactories[key];
			}
			return null;
		}

		public static void Register<T>(Func<object> factory)
		{
			Register(typeof(T).ToString(), factory);
		}

		public static void Register(string viewTypeName, Func<object> factory)
		{
			_factories[viewTypeName] = factory;
		}

		public static void Register<T, VM>()
		{
			Type typeFromHandle = typeof(T);
			Register(viewModelType: typeof(VM), viewTypeName: typeFromHandle.ToString());
		}

		public static void Register(string viewTypeName, Type viewModelType)
		{
			_typeFactories[viewTypeName] = viewModelType;
		}
	}
}
