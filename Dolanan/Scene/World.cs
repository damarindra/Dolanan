using System;
using System.Collections.Generic;
using System.Linq;
using Dolanan.Tools;
using Dolanan.Collision;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Tools.GameHelper;
using Microsoft.Xna.Framework;
using Hit = Dolanan.Collision.Hit;

namespace Dolanan.Scene
{
	
	//TODO create system that load and unload world. generally changing scene. World is scene
	/// <summary>
	/// World, yes world. It also handle all collision.
	/// </summary>
	public class World : WorldCollision, IGameCycle
	{
		public Camera Camera;
		protected List<Layer> Layers = new List<Layer>();
		private LayerName _defaultLayer = LayerName.Default;
		
		/// <summary>
		/// DynamicActor is Actor that can move between layer. Whenever layer call Unload, the DynamicActor will be removed
		/// from the layer, and if Load called, DynamicActor will be add at it. Useful for player.
		/// </summary>
		public List<Actor> DynamicActor = new List<Actor>();

		public World()
		{
			Initialize();

			foreach (LayerName name in (LayerName[]) Enum.GetValues(typeof(LayerName)))
			{
				Layers.Add(new Layer(this, name));
			}

			Camera = GetDefaultLayer().AddActor<Camera>("Camera");
		}

		#region GameCycle

		public Layer GetLayer(LayerName layerName)
		{
			return Layers[(int) layerName];
		}
		public Layer GetDefaultLayer()
		{
			return Layers[(int) LayerName.Default];
		}

		public T CreateActor<T>(string name) where T : Actor
		{
			return GetDefaultLayer().AddActor<T>(name);
		}
		public T CreateActor<T>(string name, Layer layer) where T : Actor
		{
			return layer.AddActor<T>(name);
		}
		
		public virtual void Initialize()
		{
		}
		
		public virtual void Start()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (var layer in Layers)
			{
				layer.Update(gameTime);
			}
			Camera.Update(gameTime);
		}

		public virtual void LateUpdate(GameTime gameTime)
		{
			foreach (var layer in Layers)
			{
				layer.LateUpdate(gameTime);
			}
			Camera.LateUpdate(gameTime);
		}

		/// <summary>
		/// Drawing
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="layerZDepth">Not important, for Layer / Actor / Component only</param>
		public virtual void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			foreach (var layer in Layers)
			{
				layer.Draw(gameTime, layerZDepth);
			}
		}
		
		#endregion

		public void DrawCollision()
		{
			foreach (Body collisionCollider in Colliders)
			{
				GameMgr.SpriteBatch.DrawStroke(new Rectangle((int) collisionCollider.Min.X, (int)collisionCollider.Min.Y,
					(int)collisionCollider.Size.X, (int)collisionCollider.Size.Y ), Color.White);
			}
		}
	}
}