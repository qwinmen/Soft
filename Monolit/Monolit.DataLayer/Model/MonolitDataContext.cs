using System;
using System.Data.Entity;
using EntityFramework.DynamicFilters;
using System.Linq;
using Monolit.DataLayer.Model.dbo.Objects;
using Monolit.Interfaces;

namespace Monolit.DataLayer.Model
{
	public class MonolitDataContext: DbContext
	{
		static MonolitDataContext()
		{
			Database.SetInitializer<MonolitDataContext>(null);
		}

		public MonolitDataContext()
			: base("Name=MonolitDataContext") //.config file
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
