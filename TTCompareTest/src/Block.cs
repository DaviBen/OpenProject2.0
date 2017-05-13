using System;
using SwinGameSDK;

namespace TTCompare
{
	public class Block : Button
	{
		private Availability _availability;

		public Block (Color color, float x, float y, int height, int width, string text, int scale, string value) : base (color, x, y, height, width, text, scale, value)
		{
			_availability = Availability.N;
		}

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

