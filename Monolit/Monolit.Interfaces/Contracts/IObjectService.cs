using System;
using System.ServiceModel;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Services;
using Object = Monolit.Interfaces.Models.Objects.Object;

namespace Monolit.Interfaces.Contracts
{
	[ServiceContract]
	public interface IObjectService : ICommonService
	{
		[OperationContract]
		CommonOperationResultSet<Object> GetObjects(long revision);

		[OperationContract]
		Object GetObjects(Guid uid);

		[OperationContract]
		CommentOperationResult GetObjects(Object objData);
	}
}
