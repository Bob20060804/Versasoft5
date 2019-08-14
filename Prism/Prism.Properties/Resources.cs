using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Prism.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("Prism.Properties.Resources", typeof(Resources).GetTypeInfo().get_Assembly());
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string CannotRegisterCompositeCommandInItself => ResourceManager.GetString("CannotRegisterCompositeCommandInItself", resourceCulture);

		internal static string CannotRegisterSameCommandTwice => ResourceManager.GetString("CannotRegisterSameCommandTwice", resourceCulture);

		internal static string DefaultDebugLoggerPattern => ResourceManager.GetString("DefaultDebugLoggerPattern", resourceCulture);

		internal static string DelegateCommandDelegatesCannotBeNull => ResourceManager.GetString("DelegateCommandDelegatesCannotBeNull", resourceCulture);

		internal static string DelegateCommandInvalidGenericPayloadType => ResourceManager.GetString("DelegateCommandInvalidGenericPayloadType", resourceCulture);

		internal static string EventAggregatorNotConstructedOnUIThread => ResourceManager.GetString("EventAggregatorNotConstructedOnUIThread", resourceCulture);

		internal static string InvalidDelegateRerefenceTypeException => ResourceManager.GetString("InvalidDelegateRerefenceTypeException", resourceCulture);

		internal static string InvalidPropertyNameException => ResourceManager.GetString("InvalidPropertyNameException", resourceCulture);

		internal static string PropertySupport_ExpressionNotProperty_Exception => ResourceManager.GetString("PropertySupport_ExpressionNotProperty_Exception", resourceCulture);

		internal static string PropertySupport_NotMemberAccessExpression_Exception => ResourceManager.GetString("PropertySupport_NotMemberAccessExpression_Exception", resourceCulture);

		internal static string PropertySupport_StaticExpression_Exception => ResourceManager.GetString("PropertySupport_StaticExpression_Exception", resourceCulture);

		internal Resources()
		{
		}
	}
}
