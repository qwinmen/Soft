using ViageSoft.SystemServices.Common;
using ViageSoft.SystemServices.Configuration;
using ViageSoft.SystemServices.Contextual;

namespace Monolit.ServiceApp
{
	public class MainServiceGlobalContext : GlobalContextBase
	{
		public MainServiceGlobalContext() 
			: base(new SimpleInversionOfControlContainer())
		{
		}

		protected override void InternalInit()
		{
			Set(AppConfigStore.Default);
		}

		protected override void InternalUnload()
		{
		}
	}
}