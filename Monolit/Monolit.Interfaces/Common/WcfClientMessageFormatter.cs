using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Monolit.Interfaces.Common
{
	public class WcfClientMessageFormatter : IClientMessageFormatter
	{
		private readonly IClientMessageFormatter _innerClientFormatter;
		private readonly WcfMessageEncodingContext _context;
		private readonly IWcfMessageEncodingController _controller;

		public WcfClientMessageFormatter(IClientMessageFormatter innerClientFormatter,
			WcfMessageEncodingContext context, IWcfMessageEncodingController controller)
		{
			if (innerClientFormatter == null)
				throw new ArgumentNullException("innerClientFormatter");
			_innerClientFormatter = innerClientFormatter;
			_context = context;
			_controller = controller;
		}

		public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
		{
			Message message = _innerClientFormatter.SerializeRequest(messageVersion, parameters);
			if (_controller != null)
				_controller.Encode(_context, message);
			return message;
		}

		public object DeserializeReply(Message message, object[] parameters)
		{
			if (_controller != null)
				_controller.Decode(_context, message);
			return _innerClientFormatter.DeserializeReply(message, parameters);
		}
	}
}