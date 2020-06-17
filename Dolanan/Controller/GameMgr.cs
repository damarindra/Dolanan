using System;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Controller
{
	public static class GameMgr
	{
		public static SamplerState DefaultSamplerState = SamplerState.PointClamp;
		public static BlendState DefaultBlendState = BlendState.AlphaBlend;
		public static DepthStencilState DefaultDepthStencil = DepthStencilState.None;
		internal static DrawState DrawState = DrawState.Draw;
		internal static RasterizerState RasterizerScissor = new RasterizerState {ScissorTestEnable = true};
		public static GameMin Game { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }

		public static void Init(GameMin game)
		{
			Game = game;
		}

		public static void Load(SpriteBatch spriteBatch)
		{
			SpriteBatch = spriteBatch;
		}

		/// <summary>
		///     Same as SpriteBatch.Begin, but using default settings
		/// </summary>
		/// <param name="transformMatrix"></param>
		public static void BeginDraw(Matrix? transformMatrix = null,
			SpriteSortMode sortMode = SpriteSortMode.FrontToBack,
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null,
			Effect effect = null)
		{
			SpriteBatch.Begin(transformMatrix: transformMatrix,
				blendState: blendState ?? DefaultBlendState,
				samplerState: samplerState ?? DefaultSamplerState,
				depthStencilState: depthStencilState ?? DefaultDepthStencil,
				rasterizerState: rasterizerState,
				effect: effect,
				sortMode: sortMode);
		}

		/// <summary>
		///     Same as SpriteBatch.Begin, but using default settings. This will draw in Draw method (World Space)
		/// </summary>
		public static void BeginDrawWorld(
			SpriteSortMode sortMode = SpriteSortMode.FrontToBack,
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null,
			Effect effect = null)
		{
			SpriteBatch.Begin(transformMatrix: Game.World.Camera.GetTopLeftMatrix(),
				blendState: blendState ?? DefaultBlendState,
				samplerState: samplerState ?? DefaultSamplerState,
				depthStencilState: depthStencilState ?? DefaultDepthStencil,
				rasterizerState: rasterizerState,
				effect: effect,
				sortMode: sortMode);
		}

		/// <summary>
		///     Same as SpriteBatch.Begin, but using default settings. Automatically select between Draw (World Space) / BackDraw
		///     (Window)
		/// </summary>
		public static void BeginDrawAuto(
			SpriteSortMode sortMode = SpriteSortMode.FrontToBack,
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null,
			Effect effect = null)
		{
			if (DrawState == DrawState.Draw)
				BeginDrawWorld(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
			else
				BeginDraw(null, sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
		}

		public static void EndDraw()
		{
			SpriteBatch.End();
		}

		public static void Draw(Actor actor,
			Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			Vector2 scale,
			SpriteEffects effects,
			float layerDepth)
		{
			SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
			RegisterZDepth(actor, layerDepth);
		}
		public static void Draw(Actor actor,
			Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			float scale,
			SpriteEffects effects,
			float layerDepth)
		{
			SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
			RegisterZDepth(actor, layerDepth);
		}
		public static void Draw(Actor actor,
			Texture2D texture,
			Rectangle destinationRectangle,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			SpriteEffects effects,
			float layerDepth)
		{
			SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
			RegisterZDepth(actor, layerDepth);
		}
		
		[Obsolete]
		public static void Draw(Actor actor,
			Texture2D texture,
			Vector2? position = null,
			Rectangle? destinationRectangle = null,
			Rectangle? sourceRectangle = null,
			Vector2? origin = null,
			float rotation = 0.0f,
			Vector2? scale = null,
			Color? color = null,
			SpriteEffects effects = SpriteEffects.None,
			float layerDepth = 0.0f)
		{
			SpriteBatch.Draw(texture, position, destinationRectangle, sourceRectangle, origin,rotation, scale, color, effects, layerDepth);
			RegisterZDepth(actor, layerDepth);
		}

		private static int drawCounter = 0;
		static void RegisterZDepth(Actor actor, float layerDepth)
		{
			actor.ZDepth = drawCounter;
			drawCounter ++;
		}

		public static void ResetState()
		{
			drawCounter = 0;
		}
	}

	public enum DrawState
	{
		Draw,
		BackDraw,
		None
	}
}