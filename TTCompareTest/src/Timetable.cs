using System;
using SwinGameSDK;

namespace TTCompare
{
	public class Timetable
	{
		private Block[,] _times;

		public Timetable ()
		{
			_times = new Block[48, 7];
		}

		public Block[,] Times {
			get {
				return _times;
			}	
			}

		public void Create ()
		{
			int x = 80;
			int y = 100;
			int temp = y;
			for (int i = 0; i < _times.GetLength (0); i++)
			{
				for (int p = 0; p < _times.GetLength (1); p++)
				{
					_times [i, p] = new Block (Color.Red, x, y, 15, 15, "", 2, "");
					y += 20;
				}
				x += 20;
				y = temp;
			}
			Console.WriteLine (x);
		}

		public void Draw ()
		{
			for (int i = 0; i < _times.GetLength (0); i++)
			{
				for (int p = 0; p < _times.GetLength (1); p++)
				{
					_times [i, p].Draw ();
				}
			}	
		}

		public void clicked (Point2D pt)
		{
			foreach (Block b in _times)
			{
				if (b.IsAt (pt))
				{
					if (b.Availability == Availability.N)
					{
						b.Availability = Availability.Y;
					}
					else if (b.Availability == Availability.Y)
					{
						b.Availability = Availability.M;
					}
					else
					{
						b.Availability = Availability.N;	
					}
				}
			}
		}
	}
}
