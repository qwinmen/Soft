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
	public class ObjectService : IObjectService
	{
		public CommonOperationResultSet<Object> GetObjects(long revision)
		{
			return ObjectManager.GetObjects(revision);
		}

		public Object GetObjects(Guid uid)
		{
			return ObjectManager.GetObjectByUid(uid);
		}

		public CommentOperationResult GetObjects(Object objData)
		{
			return ObjectManager.UpdateObjects(objData);
		}
	}
}
