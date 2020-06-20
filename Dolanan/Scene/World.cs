﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dolanan.Collision;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	//TODO create system that load and unload world. generally changing scene. World is scene
	/// <summary>
	///     World, yes world. It also handle all collision.
	/// </summary>
	public class World : WorldCollision, IGameCycle
	{
		private int _defaultLayer = 1;
		public Camera Camera;

		public World()
		{
			Initialize();

			Start();
		}

		protected List<Layer> Layers { get; } = new List<Layer>();
		protected List<UILayer> UILayers { get; private set; } = new List<UILayer>();

		public int LayerCount => Layers.Count;

		/// <summary>
		///     Create new layer, if the layerZ already exist, return it
		/// </summary>
		/// <param name="layerZ"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T CreateLayer<T>(int layerZ) where T : Layer
		{
			foreach (var layer in Layers)
				if (layer.LayerZ == layerZ)
					return (T) layer;

			var result = (T) Activator.CreateInstance(typeof(T), this, layerZ);
			Layers.Add(result);
			UILayers.Clear();
			UILayers = Layers.OfType<UILayer>().ToList();
			UpdateLayerZOrder();
			return result;
		}

		public void DrawCollision()
		{
			foreach (var collisionCollider in Colliders)
				GameMgr.SpriteBatch.DrawStroke(new Rectangle((int) collisionCollider.Min.X,
					(int) collisionCollider.Min.Y,
					(int) collisionCollider.Size.X, (int) collisionCollider.Size.Y), Color.White);
		}

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
			return GetDefaultLayer().CreateActor<T>(name);
		}

		public T CreateActor<T>(string name, Layer layer) where T : Actor
		{
			return layer.CreateActor<T>(name);
		}

		public void UpdateLayerZOrder()
		{
			for (var i = 0; i < Layers.Count; i++) Layers[i].LayerZ = i / (float) LayerCount;
		}

		public void SwapLayer(Layer l1, Layer l2)
		{
			var indexL1 = Layers.IndexOf(l1);
			var indexL2 = Layers.IndexOf(l2);

			if (indexL1 < 0 || indexL2 < 0)
				return;

			Layers[indexL1] = l2;
			Layers[indexL2] = l1;

			var l1LayerZ = l1.LayerZ;
			l1.LayerZ = l2.LayerZ;
			l2.LayerZ = l1LayerZ;
		}

		public void SwapLayer(int indexL1, int indexL2)
		{
			if (indexL1 < 0 || indexL2 < 0 || indexL1 >= LayerCount || indexL2 >= LayerCount)
				return;

			var l1 = Layers[indexL1];
			Layers[indexL1] = Layers[indexL2];
			Layers[indexL2] = l1;

			var l1LayerZ = l1.LayerZ;
			l1.LayerZ = Layers[indexL2].LayerZ;
			Layers[indexL2].LayerZ = l1LayerZ;
		}

		#region Cycle

		public virtual void Initialize()
		{
		}

		public virtual void Start()
		{
			foreach (var name in (LayerName[]) Enum.GetValues(typeof(LayerName)))
				Layers.Add(new Layer(this, (int) name));

			Camera = GetDefaultLayer().CreateActor<Camera>("Camera");
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (var layer in Layers) layer.Update(gameTime);
			Camera.Update(gameTime);
		}

		public virtual void LateUpdate(GameTime gameTime)
		{
			foreach (var layer in Layers) layer.LateUpdate(gameTime);
			Camera.LateUpdate(gameTime);
		}

		/// <summary>
		///     Drawing
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="layerZDepth">Not important, for Layer / Actor / Component only</param>
		public virtual void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			foreach (var layer in Layers)
				if (!UILayers.Contains(layer) || ((UILayer) layer).UISpace == UISpace.World)
					layer.Draw(gameTime, layerZDepth);
		}

		/// <summary>
		///     Render after BackBufferRender (Whole game world render). It useful for debugging, fixed rendering to screen, etc
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="worldRect">The back buffer render size</param>
		public virtual void BackDraw(GameTime gameTime, Rectangle worldRect)
		{
			// SpriteBatch.Draw(ScreenDebugger.Pixel, new Rectangle(Camera.ScreenToCameraSpace(Mouse.GetState().Position),
			// 	new Point(5, 5)), Color.Yellow);
			foreach (var layer in UILayers)
				if (layer.UISpace == UISpace.Window)
					layer.BackDraw(gameTime, worldRect);
		}

		#endregion
	}
}