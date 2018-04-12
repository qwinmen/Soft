using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Monolit.Interfaces.Configuration;
using ViageSoft.SystemServices;

namespace Monolit.Interfaces.Common
{
	public class WcfServiceInfoFactory : IWcfServiceInfoFactory
	{
		private readonly ClusterNodeInfo[] _appServerNodeMap;
		private readonly Dictionary<string, ClusterNodeInfo> _appServerNodes;

		public WcfServiceInfoFactory(IEnumerable<ClusterNodeInfo> appServerNodes)
		{
			_appServerNodes = appServerNodes.ToDictionary(i => i.Key);
			_appServerNodeMap = GetMap(_appServerNodes.Keys.ToArray())
				.Select(i => _appServerNodes[i])
				.ToArray();
		}

		public WcfServiceInfoFactory(ClusterSection clusterSection)
			: this(clusterSection.ApplicationServerNodes.Cast<ClusterNodeElement>().Select(ClusterNodeInfo.Create))
		{
		}

		#region IWcfServiceInfoFactory Members

		public ClusterNodeState GetClusterNodeState(byte cnid)
		{
			ClusterNodeInfo nodeInfo = _appServerNodeMap[cnid % _appServerNodeMap.Length];
			return nodeInfo.State;
		}

		public ClusterNodeState GetClusterNodeState(string clusterNodeKey)
		{
			ClusterNodeInfo nodeInfo = _appServerNodes[clusterNodeKey];
			return nodeInfo.State;
		}

		public void SetClusterNodeState(byte cnid, ClusterNodeState state, TimeSpan stateLifetime)
		{
			ClusterNodeInfo nodeInfo = _appServerNodeMap[cnid % _appServerNodeMap.Length];
			nodeInfo.SetState(state, stateLifetime);
		}

		public void SetClusterNodeState(string clusterNodeKey, ClusterNodeState state, TimeSpan stateLifetime)
		{
			ClusterNodeInfo nodeInfo = _appServerNodes[clusterNodeKey];
			nodeInfo.SetState(state, stateLifetime);
		}

		public WcfServiceBasicInfo CreateServiceBasicInfo(byte cnid, Type contractType)
		{
			ClusterNodeInfo nodeInfo = GetActualClusterNode(cnid, true);
			var uri = new Uri(string.Format("net.tcp://{0}:{1}", nodeInfo.Host, nodeInfo.FirstPortNumber));
			return new WcfServiceBasicInfo(nodeInfo.Key, contractType, uri, new NetTcpBinding("defaultNetTcpBinding"));
		}

		internal ClusterNodeInfo GetActualClusterNode(byte cnid, bool canUseOverloaded)
		{
			ClusterNodeInfo nearestOverloadedNode = null;
			int nodeCount = _appServerNodeMap.Length;
			for (int actualNodeDisp = nodeCount; actualNodeDisp > 0; actualNodeDisp--)
			{
				var actualCnid = (byte)(cnid + nodeCount - actualNodeDisp);
				ClusterNodeInfo nodeInfo = _appServerNodeMap[actualCnid % _appServerNodeMap.Length];
				switch (nodeInfo.State)
				{
					case ClusterNodeState.Overload:
						if (canUseOverloaded && nearestOverloadedNode == null)
							nearestOverloadedNode = nodeInfo;
						break;
					case ClusterNodeState.Maintenance:
					case ClusterNodeState.Offline:
						break;
					default:
						return nodeInfo;
				}
			}
			if (nearestOverloadedNode == null)
				throw new MaintenanceException("There is no online service detected.");
			return nearestOverloadedNode;
		}

		public WcfServiceServerInfo CreateServiceServerInfo(string applicationServerNodeKey, Type contractType, Type serviceType)
		{
			ClusterNodeInfo nodeInfo = _appServerNodes[applicationServerNodeKey];
			var uri = new Uri(string.Format("net.tcp://{0}:{1}", nodeInfo.Host, nodeInfo.FirstPortNumber));
			return new WcfServiceServerInfo(contractType, serviceType, uri, new NetTcpBinding("defaultNetTcpBinding"));
		}

		#endregion

		public static string[] GetMap(string[] keys)
		{
			if (keys.Length == 0)
				keys = new[] { "AS1" };
			var temp = new List<Tuple<string, string>>();
			for (int i = 0; i < keys.Length; i++)
			for (int j = (i + 1) % keys.Length; j != i; j = (j + 1) % keys.Length)
				temp.Add(Tuple.Create(keys[i], keys[j]));
			if (temp.Count == 0)
				temp.Add(Tuple.Create(keys[0], keys[0]));
			var result = new List<Tuple<string, string>>();
			Tuple<string, string> item = temp.First();
			while (item != null)
			{
				result.Add(item);
				temp.Remove(item);
				item = temp.FirstOrDefault(i => i.Item1 == item.Item2);
			}
			return result.Select(tuple => tuple.Item1).ToArray();
		}

		#region Nested type: ClusterNodeInfo

		public class ClusterNodeInfo
		{
			private readonly ReaderWriterLockWrapper _lock = new ReaderWriterLockWrapper();
			private DateTime _actualUntilTime = DateTime.MaxValue;
			private ClusterNodeState _state;

			private ClusterNodeInfo()
			{
			}

			public string Key { get; set; }
			public string Host { get; set; }
			public ushort FirstPortNumber { get; set; }

			public ClusterNodeState State
			{
				get
				{
					return _lock.Read(() =>
						{
							if (DateTime.Now > _actualUntilTime)
								return ClusterNodeState.Unknown;
							return _state;
						});
				}
			}

			public void SetState(ClusterNodeState state, TimeSpan stateLifetime)
			{
				DateTime time = DateTime.Now + stateLifetime;
				_lock.ReadWrite(
					() => _state != state || time > _actualUntilTime,
					() =>
						{
							_state = state;
							_actualUntilTime = time;
						}
				);
			}

			public static ClusterNodeInfo Create(ClusterNodeElement element)
			{
				return new ClusterNodeInfo
					{
						Key = element.Key,
						Host = element.Host,
						FirstPortNumber = element.FirstPortNumber,
					};
			}

			internal static ClusterNodeInfo Create(string key, string host, ushort firstPortNumber)
			{
				return new ClusterNodeInfo
					{
						Key = key,
						Host = host,
						FirstPortNumber = firstPortNumber,
					};
			}
		}

		#endregion
	}
}
