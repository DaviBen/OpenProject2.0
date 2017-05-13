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

		public void Draw ()
		{
			SwinGame.FillRectangle (_color, _x, _y, _width, _height);
			SwinGame.DrawText (_text, Color.Black, _x + _width / _scale, _y + _height / _scale);  
		}

		public bool IsAt (Point2D pt)
		{
			if (SwinGame.PointInRect (pt, _x, _y, _width, _height))
			{
				return true;
			}
			return false;
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

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

