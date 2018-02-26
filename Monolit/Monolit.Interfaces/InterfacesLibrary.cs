using Monolit.Interfaces.Common;
using ViageSoft.SystemServices.Contextual;

namespace Monolit.Interfaces
{
	public sealed class InterfacesLibrary : LibraryBase
	{
		public InterfacesLibrary(IGlobalContext context)
			: base(context)
		{
		}

		protected override void InternalPrepareForUse()
		{
			Context.Set<IWcfKnownTypeRepository>(new WcfKnownTypeRepository()).Register(Assembly);
		}

		protected override void InternalUnload()
		{
		}
	}
}
