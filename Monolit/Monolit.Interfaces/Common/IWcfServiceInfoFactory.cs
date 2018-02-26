using System;
using System.ServiceModel.Channels;

namespace Monolit.Interfaces.Common
{
	public interface IWcfServiceInfoFactory
	{
		ClusterNodeState GetClusterNodeState(byte cnid);
		ClusterNodeState GetClusterNodeState(string clusterNodeKey);
		void SetClusterNodeState(byte cnid, ClusterNodeState state, TimeSpan stateLifetime);
		void SetClusterNodeState(string clusterNodeKey, ClusterNodeState state, TimeSpan stateLifetime);
		WcfServiceBasicInfo CreateServiceBasicInfo(byte cnid, Type contractType);
		WcfServiceServerInfo CreateServiceServerInfo(string applicationServerNodeKey, Type contractType, Type serviceType);
	}

	public sealed class WcfServiceServerInfo : WcfServiceInfoBase
	{
		private readonly Type _serviceType;

		public WcfServiceServerInfo(Type contractType, Type serviceType, Uri baseAddress, Binding binding)
			: base(contractType, baseAddress, binding)
		{
			_serviceType = serviceType;
		}

		public Type ServiceType => _serviceType;
	}

	public sealed class WcfServiceBasicInfo : WcfServiceInfoBase
	{
		public WcfServiceBasicInfo(string clusterNodeKey, Type contractType, Uri baseAddress, Binding binding)
			: base(contractType, baseAddress, binding)
		{
			ClusterNodeKey = clusterNodeKey;
		}

		public string ClusterNodeKey { get; private set; }
	}

	public enum ClusterNodeState
	{
		Unknown,
		Online,
		Overload,
		Maintenance,
		Offline,
	}
}
