using System;

namespace Monolit.Interfaces.Common
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
	public sealed class WcfKnownTypeAttribute : Attribute
	{
	}
}