using System.Runtime.Serialization;

namespace Monolit.Interfaces.Models.Objects
{
	[DataContract]
	public class Object : VersionedEntry
	{
		[DataMember]
		public string Name { get; set; }
		
	}
}
