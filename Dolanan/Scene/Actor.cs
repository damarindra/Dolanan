using System;
using System.Collections.Generic;
using System.Linq;
using Dolanan.Components;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public delegate void ParentChange(Actor parent);
	
	/// <summary>
	/// Actor is an Entity.
	/// </summary>
	public class Actor : IGameCycle
	{
		public string Name;
		public Transform2D Transform;
		public Layer Layer { get; private set; }
		
		protected readonly List<Actor> Childs = new List<Actor>();

		public ParentChange OnParentChange;
		public Actor Parent { get; private set; }
		public Actor[] GetChilds
		{
			get => Childs.ToArray();
		}
		
		// Component stuff
		// Render
		protected readonly HashSet<Component> Components = new HashSet<Component>();
		
		public Actor(string name, Layer layer)
		{
			Initialize();
			Name = name;
			Layer = layer;
			Transform = AddComponent<Transform2D>();
			layer.AddActor(this);
			Start();
		}

		//Extension stuff
		public T AddComponent<T>() where T : Component
		{
			T t = (T)Activator.CreateInstance(typeof(T), this);
			Components.Add(t);
			return t;
		}

		public void RemoveComponent(Component component)
		{
			if (Components.Contains(component))
				Components.Remove(component);
		}

		public T GetComponent<T>()
		{
			return Components.OfType<T>().First();
		}

		public T[] GetComponents<T>()
		{
			return Components.OfType<T>().ToArray();
		}

		public void SetParent(Actor parent)
		{
			if (Parent != null)
				Parent.Childs.Remove(this);
			Parent = parent;
			Transform.Parent = parent.Transform;
			Parent.Childs.Add(this);
			
			OnParentChange?.Invoke(parent);
		}

		public virtual void Initialize() { }
		public virtual void Start() { }

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
			foreach (Components.Component baseComponent in Components)
			{
				baseComponent.LateUpdate(gameTime);
			}
		}
	}
}