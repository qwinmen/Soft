using System.Runtime.Serialization;
using Monolit.Interfaces.Common;
using Object = Monolit.Interfaces.Models.Objects.Object;

namespace Monolit.Interfaces.Contracts
{
	public enum ObjectServiceOperationStatus
	{
		Success,
		Fail,
	}

	[DataContract]
	public class CommentOperationResult : OperationResult<ObjectServiceOperationStatus, Object>
	{
		public CommentOperationResult(ObjectServiceOperationStatus status, Object data)
			: base(status, data)
		{
		}

		public CommentOperationResult(Object data)
			: base(ObjectServiceOperationStatus.Success, data)
		{
		}

		public CommentOperationResult(string messageError)
			: base(ObjectServiceOperationStatus.Fail, null, messageError)
		{
		}
	}

}
