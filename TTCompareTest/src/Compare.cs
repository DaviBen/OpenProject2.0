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
		private List<String> _toLoad = new List<String> ();
		List<int []> _yes;
		List<int []> _maybe;
		List<string> _compare;

		public Compare ()
		{
			_buttons = new List<Button> ();
			Fill_buttons ();
		}

		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 0, 550, 50, 100, "Back", 3, "Back"));
			_buttons.Add (new Button (Color.DarkGray, 900, 500, 100, 150, "Compare", 3, "Compare"));

			//Load the filenames and display as buttons
			/*TODO I'm not sure if this is the correct way to give the directory*/
			DirectoryInfo dinfo = new DirectoryInfo ("C:\\Users\\Benjamin\\GUIOpenProject\\TTCompareTest\\TTCompareTest\\TTCompareTest\\bin\\Debug");
			FileInfo [] Files = dinfo.GetFiles ("*.txt");
			// Used to move the next button another 15 pixels down
			int i = 1;
			foreach (FileInfo file in Files) 
			{
				_buttons.Add (new Button (Color.DarkGray, 800, 80 + 40 * i, 30, 220, file.Name, 3, "Add"));
				i++;
			}
		}

		public void Handle ()
		{
			string result = null;
			while ((result == null)&&(!(SwinGame.WindowCloseRequested())))
			{
				SwinGame.ProcessEvents ();
				this.Draw ();
				if (SwinGame.MouseClicked (MouseButton.LeftButton))
				{
					result = this.clicked (SwinGame.MousePosition ());
				}
			}
			switch (result)
			{
			case "Back":
				GlobalState.State = State.Back;
				break;
			case "Compare":
				loadFiles (_toLoad);
				CompareTimetables ();
				GlobalState.State = State.Back;
				break;
			case "Add":
				
				_toLoad.Add (UserSelected(SwinGame.MousePosition()));
				//Might be a source of some issues, just take note of it for now
				this.Handle ();
				break;
			default:  
				break;  
			}
		}

		//Check if a specific user button has been selected and return its text/name
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
		//Draw contents to the screen
		public void Draw ()
		{
			SwinGame.ClearScreen ();
			SwinGame.DrawText ("Timetables",Color.Black,850,80);
			foreach (Button b in _buttons)
			{
				b.Draw ();
			}
			SwinGame.RefreshScreen (60);
		}
		//Controls user input
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
		//Loads the text files specified by the user
		private void loadFiles (List<string> filenames)
		{
			Timetable toAdd = new Timetable ();
			foreach (string filename in filenames) 
			{
				StreamReader reader = new StreamReader (filename);
				string file = reader.ReadLine ();
				toAdd.populateTimetable (file.ToCharArray ());
				_toCompare.Add (toAdd);
			}
		}

		private void CompareTimetables ()
		{
			/* Compare functionality is a modified version of the code from iteration 1
			 * Originally written by PT, modified for iteration 2 by BD
			 */
			for (int k = 0; k < _toCompare.Count - 1; k++) {
				for (int i = 0; i < 48; i++) {
					for (int j = 0; j < 7; j++) {
						//FIXME: Wrong if more than 2
						//fix repeating values when more than 2
						if (((_toCompare [k].CheckBlock (i, j) == _toCompare[k + 1].CheckBlock (i, j)))
						    && (((_toCompare [k].CheckBlock (i, j)).Equals (Availability.Y))
						        || ((_toCompare [k].CheckBlock (i, j)).Equals (Availability.M)))) {
							if (((_toCompare [k].CheckBlock (i, j)).Equals (Availability.Y))) {
								// intArray[0] is Time
								// intArray[1] is Day
								// intArray[2] is Availability
								int [] intArray = new int [3];
								intArray [0] = i;
								intArray [1] = j;
								intArray [2] = (int)_toCompare [k].CheckBlock (i, j);
								_yes.Add (intArray);
							}
							if (((_toCompare [k].CheckBlock (i, j)).Equals (Availability.M))) {
								// intArray[0] is Time
								// intArray[1] is Day
								// intArray[2] is Availability
								int [] intArray = new int [3];
								intArray [0] = i;
								intArray [1] = j;
								intArray [2] = (int)_toCompare [k].CheckBlock (i, j);
								_maybe.Add (intArray);
							}
						}
					}
				}
			}
			//Check to see if there are any times that work before displaying data
			if (_yes == null) 
			{
				SwinGame.DrawText ("Available Meeting Times", Color.Black, 100, 100);
				int c = 0;
				foreach (int [] data in _yes) {
					SwinGame.DrawText (Format (_yes), Color.Black, 100, 115 + 15 * c);
					c++;
				}
			} else 
			{
				SwinGame.DrawText ("There are no available meeting times", Color.Black, 100, 100);
			}

			if (_maybe == null) 
			{
				SwinGame.DrawText ("Potential Meeting Times (Maybe)", Color.Black, 200, 100);
				int c = 0;
				foreach (int [] data in _maybe) {
					SwinGame.DrawText (Format (_maybe), Color.Black, 200, 115 + 15 * c);
					c++;
				}
			} else 
			{
				SwinGame.DrawText ("There are no potential (maybe) meeting times", Color.Black, 100, 100);
			}
		}

		//Format data to a readable format for the user
		// Taken from iteration 1, written by PT
		private string Format (List<int []> intArrays)
		{
			_compare.Clear ();
			foreach (int [] intArray in intArrays) {
				string str;
				DateTime time = new DateTime (0);
				for (int i = 0; i < intArray [0]; i++)
					time = time + TimeSpan.FromMinutes (30);
				str = time.ToString ("HH:mm");
				string day = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName ((DayOfWeek)intArray [1]);
				str = str + " " + day + " " + ((Availability)intArray [2]).ToString ();
				_compare.Add (str);
			}
			return string.Join ("\n", _compare);
		}
	}
}

