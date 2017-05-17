/* The Compare Timetables screen for the GUI
 * includes functionality to draw itself and handle any user input
 */
/*				for (int q = 0; q <= 50; q++) 
				{
					SwinGame.ProcessEvents ();
					SwinGame.DrawText ("b1", Color.Black, 400, 200);
					SwinGame.RefreshScreen ();
				}
*/
using System;
using System.IO;
using SwinGameSDK;
using System.Collections.Generic;

namespace TTCompare
{
	public class Compare
	{
		private List<Button> _buttons;
		private List<Timetable> _toCompare = new List<Timetable> ();
		private List<String> _TTnames = new List<String> ();
		List<int []> _yes = new List<int[]>();
		List<int []> _maybe = new List<int[]>();
		private Timetable _toPrint = new Timetable ();

		/// <summary>
		/// The class to control the compare menu
		/// </summary>
		public Compare ()
		{
			_buttons = new List<Button> ();
			Fill_buttons ();
		}

		/// <summary>
		/// Adds the back, compare and add buttons to the array to be placed on the screen
		/// </summary>
		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 0, 550, 50, 100, "Back", 3, "Back"));
			_buttons.Add (new Button (Color.DarkGray, 425, 525, 75, 200, "Compare", 3, "Compare"));
			_buttons.Add (new Button (Color.DarkGray, 850, 525, 75, 200, "Add", 2, "Add"));
		}

		/// <summary>
		/// Method to handle user input and run the main command loop of the compare menu
		/// </summary>
		public void Handle ()
		{
			//Initialize the result of the command loop to null
			string result = null;

			//Create a timetable setting values to defaults
			_toPrint.Create();

			//While the result string is null or the window close is not requested by the user
			while ((result == null)&&(!(SwinGame.WindowCloseRequested())))
			{
				//Process user input
				SwinGame.ProcessEvents ();

				//Draw the main menu
				this.Draw ();

				//If the user clicks the left mouse button call the clicked function and set result to the return string
				if (SwinGame.MouseClicked (MouseButton.LeftButton))
				{
					result = this.clicked (SwinGame.MousePosition ());
				}
			}

			//Check the value result picked up from the button clicked and navigate to the menu or function requested
			switch (result)
			{
				case "Back":
					GlobalState.State = State.Back;
					break;
				case "Add":
					LoadFile ();
					this.Handle ();
					break;
				case "Compare":
					CompareTimetables ();
					OutputTimetableInfo ();
					break;
				default:  
					break;  
			}
		}

		/// <summary>
		/// A utility method to check if a specific user button has been selected and return its text/name
		/// </summary>
		/// <param name="pt">A 2D point on the screen</param>
		/// <returns>string</returns>
		private string UserSelected (Point2D pt)
		{
			foreach (Button b in _buttons)
			{
				if (b.IsAt (pt))
				{
					return b.Text;
				}
			}
			return null;
		}

		/// <summary>
		/// Method to output the compare menu to the screen, including buttons and text. Called by the command loops.
		/// </summary>
		public void Draw ()
		{
			SwinGame.ClearScreen ();

			//Draw the various text elements to the screen
			SwinGame.DrawText ("Loaded Timetables:", Color.Black, Resources.GetFont("Courier"), 800, 80);
			string output = "Select 'ADD' to add a timetable";
			int offset = 7*output.ToCharArray ().Length / 2;
			SwinGame.DrawText (output, Color.Black, Resources.GetFont("Courier"), 500-offset , 370);
			int y = 105;

			//Draw the filenames to the screen, moving the y down every name drawn
			foreach (string s in _TTnames)
			{
				SwinGame.DrawText (s, Color.Black,Resources.GetFont("Courier"), 800, y);
				y += 25;
			}

			//Draw the buttons to the screen
			foreach (Button b in _buttons)
			{
				// Compare button only displays when there are enough timetables selected to compare
				if (b.Value == "Compare" && _toCompare.Count >= 2) 
				{
					b.Draw ();
				}
				else if (b.Value != "Compare") 
				{
					b.Draw();
				}

			}
			SwinGame.RefreshScreen (60);
		}

		/// <summary>
		/// A utility method to check which button on the main menu was clicked and return it's value
		/// </summary>
		/// <param name="pt">A 2D point on the screen</param>
		/// <returns>string</returns>
		public string clicked (Point2D pt)
		{
			foreach (Button b in _buttons)
			{
				if (b.IsAt (pt))
				{
					return b.Value;
				}
			}
			return null;
		}

		/// <summary>
		/// Method to load a requested file into the list of files
		/// </summary>
		private void LoadFile ()
		{
			//Initialize the result of the command loop to null
			string result = null;

			//Reads user input
			SwinGame.StartReadingText(Color.Black, 20, Resources.GetFont("Courier"), 500, 400);

			//While the result string is null or the window close is not requested by the user
			while ((SwinGame.ReadingText() && (GlobalState.State != State.Back)))
			{
				//Process user input
				SwinGame.ProcessEvents();

				//If the user clicks the back button stop loading
				if (SwinGame.MouseClicked (MouseButton.LeftButton))
				{
					result = this.clicked (SwinGame.MousePosition ());
					switch (result)
					{
					case "Back":
						GlobalState.State = State.Back;
						break;
					default:  
						break;  
					}
				}
				this.Draw ();
			}

			//Tries to read the file requested (adding file path) then sanatize it with the format method before adding it to the lists
			string _filename = SwinGame.TextReadAsASCII();
			if (!(GlobalState.State == State.Back))
			{
				try
				{
					using (StreamReader File = new StreamReader("..\\Debug\\" + _filename + ".txt"))
					{ 
						_toCompare.Add(FormatInputForTimetableEntry(File.ReadLine()));
						_TTnames.Add(_filename);
					}
				}
				catch
				{
					Console.WriteLine ("Error");
					//TODO Add real error message
				}
			}
		}

		/// <summary>
		/// Compares the times in the selected timetables and stores the results in a timetable object
		/// </summary>
		private void CompareTimetables ()
		{
			for (int k = 0; k < _toCompare.Count - 1; k++) {
				// each row of the timetable
				for (int i = 0; i < 7; i++) {
					// each column of the timetable
					for (int j = 0; j < 48; j++) {
						/* Every block is initiated to NO so we need to run all the code the first time (k=0)
						 * After the first round of comparisons, every block has been checked and modified
						 * If a PRINTblock is still NO then it's because one of the compared blocks was NO
						 * If this is the case we don't allow that block to be overwritten in the second round of comparisonso
						 */
						if (k == 0 || (k > 0 && _toPrint.Times[j,i].Availability != Availability.N)) 
						{
							
							/* These lines of IF statements check the two blocks being compared
							 * It confirms that both blocks are either YES or MAYBE
							 */
							if ((((_toCompare [k].CheckBlock (j, i) == _toCompare [k + 1].CheckBlock (j, i)) ||
								 ((_toCompare [k + 1].CheckBlock (j, i)).Equals (Availability.M))) &&
							    ((_toCompare [k].CheckBlock (j, i)).Equals (Availability.Y))) ||
									 ((_toCompare [k].CheckBlock (j, i)).Equals (Availability.M))) 
							{

								// Checks that both are YES, sets the PRINT block to YES if they are
								if ((_toCompare [k].CheckBlock (j, i)).Equals (Availability.Y) &&
								   (_toCompare [k + 1].CheckBlock (j, i)).Equals (Availability.Y) && 
								    _toPrint.Times [j, i].Availability != Availability.M) // This prevents a MAYBE from being overwritten with a YES
								{
									_toPrint.Times [j, i].Availability = Availability.Y;
								}
								// Checks that either of the two is MAYBE, sets the PRINT block to MAYBE if one or both are
								if ((_toCompare [k].CheckBlock (j, i)).Equals (Availability.M) ||
								   (_toCompare [k + 1].CheckBlock (j, i)).Equals (Availability.M)) 
								{
									_toPrint.Times [j, i].Availability = Availability.M;
								}
							}
						}
						// Sets PRINT block to NO if either block is NO 
						if (_toCompare [k].CheckBlock (j, i).Equals (Availability.N) || 
							_toCompare [k + 1].CheckBlock (j, i).Equals (Availability.N)) 
							{
								_toPrint.Times [j, i].Availability = Availability.N;
							}
					}
				}
			}
		}

		/// <summary>
		/// Prints the timetable of compared times and handles corresponding user input
		/// </summary>
		private void OutputTimetableInfo ()
		{
			//Display the data as a timetable
			string result = null;
			SwinGame.ClearScreen ();
			do {
				SwinGame.ProcessEvents ();

				TTDraw ();
				_buttons [0].Draw ();

				SwinGame.RefreshScreen ();
				// Display output untill user clicks again
				if (SwinGame.MouseClicked (MouseButton.LeftButton)) {
					result = this.clicked (SwinGame.MousePosition ());
					//Clears lists and objects so they're ready to go for another comparison
					_toCompare.Clear ();
					_toPrint.Times.Initialize ();
					_TTnames.Clear ();
				}
				
	
			} while (result != "Back");
		}

		/// <summary>
		/// Utility method that takes in a string from a textfile and returns a timetable of the values
		/// </summary>
		private Timetable FormatInputForTimetableEntry (String text)
		{
			char[] chars = text.ToCharArray ();
			Timetable result = new Timetable ();
			result.Create ();

			//Pass the array to the timetables populate method
			result.PopulateTimetable (chars);
			return result;
		}

		/// <summary>
		/// Format data to a printable format
		/// </summary>
		private String FormatListForOutput (int [] date)
		{
			string str;
			DateTime time = new DateTime (0);
			for (int i = 0; i < date [0]; i++) 
			{
				time = time + TimeSpan.FromMinutes (30);
			}
			str = time.ToString ("HH:mm");
			string day = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName ((DayOfWeek)date [1]);
			str = string.Format("{0} at {1}", day, str);
			return str;
		}

		/// <summary>
		/// Print the labels for the timetable, calls the timetable draw function as well
		/// </summary>
		private void TTDraw ()
		{
			SwinGame.DrawText ("Available Meeting Times:", Color.Black, 500, 50);
			SwinGame.DrawText ("Monday", Color.Black, 8, 100);
			SwinGame.DrawText ("Tuesday", Color.Black, 8, 120);
			SwinGame.DrawText ("Wednesday", Color.Black, 8, 140);
			SwinGame.DrawText ("Thursday", Color.Black, 8, 160);
			SwinGame.DrawText ("Friday", Color.Black, 8, 180);
			SwinGame.DrawText ("Saturday", Color.Black, 8, 200);
			SwinGame.DrawText ("Sunday", Color.Black, 8, 220);
			SwinGame.DrawText ("0AM", Color.Black, 80, 80);
			SwinGame.DrawText ("1AM", Color.Black, 120, 80);
			SwinGame.DrawText ("2AM", Color.Black, 160, 80);
			SwinGame.DrawText ("3AM", Color.Black, 200, 80);
			SwinGame.DrawText ("4AM", Color.Black, 240, 80);
			SwinGame.DrawText ("5AM", Color.Black, 280, 80);
			SwinGame.DrawText ("6AM", Color.Black, 320, 80);
			SwinGame.DrawText ("7AM", Color.Black, 360, 80);
			SwinGame.DrawText ("8AM", Color.Black, 400, 80);
			SwinGame.DrawText ("9AM", Color.Black, 440, 80);
			SwinGame.DrawText ("10AM", Color.Black, 480, 80);
			SwinGame.DrawText ("11AM", Color.Black, 520, 80);
			SwinGame.DrawText ("12AM", Color.Black, 560, 80);
			SwinGame.DrawText ("1PM", Color.Black, 600, 80);
			SwinGame.DrawText ("2PM", Color.Black, 640, 80);
			SwinGame.DrawText ("3PM", Color.Black, 680, 80);
			SwinGame.DrawText ("4PM", Color.Black, 720, 80);
			SwinGame.DrawText ("5PM", Color.Black, 760, 80);
			SwinGame.DrawText ("6PM", Color.Black, 800, 80);
			SwinGame.DrawText ("7PM", Color.Black, 840, 80);
			SwinGame.DrawText ("8PM", Color.Black, 880, 80);
			SwinGame.DrawText ("9PM", Color.Black, 920, 80);
			SwinGame.DrawText ("10PM", Color.Black, 960, 80);
			SwinGame.DrawText ("11PM", Color.Black, 1000, 80);
			_toPrint.Draw ();
		}

	}
}

