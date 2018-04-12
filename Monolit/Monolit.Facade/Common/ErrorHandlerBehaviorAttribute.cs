using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using ViageSoft.SystemServices.Contextual;
using System.Collections.ObjectModel;

namespace Monolit.Facade.Common
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ErrorHandlerBehaviorAttribute : Attribute, IServiceBehavior
	{
		#region Implementation of IServiceBehavior

		void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription,
			ServiceHostBase serviceHostBase,
			Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription,
			ServiceHostBase serviceHostBase)
		{
			var errorHandler = GlobalContextManager.CurrentContext.Get<IErrorHandler>();

			foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
			{
				var channelDispatcher = channelDispatcherBase as ChannelDispatcher;
				if (channelDispatcher != null)
					channelDispatcher.ErrorHandlers.Add(errorHandler);
			}
		}

		#endregion
	}
}
