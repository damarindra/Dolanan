using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame.Collision;
using CoreGame.Controller;
using CoreGame.Engine;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Humper;
using Humper.Base;
using Humper.Responses;
using Hit = CoreGame.Collision.Hit;

namespace CoreGame.Scene
{
	
	//TODO create system that load and unload world. generally changing scene. World is scene
	/// <summary>
	/// World, yes world. It also handle all collision.
	/// </summary>
	public class World : IGameCycle, IWorld
	{
		public Camera Camera;
		protected List<Layer> Layers = new List<Layer>();
		private LayerName _defaultLayer = LayerName.Default;
		Collision.Collision _collision = new Collision.Collision();

		public World(int width, int height, float cellSize = 64)
		{
			Camera = new Camera();

			foreach (LayerName name in (LayerName[]) Enum.GetValues(typeof(LayerName)))
			{
				Layers.Add(new Layer(this, name));
			}
			grid = new Grid((int) Math.Ceiling(width / cellSize), 
				(int) Math.Ceiling(height / cellSize), cellSize);
			
			
			Transform2D tr1 = Transform2D.Identity;
			Transform2D tr2 = Transform2D.Identity;
			tr2.Position += new Vector2(60, 32);
			Transform2D tr3 = Transform2D.Identity;
			tr3.Position += new Vector2(256, 123);
			Transform2D tr4 = Transform2D.Identity;
			tr4.Position += new Vector2(70, 326);
			Transform2D tr5 = Transform2D.Identity;
			tr5.Position += new Vector2(753, 43);
			
			CreateAABB(tr1.Position + new Vector2(165, 0),  new Vector2(128, 64));
			CreateAABB(tr2.Position,  new Vector2(128, 128));
			CreateAABB(tr3.Position,  new Vector2(64, 128));
			CreateAABB(tr4.Position, new Vector2( 128 + 43, 64+76));
			CreateAABB(tr5.Position, new Vector2( 128+43, 64+65));
			CreateAABB(new Vector2(8, 80), new Vector2(40, 65));
		}
		
		#region GameCycle

		public Layer GetLayer(LayerName layerName)
		{
			//return Layers.Count < (int) layerName ? Layers[(int) layerName] : Layers[(int) layerName];
			return Layers[(int) layerName];
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

		#region Collisions
		
		public RectangleF Bounds => new RectangleF(0, 0, this.grid.Width , this.grid.Height);
		private Grid grid;

		/// <summary>
		/// Create a collision body
		/// </summary>
		/// <param name="transform2D">Transform</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		public BodyHumper Create(Transform2D transform2D, float width, float height, Vector2 origin)
		{
			BodyHumper b = new BodyHumper(this, transform2D, origin, width, height);
			grid.Add(b);
			return b;
		}

		IBox IWorld.Create(float x, float y, float width, float height)
		{
			Log.Print("Use the World.Create instead! IWorld.Create is not supported!");
			throw new NotImplementedException();
		}

		public bool Remove(IBox box)
		{
			return this.grid.Remove(box);
		}

		public void Update(IBox box, RectangleF @from)
		{
			this.grid.Update(box, from);
		}
		
		public Body CreateBody(Transform2D transform, Vector2 origin, Vector2 size)
		{
			Body result = new Body(transform, origin, size);
			_collision.Colliders.Add(result);
			return result;
		}

		public AABB CreateAABB(Vector2 pos, Vector2 size)
		{
			AABB result = new AABB(pos, size);
			_collision.Colliders.Add(result);
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
			
			foreach (AABB b in _collision.Colliders)
			{
				if(b == aabb)
					continue;
				
				if (_collision.Check(aabb, b, velo, out var rVelo, out var rRemain, out var rHit))
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
			foreach (AABB collisionCollider in _collision.Colliders)
			{
				GameMgr.SpriteBatch.DrawStroke(new Rectangle((int) collisionCollider.Min.X, (int)collisionCollider.Min.Y,
					(int)collisionCollider.Size.X, (int)collisionCollider.Size.Y ), Color.White);
			}
		}


		public IEnumerable<IBox> Find(float x, float y, float w, float h)
		{
			x = Math.Max(0, Math.Min(x, this.Bounds.Right - w));
			y = Math.Max(0, Math.Min(y, this.Bounds.Bottom - h));

			return this.grid.QueryBoxes(x, y, w, h);
		}

		public IEnumerable<IBox> Find(RectangleF area)
		{
			return this.Find(area.X, area.Y, area.Width, area.Height);
		}

		public IHit Hit(Vector2 point, IEnumerable<IBox> ignoring = null)
		{
			var boxes = this.Find(point.X, point.Y, 0, 0);

			if (ignoring != null)
			{
				boxes = boxes.Except(ignoring);
			}

			foreach (var other in boxes)
			{
				var hit = Humper.Hit.Resolve(point, other);

				if (hit != null)
				{
					return hit;
				}
			}

			return null;
		}

		public IHit Hit(Vector2 origin, Vector2 destination, IEnumerable<IBox> ignoring = null)
		{
			var min = Vector2.Min(origin, destination);
			var max = Vector2.Max(origin, destination);

			var wrap = new RectangleF(min, max - min);
			var boxes = this.Find(wrap.X, wrap.Y, wrap.Width, wrap.Height);

			if (ignoring != null)
			{
				boxes = boxes.Except(ignoring);
			}

			IHit nearest = null;

			foreach (var other in boxes)
			{
				var hit = Humper.Hit.Resolve(origin, destination, other);

				if (hit != null && (nearest == null || hit.IsNearest(nearest,origin)))
				{
					nearest = hit;
				}
			}

			return nearest;
		}

		public IHit Hit(RectangleF origin, RectangleF destination, IEnumerable<IBox> ignoring = null)
		{
			var wrap = new RectangleF(origin, destination);
			var boxes = this.Find(wrap.X, wrap.Y, wrap.Width, wrap.Height);

			if (ignoring != null)
			{
				boxes = boxes.Except(ignoring);
			}

			IHit nearest = null;

			foreach (var other in boxes)
			{
				var hit = Humper.Hit.Resolve(origin, destination, other);

				if (hit != null && (nearest == null || hit.IsNearest(nearest, origin.Location)))
				{
					nearest = hit;
				}
			}

			return nearest;
		}

		public IMovement Simulate(IBox box, float x, float y, Func<ICollision, ICollisionResponse> filter)
		{
			var origin = box.Bounds;
			var destination = new RectangleF(x, y, box.Width, box.Height);

			var hits = new List<IHit>();

			var result = new Movement()
			{
				Origin = origin,
				Goal = destination,
				Destination = this.Simulate(hits, new List<IBox>() { box }, (BodyHumper)box, origin, destination, filter),
				Hits = hits,
			};

			return result;
		}
		private RectangleF Simulate(List<IHit> hits, List<IBox> ignoring, BodyHumper box, RectangleF origin, RectangleF destination, Func<ICollision, ICollisionResponse> filter)
		{
			var nearest = this.Hit(origin, destination, ignoring);
				
			if (nearest != null)
			{
				hits.Add(nearest);

				var impact = new RectangleF(nearest.Position, origin.Size);
				var collision = new Humper.Collision() { Box = box, Hit = nearest, Goal = destination, Origin = origin };
				var response = filter(collision);

				if (response != null && destination != response.Destination)
				{
					ignoring.Add(nearest.Box);
					return this.Simulate(hits, ignoring, box, impact, response.Destination, filter);
				}
			}

			return destination;
		}
		
		public bool IsValidLocation(Vector2 v)
		{
			return (v.X >= 0) && (v.Y >= 0) && (v.X <= grid.Width) && (v.X <= grid.Height);
		}

		public void DrawDebug(int x, int y, int w, int h, Action<int, int, int, int, float> drawCell, Action<IBox> drawBox, Action<string, int, int, float> drawString)
		{
			var boxes = this.grid.QueryBoxes(x, y, w, h);
			foreach (var box in boxes)
			{
				drawBox(box);
			}

			// Drawing cells
			var cells = this.grid.QueryCells(x, y, w, h);
			foreach (var cell in cells)
			{
				var count = cell.Count();
				var alpha = count > 0 ? 1f : 0.4f;
				drawCell((int)cell.Bounds.X, (int)cell.Bounds.Y, (int)cell.Bounds.Width, (int)cell.Bounds.Height, alpha);
				drawString(count.ToString(), (int)cell.Bounds.Center.X, (int)cell.Bounds.Center.Y,alpha);
			}
		}
		#endregion
	}
}