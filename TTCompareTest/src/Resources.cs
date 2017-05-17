using System;
using SwinGameSDK;
using System.Collections.Generic;

namespace TTCompare
{
	public static class Resources
	{
		private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();

		/// <summary>
		/// The class to load in resources need for the program
		/// </summary>
		public static void LoadResources ()
		{
			LoadFonts();
		}

		/// <summary>
		/// The method to load in the required fonts
		/// </summary>
		private static void LoadFonts ()
		{
			_Fonts.Add("Courier", SwinGame.LoadFont(SwinGame.PathToResource("cour.ttf", ResourceKind.FontResource), 14));
		}

		/// <summary>
		/// A utility method to return a font inputting it's name
		/// </summary>
		/// <param name="font">the name of the font requested</param>
		/// <returns>Font</returns>
		public static Font GetFont (string font)
		{
			return _Fonts[font];
		}
	}
}

