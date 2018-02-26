using System;
using System.ServiceModel.Channels;

namespace Monolit.Interfaces.Common
{
	public abstract class WcfServiceInfoBase
	{
		private readonly Uri _baseAddress;
		private readonly Binding _binding;
		private readonly Type _contractType;
		private readonly string _serviceName;
		private readonly Uri _url;

		protected WcfServiceInfoBase(Type contractType, Uri baseAddress, Binding binding)
		{
			_contractType = contractType;
			_serviceName = contractType.Name.Substring(1);
			_baseAddress = baseAddress;
			_binding = binding;
			_url = new Uri(_baseAddress, _serviceName);
		}

		public Type ContractType => _contractType;

		public string ServiceName => _serviceName;

		public Uri BaseAddress => _baseAddress;

		public Binding Binding => _binding;

		public Uri Url => _url;
	}
}