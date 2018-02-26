using System.ServiceModel.Description;

namespace Monolit.Interfaces.Common
{
	public class WcfMessageEncodingContext
	{
		public const string MessageEncodingNamespace = "127.0.0.1";

		private readonly ServiceEndpoint _endpoint;
		private readonly object _operation;
		private readonly OperationDescription _operationDescription;

		public WcfMessageEncodingContext(ServiceEndpoint endpoint, OperationDescription operationDescription,
			object operation)
		{
			_endpoint = endpoint;
			_operationDescription = operationDescription;
			_operation = operation;
		}

		public ServiceEndpoint Endpoint => _endpoint;

		public OperationDescription OperationDescription => _operationDescription;

		public object Operation => _operation;
	}
}