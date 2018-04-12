using Monolit.DataLayer.Model;
using Monolit.DataLayer.Model.dbo.Objects;
using Monolit.DataLayer.ORM.Repositories;
using ViageSoft.SystemServices.Contextual;

namespace Monolit.DataLayer
{
	public sealed class DataLayerLibrary : LibraryBase
	{
		public DataLayerLibrary(IGlobalContext context)
			: base(context)
		{}

		protected override void InternalPrepareForUse()
		{
			Context.RegisterType<IDataRepository<ObjectDao>, DefaultEntityDataRepository<MonolitDataContext, ObjectDao>>();
		}

		protected override void InternalUnload()
		{}
	}
}
