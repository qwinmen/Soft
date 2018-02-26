using System.Configuration;

namespace Monolit.Interfaces.Configuration
{
	public class ClusterSection : ConfigurationSection
	{
		[ConfigurationProperty("applicationServerNodes")]
		public ClusterNodeCollection ApplicationServerNodes => (ClusterNodeCollection)this["applicationServerNodes"] ?? new ClusterNodeCollection();

		public static ClusterSection GetConfigSection()
		{
			return (ClusterSection)ConfigurationManager.GetSection("cluster");
		}
	}

	public class ClusterNodeCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ClusterNodeElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ClusterNodeElement)element).Key;
		}
	}
}
