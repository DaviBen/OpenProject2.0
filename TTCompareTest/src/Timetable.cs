using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace TTCompare
{
	public class Timetable
	{
		private Block[,] _times;

		/// <summary>
		/// Class to represent a timetable
		/// </summary>
		public Timetable ()
		{
			_times = new Block[48, 7];
		}

		/// <summary>
		/// Property to get/set the _times array
		/// </summary>
		/// <returns>2D array of Block</returns>
		public Block[,] Times
		{
			get
			{
				return _times;
			}	
			set
			{
				_times = value;
			}
		}

		/// <summary>
		/// Method that takes in an array of chars and changes the values of the timetable to the parameter values
		/// </summary>
		/// <param name="values">Array of char to polulate the array with</param>
		public void PopulateTimetable (char[] values)
		{
			int c = 0;

			//Loop through the timetable
			for (int i = 0; i < _times.GetLength (0); i++) 
			{
				for (int j = 0; j < _times.GetLength (1); j++) 
				{
					//Set the availability of the value in question to the relevant value in the char array
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

		/// <summary>
		/// Initialize the array to default values and placement
		/// </summary>
		public void Create ()
		{
			//Set default screen placement for timetables
			int x = 80;
			int y = 100;
			int temp = y;

			//Loop through the array
			for (int i = 0; i < _times.GetLength (0); i++)
			{
				for (int p = 0; p < _times.GetLength (1); p++)
				{
					//Create the blocks in the array passing in default values
					_times [i, p] = new Block (Color.Red, x, y, 15, 15, "", 2, "");
					y += 20;
				}

				//Reposition the blocks
				x += 20;
				y = temp;
			}
			Console.WriteLine (x);
		}

		/// <summary>
		/// Method to draw a timetable to the screens
		/// </summary>
		public void Draw ()
		{
			//Loop through the array
			for (int i = 0; i < _times.GetLength (0); i++)
			{
				for (int p = 0; p < _times.GetLength (1); p++)
				{
					_times [i, p].Draw ();
				}
			}	
		}

		/// <summary>
		/// Method to change the availability of a block when clicked
		/// </summary>
		public void clicked (Point2D pt)
		{
			//Loop the array
			foreach (Block b in _times)
			{
				//Check that the loop element block is the one the mouse is over
				if (b.IsAt (pt))
				{
					//checks that a block was not already altered (used in the click and drag functionality)
					if (b.NotYetAltered) 
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
						b.NotYetAltered = false;
					}
				}
			}
		}

		/// <summary>
		/// A utility method to return an availability of a block at a given time and day
		/// </summary>
		/// <param name="hour">hour of desired block</param>
		/// <param name="day">day of desired block</param>
		/// <returns>Availability</returns>
		public Availability CheckBlock (int hour, int day)
		{
			return _times [hour, day].Availability;
		}
	}
}
