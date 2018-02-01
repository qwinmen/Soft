namespace Monolit.Interfaces
{
	public interface IVersionedEntry : IDataEntry
	{
		bool IsDeleted { get; }
		long Revision { get; }
	}
}
