using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Monolit.DataLayer.SqlServerTypes;
using Monolit.Facade.Common;
using ViageSoft.SystemServices.Applications;
using ViageSoft.SystemServices.Contextual;

namespace Monolit.ServiceApp
{
	internal static class Program
	{
		const string AppTitle = "Monolit Application Service";

		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			StartupSettings.SetCurrentSettings(new CustomStartupSettings());

			var service = new MainService();

			Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

			if (Environment.UserInteractive)
			{
				Console.Title = AppTitle;
				try
				{
					((IServiceApplication) service).Start();
					var app = (ServerApplicationBase) GlobalContextManager.CurrentContext.Get<IApplication>();
					Console.Title = string.Format(" {0} (application server node key: {1})", AppTitle, app.StartupSettings.CurrentNodeKey);
					Console.WriteLine(@"The service is ready.");
					Console.WriteLine(@"Press <ENTER> to terminate service.");

					//Run windows app:
					Monolit.Program.Main();
					Console.ReadLine();
				}
				finally
				{
					((IServiceApplication) service).Stop();
				}
			}
			else
				ServiceBase.Run(service);
		}

		private class CustomStartupSettings: IStartupSettings
		{
			public CustomStartupSettings()
			{
				string[] args = Environment.GetCommandLineArgs();
				string appBaseDir = Path.GetDirectoryName(args[0]);
				AppBaseDir = string.IsNullOrEmpty(appBaseDir) ? Environment.CurrentDirectory : appBaseDir;
				CurrentClusterKey = string.Empty;
				CurrentNodeKey = GetCurrentNodeKey(args);
			}

			public string AppBaseDir { get; private set; }
			public string CurrentClusterKey { get; private set; }
			public string CurrentNodeKey { get; private set; }

			private static string GetCurrentNodeKey(string[] args)
			{
				return "AS1";
			}
		}
	}
}
