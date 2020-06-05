#nullable enable
using System.Collections.Generic;
using CoreGame.Components;
using CoreGame.Engine;
using Microsoft.Xna.Framework;

namespace CoreGame.Scene
{
	/// <summary>
	/// Actor is an Entity.
	/// </summary>
	public class Actor : IGameCycle
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
		private readonly List<Component> _components = new List<Components.Component>();

		public Actor(string name, Layer layer)
		{
			Name = name;
			Layer = layer;
		}
		
		
		//Extension stuff
		public T AddComponent<T>() where T : Component, new()
		{
			T t = new T {Owner = this};
			_components.Add(t);
			t.Start();
			return t;
		}

		public void RemoveComponent(Component component)
		{
			if (_components.Contains(component))
				_components.Remove(component);
		}

		public virtual void Initialize() {
			foreach (Component baseComponent in _components)
			{
				baseComponent.Initialize();
			}
		}
		public virtual void Start() {
			foreach (Components.Component baseComponent in _components)
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

		public virtual void Draw(GameTime gameTime, float layerZDepth)
		{
			foreach (var component in _components)
			{
				component.Draw(gameTime, layerZDepth);
			}
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