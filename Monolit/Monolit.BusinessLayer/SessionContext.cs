using System;

namespace Monolit.BusinessLayer
{
	public interface ISessionContext
	{
		
	}

	public sealed class SessionContext : ISessionContext
	{
		[ThreadStatic]
		private static ISessionContext __current;

		public static ISessionContext Current => __current;

		public static void SetCurrent(ISessionContext context)
		{
			__current = context;
		}

		public SessionContext()
		{
		}
		
	}
}
