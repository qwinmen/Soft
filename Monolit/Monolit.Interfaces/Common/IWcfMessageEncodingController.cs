using System.ServiceModel.Channels;

namespace Monolit.Interfaces.Common
{
	public interface IWcfMessageEncodingController
	{
		void Encode(WcfMessageEncodingContext context, Message message);
		void Decode(WcfMessageEncodingContext context, Message message);
	}
}
