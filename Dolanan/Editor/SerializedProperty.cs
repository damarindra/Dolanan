using System;
using Microsoft.Xna.Framework;

namespace Dolanan.Editor
{
	public class SerializedProperty
	{
		public Object inspectedObject;
		public int intValue;
		public float floatValue;
		public string stringValue;
		public bool boolValue;
		public Vector2 vector2Value;
		public Point pointValue;
		public Object? objectValue;
	}
}