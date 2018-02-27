using System;
using System.ServiceModel;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Services;
using Object = Monolit.Interfaces.Models.Objects.Object;

namespace Monolit.Interfaces.Contracts
{
	[ServiceContract]
	public interface IObjectServices : ICommonService
	{
		[OperationContract(Name="GetByRevision")]
		CommonOperationResultSet<Object> GetNameObjects(long revision);

		[OperationContract(Name = "GetByUID")]
		Object GetNameObjects(Guid uid);

		[OperationContract(Name="GetByData")]
		CommentOperationResult GetNameObjects(Object objData);
	}
}
