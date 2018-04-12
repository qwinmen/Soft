using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Monolit.Interfaces;

namespace Monolit.DataLayer.Model
{
	public abstract class VersionedEntryDao : IVersionedEntry
	{
		public int ID { get; set; }
		public long DocID { get; set; }
		public Guid UID { get; set; }
		public bool IsDeleted { get; set; }
		public long Revision { get; set; }

		internal class Map<TVersionedEntry> : EntityTypeConfiguration<TVersionedEntry>
			where TVersionedEntry : VersionedEntryDao
		{
			public Map()
			{
				HasKey(t => t.UID);

				Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
				Property(t => t.DocID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
				Property(t => t.UID).IsRequired();
				Property(t => t.IsDeleted).IsRequired();
				Property(t => t.Revision).IsRequired().IsConcurrencyToken().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
			}
		}
	}
}
