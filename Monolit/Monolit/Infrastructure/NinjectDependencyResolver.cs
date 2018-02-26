using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Monolit.Facade.Services;
using Monolit.Interfaces.Contracts;
using Ninject;

namespace Monolit.Infrastructure
{
	public class NinjectDependencyResolver : IDependencyResolver
	{
		private IKernel kernel;

		public NinjectDependencyResolver(IKernel kernelParam)
		{
			kernel = kernelParam;
			AddBindings();
		}

		public object GetService(Type serviceType)
		{
			return kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}

		/// <summary>
		/// Тут размещают привязки
		/// </summary>
		private void AddBindings()
		{
			//Mock<IProductRepository> mock = new Mock<IProductRepository>();
			//mock.Setup(m => m.Products).Returns(new List<Product>
			//{
			//    new Product {Name = "Footbal", Price = 25},
			//    new Product {Name = "Surf board", Price = 179},
			//    new Product {Name = "Running shoes", Price = 95},
			//});
			//Жесткая привязка
			//kernel.Bind<IProductRepository>().ToConstant(mock.Object);

			//IService To Service
			//Привязка к БД и Entity Framework
			kernel.Bind<IObjectService>().To<ObjectService>();
		}

	}
}
