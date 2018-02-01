using System.Data.Entity;
using Monolit.Interfaces;

namespace Monolit.DataLayer.Model
{
	public class DefaultEntityDataRepository<TDataContext, TDataEntry> : EntityDataRepositoryBase<TDataContext, TDataEntry>
		where TDataContext : DbContext, new()
		where TDataEntry : class, IDataEntry, new()
	{}
}
