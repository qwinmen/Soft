using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Monolit.Interfaces.Common
{
	public class WcfDispatchMessageFormatter : IDispatchMessageFormatter
	{
		private readonly IDispatchMessageFormatter _innerDispatchFormatter;
		private readonly WcfMessageEncodingContext _context;
		private readonly IWcfMessageEncodingController _controller;

		public WcfDispatchMessageFormatter(IDispatchMessageFormatter innerDispatchFormatter,
			WcfMessageEncodingContext context, IWcfMessageEncodingController controller)
		{
			if (innerDispatchFormatter == null)
				throw new ArgumentNullException("innerDispatchFormatter");

			_innerDispatchFormatter = innerDispatchFormatter;
			_context = context;
			_controller = controller;
		}

		public void DeserializeRequest(Message message, object[] parameters)
		{
			if (_controller != null)
				_controller.Decode(_context, message);
			_innerDispatchFormatter.DeserializeRequest(message, parameters);
		}

		public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
		{
			Message message = _innerDispatchFormatter.SerializeReply(messageVersion, parameters, result);
			if (_controller != null)
				_controller.Encode(_context, message);
			return message;
		}
	}
}
