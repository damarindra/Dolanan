﻿using System;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace Dolanan.Core.Utility
{
	public static class DolananParser
	{
		public static string[] ParseCfg(string cfg)
		{
			string r = cfg.Replace(" ", "");
			var result = Regex.Split(r, "\\n");
			return result;
		}
		
		public static Vector2 ToVector2(string input)
		{
			var xy = input.Replace("{", "").Replace("}", "").Replace(" ", "").Replace("X", "").Replace(":", "").Split(new []{'Y', 'Z'});
			if (xy.Length > 1)
			{
				return new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
			}
			return Microsoft.Xna.Framework.Vector2.Zero;
		}
		public static Point ToPoint(string input)
		{
			var xy = input.Replace("{", "").Replace("}", "").Replace(" ", "").Replace("X", "").Replace(":", "").Split(new []{'Y', 'Z'});
			if (xy.Length > 1)
			{
				return new Point(Int32.Parse(xy[0]), Int32.Parse(xy[1]));
			}
			return Microsoft.Xna.Framework.Point.Zero;
		}
	}
}