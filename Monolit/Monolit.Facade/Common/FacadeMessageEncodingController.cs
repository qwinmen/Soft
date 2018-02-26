using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading;
using Monolit.BusinessLayer;
using Monolit.Interfaces.Common;

namespace Monolit.Facade.Common
{
	public class FacadeMessageEncodingController: IWcfMessageEncodingController
	{
		private static bool TryGetHeaderValue<T>(Message message, string name, out T result)
		{
			result = default(T);
			int headerIndex = message.Headers.FindHeader(name, WcfMessageEncodingContext.MessageEncodingNamespace);
			if (headerIndex < 0)
				return false;
			result = message.Headers.GetHeader<T>(headerIndex);
			return true;
		}

		private static T TryGetHeaderValue<T>(Message message, string name, T defaultValue)
		{
			T result;
			return TryGetHeaderValue(message, name, out result) ? result : defaultValue;
		}

		#region IWcfMessageEncodingController Members

		public void Encode(WcfMessageEncodingContext context, Message message)
		{
		}

		public void Decode(WcfMessageEncodingContext context, Message message)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo(TryGetHeaderValue(message, "Culture", "auto"));
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(TryGetHeaderValue(message, "UICulture", "auto"));
			SessionContext.SetCurrent(new SessionContext());
		}

		#endregion
	}
}
