using System;
using System.Runtime.Serialization;

namespace Monolit.Interfaces
{
	[DataContract]
	public abstract class VersionedEntry : IVersionedEntry
	{
		[DataMember]
		public int ID { get; set; }

		[DataMember]
		public long DocID { get; set; }

		[DataMember]
		public Guid UID { get; set; }

		[DataMember]
		public bool IsDeleted { get; set; }

		[DataMember]
		public long Revision { get; set; }
	}
}
