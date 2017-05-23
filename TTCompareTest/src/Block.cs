using System;
using SwinGameSDK;

namespace TTCompare
{
	public class Block : Button
	{
		private Availability _availability;

		/// <summary>
		/// Class to represent the buttons in a timetable
		/// </summary>
		/// <param name="color">color of the button</param>
		/// <param name="x">x coordinate of the button in pixels</param>
		/// <param name="y">y coordinate of the button in pixels</param>
		/// <param name="height">height of the button in pixels</param>
		/// <param name="width">width of the button in pixels</param>
		/// <param name="text">Text to be displayed on the button</param>
		/// <param name="scale">Determines the position of the text on the button</param>
		/// <param name="value">The string returned when the button is clicked</param>
		public Block (Color color, float x, float y, int height, int width, string text, int scale, string value) : base (color, x, y, height, width, text, scale, value)
		{
			_availability = Availability.N;

		}


		/// <summary>
		/// Property to get/set the availability of the block (also changes the color of the block)
		/// </summary>
		public Availability Availability
		{
			get
			{
				return _availability;
			}
			set
			{
				_availability = value;
				switch (_availability)
				{
				case Availability.Y:
					Color = Color.Green;
					break;
				case Availability.N:
					Color = Color.Red;
					break;
				case Availability.M:  
					Color = Color.Yellow;
					break;  
				}
			}
		}
	}
}

