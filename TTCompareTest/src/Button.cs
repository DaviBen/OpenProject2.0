using System;
using SwinGameSDK;

namespace TTCompare
{
	public class Button
	{
		private Color _color;
		private float _x;
		private float _y;
		private int _height;
		private int _width;
		private string _text;
		private int _scale;
		private string _value;
		private bool _notYetAltered = true;

		/// <summary>
		/// Class to represent a button on the screen
		/// </summary>
		/// <param name="color">color of the button</param>
		/// <param name="x">x coordinate of the button in pixels</param>
		/// <param name="y">y coordinate of the button in pixels</param>
		/// <param name="height">height of the button in pixels</param>
		/// <param name="width">width of the button in pixels</param>
		/// <param name="text">Text to be displayed on the button</param>
		/// <param name="scale">Determines the position of the text on the button</param>
		/// <param name="value">The string returned when the button is clicked</param>
		public Button (Color color, float x, float y, int height, int width, string text, int scale, string value)
		{
			_color = color;
			_x = x;
			_y = y;
			_height = height;
			_width = width;
			_text = text;
			_scale = scale;
			_value = value;
		}


		public bool NotYetAltered {
			get { return _notYetAltered; }
			set { _notYetAltered = value; }
		}

		/// <summary>
		/// Method to draw the button to the screen
		/// </summary>
		public void Draw ()
		{
			SwinGame.FillRectangle (_color, _x, _y, _width, _height);

			// Centre-aligns the text in the buttons
			int X = (int)_x + (_width - _text.ToCharArray ().Length) / _scale;
			int Y = (int)_y + (_height - 15) / 2;
			SwinGame.DrawText (_text, Color.Black, Resources.GetFont ("Courier"), X, Y);

			//Add a outline when the button is hovered over by calling the hoverover function
			HoverOverRectangle (SwinGame.MousePosition ());
		}

		// TODO: I think this method can be removed
		public void MouseIsOver (Point2D pt)
		{
			if (SwinGame.PointInRect (pt, _x, _y, _width, _height)) {
				SwinGame.DrawRectangle (Color.Black, _x, _y, _width, _height);
				SwinGame.RefreshScreen ();
			}
		}

		/// <summary>
		/// Method to check if the button is located at the passed in point
		/// </summary>
		/// <param name="pt">2D point to be checked</param>
		/// <returns>bool</returns>
		public bool IsAt (Point2D pt)
		{
			if (SwinGame.PointInRect (pt, _x, _y, _width, _height)) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Called by the draw method, Gives a black outline to a button if the mouse is hovering over it 
		/// </summary>
		/// <param name="pt">2D point to be checked</param>
		private void HoverOverRectangle (Point2D pt)
		{
			if (this.IsAt (pt))
				{
					SwinGame.DrawRectangle (Color.Black, _x, _y, _width, _height);
				}

		}

		/// <summary>
		/// readonly property to return the value of the button
		/// </summary>
		public string Value
		{
			get
			{
				return _value;
			}
		}

		/// <summary>
		/// Property to get the x coordinate of a button
		/// </summary>
		public float X
		{
			get
			{
				return _x;
			}
		}

		/// <summary>
		/// Property to get/set the color of the button
		/// </summary>
		public Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
			}
		}

		/// <summary>
		/// Property to get/set the text on the button
		/// </summary>
		public String Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;

			}
		}

	}
}

