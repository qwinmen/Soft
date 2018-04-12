using System;
using System.Linq;
using System.ServiceModel;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Services;

namespace Monolit.Interfaces.Contracts
{
	[ServiceContract]
	public interface IObjectServices: ICommonService
	{
		[OperationContract(Name = "GetByRevision")]
		CommonOperationResultSet<Models.Objects.Object> GetObjects(long revision = 0);

		[OperationContract(Name = "GetByUID")]
		Models.Objects.Object GetObjectByUID(Guid uid);

		[OperationContract(Name = "GetByData")]
		CommentOperationResult UpdateObject(Models.Objects.Object objData);
	}
}
