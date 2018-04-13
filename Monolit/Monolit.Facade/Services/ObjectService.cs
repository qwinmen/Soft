using System;
using System.Linq;
using System.ServiceModel;
using Monolit.BusinessLayer.Objects;
using Monolit.Facade.Common;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Contracts;

namespace Monolit.Facade.Services
{
	[ErrorHandlerBehavior]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
	public class ObjectService: IObjectServices
	{
		public CommonOperationResultSet<Interfaces.Models.Objects.Object> GetObjects(long revision = 0)
		{
			return ObjectManager.GetNameObjects(revision);
		}

		public Interfaces.Models.Objects.Object GetObjectByUID(Guid uid)
		{
			return ObjectManager.GetNameObjectByUid(uid);
		}

		public CommentOperationResult UpdateObject(Interfaces.Models.Objects.Object objData)
		{
			return ObjectManager.UpdateNameObjects(objData);
		}

		public CommentOperationResult Delete(Guid objUid)
		{
			return ObjectManager.Delete(objUid);
		}
	}
}
