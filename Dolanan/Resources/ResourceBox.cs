﻿﻿using System.Collections.Generic;
using Dolanan.Controller;
using Dolanan.Engine;
using Microsoft.Xna.Framework.Content;

namespace Dolanan.Resources
{
	// TODO : ResourceBOX
	// TODO : ResourceBox allow read from json
	/// <summary>
	///     ResourceBox is a container for custom Object that need more process. Default type that supported by Pipeline just
	///     use GameMgr.Game.Load<>
	///     Ex : Load all AnimatedSprite from Aseprite. All process for creating AnimatedSprite done in Load function
	///     This technique inspired from Monofoxe
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ResourceBox<T>
	{
		private readonly bool _isUseMainGameContent;
		protected ContentManager ContentManager;

		protected Dictionary<string, T> ResourceHolder = new Dictionary<string, T>();

		/// <summary>
		///     Create a new ResourceBox.
		/// </summary>
		/// <param name="useMainGameContent"></param>
		public ResourceBox(bool useMainGameContent = true)
		{
			if (!useMainGameContent)
				ContentManager = new ContentManager(GameMgr.Game.Services);
			else ContentManager = GameMgr.Game.Content;

			_isUseMainGameContent = useMainGameContent;
		}

		// Local directory
		protected virtual string ContentDirectory => "";

		public virtual ResourceBox<T> Load()
		{
			return this;
		}

		/// <summary>
		///     Unload the GameContent
		/// </summary>
		/// <param name="safe">Ignore the Main Game.Content to be unloaded.</param>
		public virtual void Unload(bool safe = true)
		{
			if (safe && !_isUseMainGameContent)
				ContentManager.Unload();
			ResourceHolder.Clear();
		}

		public bool TryGet(string key, out T val)
		{
			return ResourceHolder.TryGetValue(key, out val);
		}

		/// <summary>
		///     create a new copy, so it will not reference to the ResourceHolder. Useful when ResourceBox type is like AseSprite
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public bool TryGetCopy(string key, out T val)
		{
			val = default;
			if (ResourceHolder.TryGetValue(key, out var value))
			{
				val = Cloner.Clone(value);
				return true;
			}

			return false;
		}
	}
}