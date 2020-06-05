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

			//CreateAABB(new Vector2(8, 80), new Vector2(40, 65));
		}

		#region GameCycle

		public Layer GetLayer(LayerName layerName)
		{
			//return Layers.Count < (int) layerName ? Layers[(int) layerName] : Layers[(int) layerName];
			return Layers[(int) layerName];
		}
		public Layer GetDefaultLayer()
		{
			Console.WriteLine(Layers.Count);
			//return Layers.Count < (int) layerName ? Layers[(int) layerName] : Layers[(int) layerName];
			return Layers[(int) LayerName.Default];
		}

		public T CreateActor<T>(string name) where T : Actor
		{
			return Layers[(int)_defaultLayer].AddActor<T>(name);
		}
		
		public virtual void Initialize()
		{
			foreach (Layer layer in Layers)
			{
				layer.Initialize();
			}
			Camera.Initialize();
			
			CollisionHelper.CreateColliderActor(new Vector2(165, 0), new Vector2(128, 64));
			CollisionHelper.CreateColliderActor(new Vector2(60, 32),new Vector2(128, 128));
			CollisionHelper.CreateColliderActor(new Vector2(256, 123),new Vector2(64, 128));
			CollisionHelper.CreateColliderActor(new Vector2(70, 326),new Vector2( 128 + 43, 64+76));
			CollisionHelper.CreateColliderActor(new Vector2(753, 43),new Vector2( 128+43, 64+65));
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

		public Body CreateBody(Transform2D transform, Vector2 origin, Vector2 size)
		{
			Body result = new Body(transform, origin, size);
			Colliders.Add(result);
			return result;
		}

		public AABB CreateAABB(Vector2 pos, Vector2 size)
		{
			AABB result = new AABB();
			result.Position = pos;
			result.Size = size;
			Colliders.Add(result);
			return result;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="aabb"></param>
		/// <param name="velo"></param>
		/// <param name="resultVelo"></param>
		/// <param name="remainder"></param>
		/// <param name="hit"></param>
		/// <param name="bounce"></param>
		public void CheckCollision(AABB aabb, Vector2 velo, out Vector2 resultVelo, out Vector2 remainder, out Hit hit, int bounce = 3)
		{
			resultVelo = Vector2.Zero;
			remainder = Vector2.Zero;
			hit = new Hit();

			int bounceCounter = 0;

			Vector2 resultTemp;
			
			foreach (AABB b in Colliders)
			{
				if(b == aabb)
					continue;
				
				if (Check(aabb, b, velo, out var rVelo, out var rRemain, out var rHit))
				{
					resultVelo = rVelo;
					remainder = rRemain;
					hit = rHit;

					velo = rVelo;
					//Console.WriteLine(resultVelo);
					
					bounceCounter++;
					if (bounceCounter >= bounce)
					{
						Log.Print("Done Seq");
						return;
					}
				}
				
			}

			// no collision
			if (bounceCounter == 0)
				resultVelo = velo;
			Log.Print("Done Seq");

		}

		public void DrawCollision()
		{
			foreach (AABB collisionCollider in Colliders)
			{
				GameMgr.SpriteBatch.DrawStroke(new Rectangle((int) collisionCollider.Min.X, (int)collisionCollider.Min.Y,
					(int)collisionCollider.Size.X, (int)collisionCollider.Size.Y ), Color.White);
			}
		}
	}
}