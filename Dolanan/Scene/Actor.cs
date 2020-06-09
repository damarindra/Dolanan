﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dolanan.Components;
using Dolanan.Engine;
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
		public string Name;

		public ParentChange OnParentChange;
		public Transform2D Transform;

		public Actor(string name, [NotNull]Layer layer)
		{
			Initialize();
			Name = name;
			Layer = layer;
			Transform = AddComponent<Transform2D>();
			layer.AddActor(this);
			Start();
		}

		public Layer Layer { get; }
		public Actor Parent { get; private set; }

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
	}
}