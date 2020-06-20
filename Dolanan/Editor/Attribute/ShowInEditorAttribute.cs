using System;

namespace Dolanan.Editor.Attribute
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ShowInEditorAttribute : System.Attribute
	{
#if DEBUG
		
#endif
	}
}