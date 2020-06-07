using System;
using Dolanan.Tools;
using Dolanan.Engine;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components
{
	public class Component : IGameCycle
	{
		private Actor _owner;

		public Actor Owner
		{
			get => _owner;
			set => _owner = value;
		}

		internal Component(Actor owner)
		{
			Initialize();
			Owner = owner;
			Start();
		}

		public virtual void Initialize(){ }

		public virtual void Start() { }

		public virtual void Update(GameTime gameTime) { }

		public virtual void LateUpdate(GameTime gameTime) { }

		public virtual void Draw(GameTime gameTime, float layerZDepth = 0) { }
	}
}