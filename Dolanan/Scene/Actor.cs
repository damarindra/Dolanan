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
	public delegate void ParentChange(Actor parent);

	/// <summary>
	///     Actor is an Entity.
	/// </summary>
	public class Actor : IGameCycle
	{
		protected readonly List<Actor> Childs = new List<Actor>();

		// Component stuff
		// Render
		protected readonly List<Component> Components = new List<Component>();

		private Actor _parent;
		public string Name;

		public ParentChange OnParentChange;
		public Transform2D Transform;

		public Actor(string name, [NotNull] Layer layer)
		{
			Initialize();
			Name = name;
			Layer = layer;
			Transform = AddComponent<Transform2D>();
			layer.AddActor(this);
			Start();
		}

		public Layer Layer { get; internal set; }

		public Actor Parent
		{
			get => _parent;
			private set
			{
				if (_parent != null)
				{
					_parent.Childs.Remove(this);
					Transform.Parent = null;
				}

				_parent = value;
				if (_parent != null)
				{
					Transform.Parent = _parent.Transform;
					_parent.Childs.Add(this);
					if (_parent.Layer != Layer)
						SetLayer(_parent.Layer);
				}
			}
		}

		public Actor[] GetChilds => Childs.ToArray();

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
		/// Remove parent and layer references.
		/// </summary>
		public void Detach()
		{
			Parent = null;

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
		///     Set parent. If parent in different layer, layer on this automatically changed
		/// </summary>
		/// <param name="parent"></param>
		public void SetParent(Actor parent)
		{
			Parent = parent;
			OnParentChange?.Invoke(parent);
		}

		public void SetLayer([NotNull ]Layer layer)
		{
			if (Parent != null)
			{
				// Set layer from child is forbidden!
				Log.PrintError("Trying to set layer on Child actor. Do it from the root Node!");
				return;
			}

			// set the new layer, take care everything needs to be done.
			layer.AddActor(this);
		}
	}
}