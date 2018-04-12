using System.Collections.Specialized;

namespace Monolit.DataLayer.Model.dbo.Objects
{
	public class ObjectDao : VersionedEntryDao
	{
		public string Name { get; set; }

		internal class Map : Map<ObjectDao>
		{
			public Map()
			{
				ToTable("Objects", "dbo");

				Property(t => t.Name).HasColumnName("Name").HasMaxLength(50).IsUnicode();
			}
		}
	}
}
