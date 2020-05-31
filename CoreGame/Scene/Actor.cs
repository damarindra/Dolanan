#nullable enable
using System;
using System.Collections.Generic;
using CoreGame.Engine;
using Microsoft.Xna.Framework;

namespace CoreGame.Scene
{
	/// <summary>
	/// Actor is an Entity.
	/// </summary>
	public class Actor : IGameCycle, ICloneable
	{

		public string Name;
		public Transform2D Transform = Transform2D.Identity;
		public Layer Layer { get; private set; }
		
		protected readonly List<Actor> Childs = new List<Actor>();

		public Actor[] GetChilds
		{
			get => Childs.ToArray();
		}
		
		// Component stuff
		// Render
		private readonly List<Component.Component> _components = new List<Component.Component>();

		protected Actor(string name, Layer layer)
		{
			Name = name;
			Layer = layer;
		}
		
		
		//Extension stuff
		public T AddComponent<T>() where T : Component.Component, new()
		{
			T t = new T {Owner = this};
			_components.Add(t);
			return t;
		}

		public void RemoveComponent(Component.Component component)
		{
			if (_components.Contains(component))
				_components.Remove(component);
		}

		public virtual void Initialize() {
			foreach (Component.Component baseComponent in _components)
			{
				baseComponent.Initialize();
			}
		}
		public virtual void Start() {
			foreach (Component.Component baseComponent in _components)
			{
				baseComponent.Start();
			}
		}

		// MonoGame Update
		public virtual void Update(GameTime gameTime)
		{
			//transform.UpdateComponent(gameTime);
			foreach (var component in _components)
			{
				component.Update(gameTime);
			}
		}

		public virtual void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			foreach (var component in _components)
			{
				component.Draw(gameTime, layerZDepth);
			}
		}

		// Called after Update
		public virtual void LateUpdate(GameTime gameTime)
		{
			foreach (Component.Component baseComponent in _components)
			{
				baseComponent.LateUpdate(gameTime);
			}
		}

		public virtual object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}