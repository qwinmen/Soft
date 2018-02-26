using System;
using System.Collections.Generic;
using System.Reflection;
using MassTransit;
using MassTransit.QuartzIntegration;
using Monolit.Facade;
using Monolit.Facade.Common;
using Monolit.Interfaces;
using ViageSoft.SystemServices.Applications;
using ViageSoft.SystemServices.Contextual;
using ViageSoft.SystemServices.Extensions;
using MassTransit.SubscriptionConfigurators;
using Quartz;
using Quartz.Impl;

[assembly: ServiceLibrary(typeof(InterfacesLibrary), typeof(FacadeLibrary))]

namespace Monolit.ServiceApp
{
	public static class ServiceApplicationHelper
	{
		public static IEnumerable<ServiceLibraryAttribute> GetServiceLibraryAttributes()
		{
			return Assembly.GetEntryAssembly().GetAssemblyAttributes<ServiceLibraryAttribute>();
		}
	}

	public class MainServiceApplication : ServerApplicationBase
	{
		private static readonly Lazy<string> __lazyRabbitMqHostName = new Lazy<string>(() => ApplicationSettingsManager.Default.Get("RabbitMQ.HostName", "localhost"));
		private static readonly Lazy<string> __lazyRabbitMqQueueName = new Lazy<string>(() => ApplicationSettingsManager.Default.Get("RabbitMQ.QueueName", "monolit-dev"));

		public static string RabbitMqHostName => __lazyRabbitMqHostName.Value;

		public static string RabbitMqQueueName => __lazyRabbitMqQueueName.Value;

		public MainServiceApplication()
			: base(ServiceApplicationHelper.GetServiceLibraryAttributes())
		{
		}

		public IScheduler Scheduler { get; set; }
		public IServiceBus BusCtrl { get; private set; }

		protected override IGlobalContext CreateContext()
		{
			return new MainServiceGlobalContext();
		}

		protected override void Init(IGlobalContext context)
		{
			base.Init(context);
			ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
			Scheduler = schedulerFactory.GetScheduler().Result;

			BusCtrl = context.Set(ConfigureBusRabbitMq(Scheduler));
			Scheduler.JobFactory = new MassTransitJobFactory(BusCtrl);
		}

		private IServiceBus ConfigureBusInMemory(IScheduler scheduler)
		{
			return ServiceBusFactory.New(x =>
				{
					x.ReceiveFrom(string.Format("loopback://localhost/{0}", RabbitMqQueueName));
					x.UseJsonSerializer();

					x.Subscribe(s =>
						{
							RegisterConsumers(s);
							s.Consumer(() => new ScheduleMessageConsumer(scheduler));
						});
				});
		}

		private IServiceBus ConfigureBusRabbitMq(IScheduler scheduler)
		{
			return ServiceBusFactory.New(x =>
				{
					x.ReceiveFrom(string.Format("rabbitmq://{0}/{1}", RabbitMqHostName, RabbitMqQueueName));
					x.UseJsonSerializer();
					x.UseRabbitMq();

					x.Subscribe(s =>
						{
							RegisterConsumers(s);
							s.Consumer(() => new ScheduleMessageConsumer(scheduler));
						});
				});
		}

		private static void RegisterConsumers(SubscriptionBusServiceConfigurator config)
		{
//			config.Consumer<ScheduledSendMessageConsumer>();
//			config.Consumer<RemindConsumer>();
//			config.Consumer<CongratulateConsumer>();
		}

		protected override void InternalStart(object[] args)
		{
			base.InternalStart(args);
			Scheduler.Start();

			//GlobalContextManager.CurrentContext.Get<IJobManager>().Start();
//			var taskScheduler = Context.Get<ITaskScheduler>();
//			taskScheduler.Start();
		}

		protected override void InternalStop()
		{
//			var taskScheduler = Context.Get<ITaskScheduler>();
//			taskScheduler.Stop();
			//GlobalContextManager.CurrentContext.Get<IJobManager>().Stop();
			if (Scheduler != null)
				Scheduler.Standby();
			if (BusCtrl != null)
				BusCtrl.Dispose();
			if (Scheduler != null)
				Scheduler.Shutdown();
			base.InternalStop();
		}

	}
}