using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Monolit.Interfaces.Common
{
	public class WcfContextFormatterBehavior : IOperationBehavior, IWsdlExportExtension
	{
		private readonly IOperationBehavior _innerFormatterBehavior;
		private readonly IWcfMessageEncodingController _messageEncodingController;
		private readonly ServiceEndpoint _endpoint;

		public WcfContextFormatterBehavior(IOperationBehavior innerFormatterBehavior,
			IWcfMessageEncodingController messageEncodingController, ServiceEndpoint endpoint)
		{
			_innerFormatterBehavior = innerFormatterBehavior;
			_endpoint = endpoint;
			_messageEncodingController = messageEncodingController;
		}

		#region Implementation of IOperationBehavior

		public void Validate(OperationDescription operationDescription)
		{
			if (_innerFormatterBehavior != null)
				_innerFormatterBehavior.Validate(operationDescription);
		}

		public void ApplyDispatchBehavior(OperationDescription operationDescription,
			DispatchOperation dispatchOperation)
		{
			if (dispatchOperation == null)
				throw new ArgumentNullException("dispatchOperation");
			if (_innerFormatterBehavior != null && dispatchOperation.Formatter == null)
				_innerFormatterBehavior.ApplyDispatchBehavior(operationDescription, dispatchOperation);
			dispatchOperation.Formatter = new WcfDispatchMessageFormatter(dispatchOperation.Formatter,
				new WcfMessageEncodingContext(_endpoint, operationDescription, dispatchOperation),
				_messageEncodingController);
		}

		public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
		{
			if (clientOperation == null)
				throw new ArgumentNullException("clientOperation");
			if (_innerFormatterBehavior != null && clientOperation.Formatter == null)
				_innerFormatterBehavior.ApplyClientBehavior(operationDescription, clientOperation);
			clientOperation.Formatter = new WcfClientMessageFormatter(clientOperation.Formatter,
				new WcfMessageEncodingContext(_endpoint, operationDescription, clientOperation),
				_messageEncodingController);
		}

		public void AddBindingParameters(OperationDescription operationDescription,
			BindingParameterCollection bindingParameters)
		{
			if (_innerFormatterBehavior != null)
				_innerFormatterBehavior.AddBindingParameters(operationDescription, bindingParameters);
		}

		#endregion

		#region Implementation of IWsdlExportExtension

		public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
		{
			((IWsdlExportExtension)_innerFormatterBehavior).ExportContract(exporter, context);
		}

		public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
		{
			((IWsdlExportExtension)_innerFormatterBehavior).ExportEndpoint(exporter, context);
		}

		#endregion

		public static void ReplaceFormatterBehavior(OperationDescription operationDescription,
			IWcfMessageEncodingController messageEncodingController, ServiceEndpoint endpoint)
		{
			// look for and remove the DataContract behavior if it is present
			IOperationBehavior formatterBehavior =
				operationDescription.Behaviors.Remove<DataContractSerializerOperationBehavior>();
			if (formatterBehavior == null)
			{
				// look for and remove the XmlSerializer behavior if it is present
				formatterBehavior = operationDescription.Behaviors.Remove<XmlSerializerOperationBehavior>();
				if (formatterBehavior == null)
				{
					// look for delegating formatter behavior
					var contextFormatterBehavior = operationDescription.Behaviors.Find<WcfContextFormatterBehavior>();
					if (contextFormatterBehavior == null)
					{
						throw new InvalidOperationException(
							"Could not find DataContractFormatter or XmlSerializer on the contract");
					}
				}
			}
			operationDescription.Behaviors.Insert(0,
				new WcfContextFormatterBehavior(formatterBehavior, messageEncodingController, endpoint));
		}
	}
}
