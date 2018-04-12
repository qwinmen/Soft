using System.Collections.Generic;
using System.Runtime.Serialization;
using ViageSoft.SystemServices.Collections;

namespace Monolit.Interfaces.Common
{
	[DataContract]
	public class CommonOperationResultSet<TData> : OperationResultSet<CommonOperationStatus, TData>
	{
		public CommonOperationResultSet(int pageIndex, int pageSize, int totalCount, IEnumerable<TData> data)
			: base(CommonOperationStatus.Success, pageIndex, pageSize, totalCount, data)
		{
		}

		public CommonOperationResultSet(int pageIndex, int pageSize, IEnumerable<TData> data)
			: base(CommonOperationStatus.Success, pageIndex, pageSize, data)
		{
		}

		public CommonOperationResultSet(IEnumerable<TData> data)
			: base(data == null ? CommonOperationStatus.Fail : CommonOperationStatus.Success, data)
		{
		}

		public CommonOperationResultSet(ResultSet<TData> data)
			: base(data == null ? CommonOperationStatus.Fail : CommonOperationStatus.Success, data)
		{
		}

		public CommonOperationResultSet(CommonOperationStatus status, int pageSize, int totalCount, int totalSize, IEnumerable<TData> data)
			: base(status, pageSize, totalCount, totalSize, data)
		{
		}

		public CommonOperationResultSet(CommonOperationStatus status, string message)
			: base(status, message)
		{
		}

		public CommonOperationResultSet(CommonOperationStatus status)
			: base(status)
		{
		}
	}

}
