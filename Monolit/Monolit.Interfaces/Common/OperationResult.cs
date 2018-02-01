using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Monolit.Interfaces.Common
{
	[DataContract]
	public class OperationResult<TStatus>
		where TStatus : struct
	{
		public OperationResult(TStatus status, string message)
		{
			Status = status;
			if (!string.IsNullOrEmpty(message))
				ValidationErrors = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(string.Empty, message),
				};
		}

		public OperationResult(TStatus status)
			: this(status, null)
		{
		}

		[DataMember]
		public TStatus Status { get; private set; }

		[DataMember]
		public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

		public string FormatMessage()
		{
			return ValidationErrors == null || !ValidationErrors.Any()
					? "OK"
					: string.Join("; ", ValidationErrors.Select(FormatMessage))
				;
		}

		private string FormatMessage(KeyValuePair<string, string> pair)
		{
			return string.IsNullOrEmpty(pair.Key)
					? pair.Value
					: string.Format("{0}: {1}", pair.Key, pair.Value)
				;
		}
	}

	[DataContract]
	public class OperationResult<TStatus, TData> : OperationResult<TStatus>
		where TStatus : struct
	{
		public OperationResult(TStatus status, TData data, string message)
			: base(status, message)
		{
			Data = data;
		}

		public OperationResult(TStatus status, TData data)
			: this(status, data, null)
		{
		}

		[DataMember]
		public TData Data { get; private set; }
	}
}