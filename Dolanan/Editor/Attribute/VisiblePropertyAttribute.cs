using System;

namespace Dolanan.Editor.Attribute
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class VisiblePropertyAttribute : System.Attribute
	{
		#if DEBUG
		
		#endif
	}
}