using EntityFramework.DynamicFilters;
using System.Data.Entity;
using Monolit.DataLayer.Model.dbo.Objects;
using Monolit.Interfaces;

namespace Monolit.DataLayer.Model
{
	public class MonolitDataContext : DbContext
	{
		static MonolitDataContext()
		{
			Database.SetInitializer<MonolitDataContext>(null);
		}

		public MonolitDataContext()
			: base("Name=MonolitDataContext")
		{
		}
		
		public DbSet<ObjectDao> Objects { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Filter(DataFilters.SoftDelete, (IVersionedEntry entry) => !entry.IsDeleted);

			modelBuilder.Configurations.Add(new ObjectDao.Map());
			
		}
	}

}