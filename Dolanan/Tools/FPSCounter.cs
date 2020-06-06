using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Tools
{
	public static class FPSCounter
	{
		static long TotalFrames { get; set; }
		static float TotalSeconds { get; set; }
		static float AverageFramesPerSecond { get; set; }
		static float CurrentFramesPerSecond { get; set; }

		public const int MAXIMUM_SAMPLES = 100;

		static Queue<float> _sampleBuffer = new Queue<float>();

		static void Update(float deltaTime)
		{
			CurrentFramesPerSecond = 1.0f / deltaTime;

			_sampleBuffer.Enqueue(CurrentFramesPerSecond);

			if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
			{
				_sampleBuffer.Dequeue();
				AverageFramesPerSecond = _sampleBuffer.Average(i => i);
			} 
			else
			{
				AverageFramesPerSecond = CurrentFramesPerSecond;
			}

			TotalFrames++;
			TotalSeconds += deltaTime;
		}
		public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
		{
			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Update(deltaTime);

			var fps = string.Format("FPS: {0}", AverageFramesPerSecond);

			spriteBatch.DrawString(font, fps, new Vector2(1, 1), Color.Black);
		}
	}
	
}