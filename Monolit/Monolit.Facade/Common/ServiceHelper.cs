using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using ViageSoft.SystemServices.Extensions;

namespace Monolit.Facade.Common
{
	public static class ServiceHelper
	{
		public static IEnumerable<KeyValuePair<Type, Type>> GetServiceMap(KeyValuePair<Type, Type> libraries)
		{
			KeyValuePair<ServiceContractAttribute, Type>[] serviceContracts =
				libraries.Key.Assembly.FindClassesByAttribute<ServiceContractAttribute>().ToArray();
			IEnumerable<KeyValuePair<ServiceBehaviorAttribute, Type>> services =
				libraries.Value.Assembly.FindClassesByAttribute<ServiceBehaviorAttribute>();
			foreach (KeyValuePair<ServiceBehaviorAttribute, Type> serviceType in services)
			foreach (KeyValuePair<ServiceContractAttribute, Type> contractType in serviceContracts)
				if (serviceType.Value.IsDerivedOrImplementsOf(contractType.Value))
					yield return new KeyValuePair<Type, Type>(contractType.Value, serviceType.Value);
		}

		public static IEnumerable<KeyValuePair<Type, Type>> GetServiceMap(KeyValuePair<Type, Type>[] libraries)
		{
			return libraries.SelectMany(GetServiceMap);
		}
	}
}