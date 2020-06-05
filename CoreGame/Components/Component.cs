﻿using System;
using CoreGame.Engine;
using CoreGame.Scene;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Components
{
	public class Component : IGameCycle
	{
		private Actor _owner;

		public Actor Owner
		{
			get => _owner;
			set => _owner = value;
		}

		public Component(Actor owner)
		{
			Owner = owner;
		}

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