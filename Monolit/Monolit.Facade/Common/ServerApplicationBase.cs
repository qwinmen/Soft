using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Configuration;
using ViageSoft.SystemServices.Applications;
using ViageSoft.SystemServices.Contextual;

namespace Monolit.Facade.Common
{
	public sealed class ServiceLibraryAttribute : Attribute
	{
		public ServiceLibraryAttribute(Type contractLibraryType, Type serviceLibraryType)
		{
			ContractLibraryType = contractLibraryType;
			ServiceLibraryType = serviceLibraryType;
		}

		public Type ContractLibraryType { get; private set; }
		public Type ServiceLibraryType { get; private set; }
	}


	public abstract class ServerApplicationBase : ServiceApplicationBase, IServerApplication
	{
		private List<ServiceHost> _services;

		protected ServerApplicationBase(KeyValuePair<Type, Type>[] serviceLibraries)
			: base(ServiceHelper.GetServiceMap(serviceLibraries).ToArray())
		{
			ServiceLibraries = serviceLibraries;
		}

		protected ServerApplicationBase(IEnumerable<ServiceLibraryAttribute> serviceLibraries)
			: this(serviceLibraries.Select(lib => new KeyValuePair<Type, Type>(lib.ContractLibraryType, lib.ServiceLibraryType)).ToArray())
		{
		}

		public IWcfMessageEncodingController MessageEncodingController { get; protected set; }

		public IStartupSettings StartupSettings => ViageSoft.SystemServices.Applications.StartupSettings.CurrentSettings;

		public KeyValuePair<Type, Type>[] ServiceLibraries { get; private set; }

		protected override void InitContext(IGlobalContext context)
		{
			Context.Set<IWcfServiceInfoFactory>(new WcfServiceInfoFactory(ClusterSection.GetConfigSection()));
			MessageEncodingController = new FacadeMessageEncodingController();
			base.InitContext(context);
		}

		protected override void InternalStart(object[] args)
		{
			Init(Context);
			StartServices();
		}

		protected virtual void Init(IGlobalContext context)
		{
		}

		protected override void InternalStop()
		{
			StopServices();
		}

		protected override void CreateServices()
		{
			_services = new List<ServiceHost>();
			var knownTypes = GlobalContextManager.CurrentContext.Get<IWcfKnownTypeRepository>();
			var baseAddresses = new List<Uri>();
			WcfServiceServerInfo[] serviceInfos = GetServiceInfos(StartupSettings.CurrentNodeKey, ServiceLibraries).ToArray();
			foreach (WcfServiceServerInfo serviceInfo in serviceInfos)
			{
				if (!serviceInfo.ServiceType.IsDefined(typeof(ErrorHandlerBehaviorAttribute), true))
					throw new InvalidOperationException(String.Format("Service {0} must have {1} for correct work.",
						serviceInfo.ServiceType.FullName, typeof(ErrorHandlerBehaviorAttribute).Name));

				baseAddresses.Add(serviceInfo.BaseAddress);
				ServiceHost serviceHost = CreateServiceHost(serviceInfo);
				foreach (ServiceEndpoint endpoint in serviceHost.Description.Endpoints)
				{
					foreach (OperationDescription operationDescription in endpoint.Contract.Operations)
					{
						WcfDataContractSerializerOperationBehavior.ReplaceBehavior(operationDescription, knownTypes);
						if (MessageEncodingController != null)
							WcfContextFormatterBehavior.ReplaceFormatterBehavior(operationDescription, MessageEncodingController, endpoint);
					}
				}
				_services.Add(serviceHost);
			}
		}

		private static ServiceHost CreateServiceHost(WcfServiceServerInfo serviceInfo)
		{
			var serviceHost = new ServiceHost(serviceInfo.ServiceType);
			var address = new Uri(serviceInfo.BaseAddress, serviceInfo.ServiceName);
			serviceHost.AddServiceEndpoint(serviceInfo.ContractType, serviceInfo.Binding, address);

			if (address.Scheme == "http")
			{
				var mexAddress = new Uri(address + "/mex");
				var metadataBehavior = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
				if (metadataBehavior == null)
				{
					metadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = mexAddress };
					serviceHost.Description.Behaviors.Add(metadataBehavior);
				}
//				serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), new WebHttpBinding(), mexAddress);
			}

			return serviceHost;
		}

		private void StartServices()
		{
			foreach (ServiceHost serviceHost in _services)
				serviceHost.Open();
		}

		private void StopServices()
		{
			foreach (ServiceHost serviceHost in _services)
				serviceHost.Close();
		}

		public static IEnumerable<WcfServiceServerInfo> GetServiceInfos(string currentNodeKey, KeyValuePair<Type, Type>[] libraries)
		{
			var serviceInfoFactory = GlobalContextManager.CurrentContext.Get<IWcfServiceInfoFactory>();
			return ServiceHelper.GetServiceMap(libraries)
					.Select(pair => serviceInfoFactory.CreateServiceServerInfo(currentNodeKey, pair.Key, pair.Value))
				;
		}
	}
}
