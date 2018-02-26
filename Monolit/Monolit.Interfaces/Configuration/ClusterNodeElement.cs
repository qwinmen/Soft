using System.Configuration;
using ViageSoft.SystemServices;

namespace Monolit.Interfaces.Configuration
{
	public class ClusterNodeElement : System.Configuration.ConfigurationElement
	{
		[ConfigurationProperty("key", IsRequired = true)]
		public string Key => (string)this["key"];

		[ConfigurationProperty("host", IsRequired = true)]
		public string Host => (string)this["host"];

		[ConfigurationProperty("firstPortNumber", IsRequired = true, DefaultValue = (ushort)9000)]
		public ushort FirstPortNumber => CustomConverterHelper.TryConvert<ushort>(this["firstPortNumber"], 9000);
	}
}
