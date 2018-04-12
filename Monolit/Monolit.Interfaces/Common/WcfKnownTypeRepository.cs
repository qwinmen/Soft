using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ViageSoft.SystemServices.Extensions;

namespace Monolit.Interfaces.Common
{
	internal class WcfKnownTypeRepository : IWcfKnownTypeRepository
	{
		private readonly List<Type> _knownTypes = new List<Type>();

		#region Implementation of IWcfKnownTypeRepository

		public void Register(Assembly assembly)
		{
			IEnumerable<KeyValuePair<WcfKnownTypeAttribute, Type>> pairs =
				assembly.FindClassesByAttribute<WcfKnownTypeAttribute>();
			foreach (KeyValuePair<WcfKnownTypeAttribute, Type> pair in pairs)
				_knownTypes.Add(pair.Value);
		}

		#endregion

		#region Implementation of IEnumerable

		public IEnumerator<Type> GetEnumerator()
		{
			return _knownTypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
