using System;
using System.Reflection;

namespace Dolanan.Editor.Attribute
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class ShowInEditorAttribute : System.Attribute
	{
#if DEBUG
		
#endif
	}
	
	public interface IPropertyDrawer
	{
		public abstract void OnDrawProperty(FieldInfo fieldInfo, Object obj);
	} 
}