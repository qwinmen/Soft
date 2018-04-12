using System;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Forms;
using Monolit.Infrastructure;
using Ninject;

namespace Monolit
{
	public static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(CreateKernel().Get<Form1>());
		}

		/// <summary>
		///     Creates the kernel that will manage your application.
		/// </summary>
		/// <returns>The created kernel.</returns>
		private static IKernel CreateKernel()
		{
			var kernel = new StandardKernel();
			//kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
			RegisterServices(kernel);
			return kernel;
		}

		/// <summary>
		///     Шлюз из зависимостей Ninject
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel)
		{
			DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
		}
	}
}
