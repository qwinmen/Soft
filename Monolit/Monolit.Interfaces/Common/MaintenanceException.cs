using System;
using System.Runtime.Serialization;

namespace Monolit.Interfaces.Common
{
	public class MaintenanceException : Exception
	{
		public MaintenanceException()
		{
		}

		public MaintenanceException(string message)
			: base(message)
		{
		}

		public MaintenanceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected MaintenanceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}