using System;
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

		public static bool TryParseToPoint(string str, out Point p)
		{
			p = Point.Zero;
			var xy = str.Split(',');
			if (xy.Length > 1)
			{
				p = new Point(Int32.Parse(xy[0]), Int32.Parse(xy[1]));
				return true;
			}

			return false;
		}
	}
}