using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Xml;
using ViageSoft.SystemServices.Extensions;

namespace Monolit.Interfaces.Common
{
	public class WcfDataContractSerializerOperationBehavior : DataContractSerializerOperationBehavior
	{
		private readonly IEnumerable<Type> _knownTypes;

		public WcfDataContractSerializerOperationBehavior(OperationDescription operation,
			IEnumerable<Type> knownTypes)
			: base(operation)
		{
			_knownTypes = knownTypes;
		}

		public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns,
			IList<Type> knownTypes)
		{
			List<Type> allKnownTypes = knownTypes == null
				? _knownTypes.ToList()
				: knownTypes.ToList().Do(l => l.AddRange(_knownTypes));
			return base.CreateSerializer(type, name, ns, allKnownTypes);
		}

		public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name,
			XmlDictionaryString ns, IList<Type> knownTypes)
		{
			List<Type> allKnownTypes = knownTypes == null
				? _knownTypes.ToList()
				: knownTypes.ToList().Do(l => l.AddRange(_knownTypes));
			return base.CreateSerializer(type, name, ns, allKnownTypes);
		}

		public static void ReplaceBehavior(OperationDescription operationDescription, IEnumerable<Type> knownTypes)
		{
			var innerDataContractSerializerOperationBehavior =
				operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (innerDataContractSerializerOperationBehavior != null)
				operationDescription.Behaviors.Remove(innerDataContractSerializerOperationBehavior);
			operationDescription.Behaviors.Add(new WcfDataContractSerializerOperationBehavior(operationDescription,
				knownTypes));
		}
	}
}
