using System;
using System.Data.Entity;
using System.Linq;
using EntityFramework.DynamicFilters;
using Monolit.DataLayer.Model.dbo.Objects;
using Monolit.Interfaces;
using ViageSoft.SystemServices.Contextual;

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
