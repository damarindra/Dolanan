using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dolanan.Components;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public delegate void ParentState(Actor parent);

	public delegate void ChildState(Transform2D[] childs);

	public delegate void LayerState(Layer layer);

	/// <summary>
	///     Actor is an Entity.
	/// </summary>
	public class Actor : IGameCycle
	{
		// Component stuff
		// Render
		protected readonly List<Component> Components = new List<Component>();

		public string Name;
		public ChildState OnChildChange;
		public LayerState OnLayerChange;

		public ParentState OnParentChange;
		public Transform2D Transform;

		/// <summary>
		/// 	Get the ZDepth, useful for drawing stuff, raycast from camera
		/// 	This value automatically increment whenever calling GameMgr.Draw
		/// </summary>
		public int ZDepth { get; internal set; }

		public Actor(string name, [NotNull] Layer layer)
		{
			Initialize();
			Name = name;
			Layer = layer;
			Transform = AddComponent<Transform2D>();
			layer.AddActor(this);
			Start();
		}

		public virtual Vector2 Location
		{
			get => Transform.Location;
			set => Transform.Location = value;
		}

		public virtual Vector2 GlobalLocation
		{
			get => Transform.GlobalLocation;
			set => Transform.GlobalLocation = value;
		}

		public virtual float Rotation
		{
			get => Transform.Rotation;
			set => Transform.Rotation = value;
		}

		public virtual float GlobalRotation
		{
			get => Transform.GlobalRotation;
			set => Transform.GlobalRotation = value;
		}

		public virtual Vector2 Scale
		{
			get => Transform.LocalScale;
			set => Transform.LocalScale = value;
		}

		public Layer Layer { get; internal set; }

		public virtual void Initialize()
		{
		}

		public virtual void Start()
		{
		}

		// MonoGame Update
		public virtual void Update(GameTime gameTime)
		{
			foreach (var component in Components)
				component.Update(gameTime);
		}

		public virtual void Draw(GameTime gameTime, float layerZDepth)
		{
			foreach (var component in Components)
				component.Draw(gameTime, layerZDepth);
		}

		// Called after Update
		public virtual void LateUpdate(GameTime gameTime)
		{
			foreach (var baseComponent in Components) baseComponent.LateUpdate(gameTime);
		}

		//Extension stuff
		public T AddComponent<T>() where T : Component
		{
			var t = (T) Activator.CreateInstance(typeof(T), this);
			Components.Add(t);
			return t;
		}

		public void RemoveComponent(Component component)
		{
			if (Components.Contains(component))
			{
				component.Owner = null;
				Components.Remove(component);
			}
		}

		/// <summary>
		///     Remove parent and layer references.
		/// </summary>
		public void Detach()
		{
			Transform.Parent = null;

			Layer.RemoveActorRecursive(this);
		}

		public T GetComponent<T>()
		{
			return Components.OfType<T>().First();
		}

		public T[] GetComponents<T>()
		{
			return Components.OfType<T>().ToArray();
		}

		/// <summary>
		///     Set parent. If parent in different layer, this layer will be the same as parent layer
		/// </summary>
		/// <param name="parent"></param>
		public void SetParent([NotNull] Actor parent)
		{
			if (parent == this || Transform.Parent == parent.Transform)
				return;
			Transform.Parent = parent.Transform;
			OnParentChange?.Invoke(parent);
			parent.OnChildChange?.Invoke(parent.Transform.Childs.ToArray());
		}

		public void SetLayer([NotNull] Layer layer)
		{
			if (Transform.Parent != null)
			{
				// Set layer from child is forbidden!
				Log.PrintError("Trying to set layer on Child actor. Do it from the root Node!");
				return;
			}


			// set the new layer, take care everything needs to be done.
			layer.AddActor(this);
		}

		public override string ToString()
		{
			return Name + " (" + GetType() + ")";
		}
	}
}