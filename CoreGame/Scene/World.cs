using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame.Collision;
using CoreGame.Controller;
using CoreGame.Engine;
using CoreGame.Tools;
using CoreGame.Tools.GameHelper;
using Microsoft.Xna.Framework;
using Hit = CoreGame.Collision.Hit;

namespace CoreGame.Scene
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

		public World(int width, int height, float cellSize = 64)
		{
			Camera = new Camera();

			foreach (LayerName name in (LayerName[]) Enum.GetValues(typeof(LayerName)))
			{
				Layers.Add(new Layer(this, name));
			}
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
			foreach (Layer layer in Layers)
			{
				layer.Initialize();
			}
			Camera.Initialize();
			
			var a = CollisionHelper.CreateColliderActor(new Vector2(165, 0), new Vector2(128, 64));
			a.GetComponent<Body>().Tag = "wall";
			a = CollisionHelper.CreateColliderActor(new Vector2(60, 32),new Vector2(128, 128));
			a.GetComponent<Body>().Tag = "wall";
			a = CollisionHelper.CreateColliderActor(new Vector2(256, 123),new Vector2(64, 128));
			a.GetComponent<Body>().Tag = "wall";
			a = CollisionHelper.CreateColliderActor(new Vector2(70, 326),new Vector2( 128 + 43, 64+76));
			a.GetComponent<Body>().Tag = "wall";
			a = CollisionHelper.CreateColliderActor(new Vector2(753, 43),new Vector2( 128+43, 64+65));
			a.GetComponent<Body>().Tag = "wall";

			var ac = CollisionHelper.CreateColliderActor(new Vector2(0, 128), new Vector2(32, 32));
			ac.GetComponent<Body>().BodyType = BodyType.Overlap;
			ac.GetComponent<Body>().Tag = "trigger";
		}

		public void PostInit()
		{
			foreach (Layer layer in Layers)
			{
				layer.ForceAddNewestActor();
			}
			Camera.Start();
		}
		
		/// <summary>
		/// Do not place any start again in here. In fact start will always called at the beginning of Update
		/// Layer.Start is allowed since it will called the newest Actor once
		/// Just don't call Actor.Start at here!. At ALL!
		/// </summary>
		public virtual void Start()
		{
			foreach (Layer layer in Layers)
			{
				layer.Start();
			}
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
			Camera.Draw(gameTime);
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