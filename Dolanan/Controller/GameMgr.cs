using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Controller
{
	public static class GameMgr
	{
		public static GameMin Game { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }
		public static readonly SamplerState DefaultSamplerState = SamplerState.PointClamp;
		public static readonly BlendState DefaultBlendState = BlendState.AlphaBlend;
		internal static DrawState DrawState = DrawState.Draw;
		internal static RasterizerState RazterizerScissor = new RasterizerState() { ScissorTestEnable = true };

		public static void Init(GameMin game)
		{
			Game = game;
		}

		public static void Load(SpriteBatch spriteBatch)
		{
			SpriteBatch = spriteBatch;
		}

		/// <summary>
		/// Same as SpriteBatch.Begin, but using default settings
		/// </summary>
		/// <param name="transformMatrix"></param>
		public static void BeginDraw(Matrix? transformMatrix = null,
			SpriteSortMode sortMode = SpriteSortMode.Deferred,
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null,
			Effect effect = null)
		{
			SpriteBatch.Begin(transformMatrix: transformMatrix,
				blendState: blendState?? DefaultBlendState, 
				samplerState: samplerState?? DefaultSamplerState,
				depthStencilState: depthStencilState,
				rasterizerState: rasterizerState,
				effect: effect);
		}
		/// <summary>
		/// Same as SpriteBatch.Begin, but using default settings. This will draw in Draw method (World Space)
		/// </summary>
		public static void BeginDrawWorld(
			SpriteSortMode sortMode = SpriteSortMode.Deferred,
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null,
			Effect effect = null)
		{
			SpriteBatch.Begin(transformMatrix: Game.World.Camera.GetTopLeftMatrix(), 
				blendState: blendState?? DefaultBlendState,
				samplerState: samplerState?? DefaultSamplerState,
				depthStencilState: depthStencilState,
				rasterizerState: rasterizerState,
				effect: effect);
		}
		/// <summary>
		/// Same as SpriteBatch.Begin, but using default settings. Automatically select between Draw (World Space) / BackDraw (Window)
		/// </summary>
		public static void BeginDrawAuto(
			SpriteSortMode sortMode = SpriteSortMode.Deferred,
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null,
			Effect effect = null)
		{
			if(DrawState == DrawState.Draw)
				BeginDrawWorld(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
			else
				BeginDraw(null, sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
		}

		public static void EndDraw()
		{
			SpriteBatch.End();
		}
	}

	public enum DrawState
	{
		Draw, BackDraw, None
	}
}