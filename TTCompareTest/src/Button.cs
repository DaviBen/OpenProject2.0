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

		public void Draw ()
		{
			SwinGame.FillRectangle (_color, _x, _y, _width, _height);
			// Centre-aligns the text in the buttons
			int X = (int)_x + (_width - _text.ToCharArray ().Length) / _scale;
			int Y = (int)_y + (_height - 15) / 2;
			SwinGame.DrawText (_text, Color.Black, Resources.GetFont ("Courier"), X, Y);
			HoverOverRectangle (SwinGame.MousePosition ());
		}
		public void MouseIsOver (Point2D pt)
		{
			if (SwinGame.PointInRect (pt, _x, _y, _width, _height)) {
				SwinGame.DrawRectangle (Color.Black, _x, _y, _width, _height);
				SwinGame.RefreshScreen ();
			}
		}
		// Checks if the button is at the specified location
		public bool IsAt (Point2D pt)
		{
			if (SwinGame.PointInRect (pt, _x, _y, _width, _height)) {
				return true;
			}
			return false;
		}
		// Gives a black outline to a button if the mouse is hovering over it
		private void HoverOverRectangle (Point2D pt)
		{
			if (this.IsAt (pt))
				{
					SwinGame.DrawRectangle (Color.Black, _x, _y, _width, _height);
				}

		}

		public string Value {
			get {
				return _value;
			}
		}

		public float X {
			get {
				return _x;
			}
		}

		public Color Color {
			get {
				return _color;
			}
			set {
				_color = value;
			}
		}

		public String Text {
			get {
				return _text;
			}
			set {
				_text = value;

			}
		}

	}
}

