using System;
using System.Collections.Generic;
using System.Reflection;

namespace Monolit.Interfaces.Common
{
	public interface IWcfKnownTypeRepository : IEnumerable<Type>
	{
		void Register(Assembly assembly);
	}
}
