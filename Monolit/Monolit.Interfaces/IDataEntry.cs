using System;

namespace Monolit.Interfaces
{
	public interface IDataEntry
	{
		int ID { get; }
		long DocID { get; }
		Guid UID { get; }
	}
}
