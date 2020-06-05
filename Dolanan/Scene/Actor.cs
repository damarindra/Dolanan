using System;
using System.Collections.Generic;
using System.Linq;
using Dolanan.Components;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	/// <summary>
	/// Actor is an Entity.
	/// </summary>
	public class Actor : IGameCycle
	{
		public string Name;
		public Transform2D Transform;
		public Layer Layer { get; private set; }
		
		protected readonly List<Actor> Childs = new List<Actor>();

		public Actor[] GetChilds
		{
			get => Childs.ToArray();
		}
		
		// Component stuff
		// Render
		private readonly HashSet<Component> _components = new HashSet<Component>();

		public Actor(string name, Layer layer)
		{
			Initialize();
			Name = name;
			Layer = layer;
			Transform = Transform2D.Identity;
			layer.AddActor(this);
			Start();
		}

		//Extension stuff
		public T AddComponent<T>() where T : Component
		{
			T t = (T)Activator.CreateInstance(typeof(T), this);
			_components.Add(t);
			return t;
		}

		public void RemoveComponent(Component component)
		{
			if (_components.Contains(component))
				_components.Remove(component);
		}

		public T GetComponent<T>()
		{
			return _components.OfType<T>().First();
		}

		public T[] GetComponents<T>()
		{
			return _components.OfType<T>().ToArray();
		}

		public virtual void Initialize() { }
		public virtual void Start() { }

		// MonoGame Update
		public virtual void Update(GameTime gameTime)
		{
			// Reinitiate 
			Start();
			
			foreach (var component in _components)
				component.Update(gameTime);
		}

		public virtual void Draw(GameTime gameTime, float layerZDepth)
		{
			foreach (var component in _components)
				component.Draw(gameTime, layerZDepth);
		}

		// Called after Update
		public virtual void LateUpdate(GameTime gameTime)
		{
			foreach (Components.Component baseComponent in _components)
			{
				baseComponent.LateUpdate(gameTime);
			}
		}
	}
}