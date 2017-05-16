using System;
using System.Collections.Generic;
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
			get { return _times; }	
			set { _times = value;}
			}

		public void PopulateTimetable (char[] values)
		{
			int c = 0;
			for (int i = 0; i < _times.GetLength (0); i++) 
			{
				for (int j = 0; j < _times.GetLength (1); j++) 
				{
					switch (values[c]) {
					case 'Y':
						_times [i,j].Availability = Availability.Y;
						break;
					case 'N':
						_times [i,j].Availability = Availability.N;
						break;
					case 'M':
						_times [i,j].Availability = Availability.M;
						break;
					default:
						break;
					}
					c++;
				}
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
					if (b.NotYetAltered) 
					{
						if (b.Availability == Availability.N) {
							b.Availability = Availability.Y;
						} else if (b.Availability == Availability.Y) {
							b.Availability = Availability.M;
						} else {
							b.Availability = Availability.N;
						}
						b.NotYetAltered = false;
					}
				}
			}
		}
		// Returns the availability of the specified block
		// Taken from iteration 1, written by PT
		public Availability CheckBlock (int hour, int day)
		{
			return _times [hour, day].Availability;
		}
	}
}
