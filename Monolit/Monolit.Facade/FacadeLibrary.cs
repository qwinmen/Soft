using System.ServiceModel.Dispatcher;
using ViageSoft.SystemServices.Contextual;
using Monolit.Facade.Common;


namespace Monolit.Facade
{
	public sealed class FacadeLibrary : LibraryBase
	{
		public FacadeLibrary(IGlobalContext context)
			: base(context)
		{
		}

		protected override void InternalPrepareForUse()
		{
			Context.Set<IErrorHandler>(new WcfErrorHandler());
		}

		protected override void InternalUnload()
		{
		}
	}
}
