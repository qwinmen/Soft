using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ViageSoft.SystemServices.Collections;

namespace Monolit.Interfaces.Common
{
	[DataContract]
	public class OperationResultSet<TStatus, TData> : OperationResult<TStatus>
		where TStatus : struct
	{
		public OperationResultSet(TStatus status, int pageIndex, int pageSize, int totalCount, IEnumerable<TData> data)
			: base(status)
		{
			Data = (data ?? Enumerable.Empty<TData>()).Take(pageSize).ToList();
			PageIndex = pageIndex;
			PageSize = pageSize;
			TotalCount = totalCount;
			HasNextPage = totalCount > (pageIndex + 1) * pageSize;
		}

		public OperationResultSet(TStatus status, int pageIndex, int pageSize, IEnumerable<TData> data)
			: base(status)
		{
			Data = (data ?? Enumerable.Empty<TData>()).Take(pageSize == int.MaxValue ? pageSize : pageSize + 1).ToList();
			PageIndex = pageIndex;
			PageSize = pageSize;
			TotalCount = -1;
			HasNextPage = Data.Count > pageSize;
			if (HasNextPage)
				Data.RemoveAt(pageSize);
		}

		public OperationResultSet(TStatus status, IEnumerable<TData> data, string message)
			: base(status, message)
		{
			Data = (data ?? Enumerable.Empty<TData>()).ToList();
			PageIndex = 0;
			PageSize = int.MaxValue;
			TotalCount = Data.Count;
			HasNextPage = false;
		}

		public OperationResultSet(TStatus status, IEnumerable<TData> data)
			: this(status, data, null)
		{
		}

		public OperationResultSet(TStatus status, ResultSet<TData> data)
			: this(status, data.StartIndex / data.RequestedPageSize, data.RequestedPageSize, data.TotalSize, data)
		{
		}

		public OperationResultSet(TStatus status, string message)
			: this(status, Enumerable.Empty<TData>(), message)
		{
		}

		public OperationResultSet(TStatus status)
			: this(status, Enumerable.Empty<TData>())
		{
		}

		[DataMember]
		public List<TData> Data { get; private set; }

		[DataMember]
		public int PageIndex { get; private set; }

		public int PageNumber
		{
			get { return PageIndex + 1; }
		}

		[DataMember]
		public int PageSize { get; private set; }

		[DataMember]
		public int TotalCount { get; private set; }

		[DataMember]
		public bool HasNextPage { get; private set; }

		public bool HasPrevPage
		{
			get { return PageIndex > 0; }
		}

		public ResultSet<TData> ToResultSet()
		{
			return new ResultSet<TData>(Data, PageIndex * PageSize, PageSize, TotalCount);
		}
	}

}