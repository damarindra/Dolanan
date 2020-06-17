using System;
using Dolanan.Scene;

namespace Dolanan.Engine.Attributes
{
	/// <summary>
	///     Still don't know how to do this
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor)]
	public class RequireComponent : Attribute
	{
		public Actor Actor;
		public Type ComponentType;

		public RequireComponent(Actor actor, Type componentType)
		{
			Actor = actor;
			ComponentType = componentType;
			Console.WriteLine("Require");
		}
	}
}