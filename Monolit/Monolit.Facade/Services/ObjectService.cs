using System;
using System.ServiceModel;
using Monolit.BusinessLayer.Objects;
using Monolit.Facade.Common;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Contracts;
using Object = Monolit.Interfaces.Models.Objects.Object;

namespace Monolit.Facade.Services
{
	[ErrorHandlerBehavior]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
	public class ObjectService : IObjectServices
	{
		public CommonOperationResultSet<Object> GetNameObjects(long revision)
		{
			return ObjectManager.GetNameObjects(revision);
		}

		public Object GetNameObjects(Guid uid)
		{
			return ObjectManager.GetNameObjectByUid(uid);
		}

		public CommentOperationResult GetNameObjects(Object objData)
		{
			return ObjectManager.UpdateNameObjects(objData);
		}
	}
}
