using Dolanan.Engine;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components
{
	public class Component : IGameCycle
	{
		public Component(Actor owner)
		{
			Initialize();
			Owner = owner;
			Start();
		}

		public Actor Owner { get; set; }

		public virtual void Initialize()
		{
		}

		public virtual void Start()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void LateUpdate(GameTime gameTime)
		{
		}

		public virtual void Draw(GameTime gameTime, float layerZDepth = 0)
		{
		}
	}
}