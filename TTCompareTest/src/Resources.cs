using System;
using SwinGameSDK;
using System.Collections.Generic;

namespace TTCompare
{
	public static class Resources
	{
		private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();

		public static void LoadResources ()
		{
			LoadFonts();
		}

		private static void LoadFonts ()
		{
			_Fonts.Add("Courier", SwinGame.LoadFont(SwinGame.PathToResource("cour.ttf", ResourceKind.FontResource), 14));
		}

		public static Font GetFont (string font)
		{
			return _Fonts[font];
		}
	}
}

