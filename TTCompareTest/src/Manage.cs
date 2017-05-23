/* The Manage Timetable screen for the GUI
 * includes functionality to draw itself and handle any user input
 */
using System;
using System.IO;
using SwinGameSDK;
using System.Collections.Generic;

namespace TTCompare
{
	public class Manage
	{
		private List<Button> _buttons;
		private Timetable _timetable;
		private bool _timetabledisplayed;
		string _name;
		bool _toChange = false;
		bool _changeRow = false;
		bool _changeCol = false;
		bool newTimetable = false;
		bool correctName = true;
		int rowOrCol;
		Availability _currentState = Availability.N;
		Availability _changeTo = Availability.Y;

		/// <summary>
		/// The class to control the manage menu
		/// </summary>
		public Manage ()
		{
			_buttons = new List<Button> ();
			_timetable = new Timetable ();
			Fill_buttons ();
		}

		private enum Day 
		{
			Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
		}

		/// <summary>
		/// Adds the back, save and change all buttons to the array to be placed on the screen
		/// </summary>
		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 0, 	550, 50, 100, "Back", 3, "Back"));
			_buttons.Add (new Button (Color.DarkGray, 850, 	525, 75, 200, "Save", 2, "Save"));
			_buttons.Add (new Button (Color.DarkGray, 0, 	0, 	 50, 200, "Change All", 3, "Change"));

			//Buttons for row selections
			for (int i = 0; i < 7; i++) 
			{
				_buttons.Add (new Button (Color.Transparent, 0, 100 + (i * 20), 15, 80, "", 1, ((Day)i).ToString ()));
			}

			//Buttons for column selections
			for (int i = 0; i < 24; i++) {
				DateTime time = new DateTime (0);
				time = time + TimeSpan.FromHours (i);
				_buttons.Add (new Button (Color.Transparent, 80 + (40 * i), 80, 20, 35, "", 1, time.ToString ("HH")));
			}

		}

		/// <summary>
		/// Method to handle user input and run the main command loop of the manage menu
		/// </summary>
		public void Handle ()
		{
			//Display and create the timetable
			//New timetable prevents the timetable from being overridden if a user is returning from the Name Entry page
			_timetabledisplayed = true;
			if (!newTimetable) _timetable.Create ();

			//Initialize the result of the command loop to null
			string result = null;

			//While the result string is null or the window close is not requested by the user
			while ((result == null) && (!(SwinGame.WindowCloseRequested ()))) {
				//Process user input
				SwinGame.ProcessEvents ();

				//Draw the manage menu
				this.Draw ();

				//If the user clicks or holds the left mouse button call the clicked function and set result to the return string
				if (SwinGame.MouseClicked (MouseButton.LeftButton)) {
					//Add all blocks in the timetable to the NotYetAltered list, part of the click and drag functionality
					foreach (Block b in _timetable.Times) {
						b.NotYetAltered = true;
					}
					result = this.clicked (SwinGame.MousePosition ());

				}
				//Used for click and drag when changing timetable blocks
				//Only in effect when the mouse is within the boundaries of the timetable
				else if (SwinGame.MouseDown (MouseButton.LeftButton) &&
						 SwinGame.MouseY () > 100 && SwinGame.MouseY () < 230 &&
						 SwinGame.MouseX () > 80) {
					result = this.clicked (SwinGame.MousePosition ());
				}
			}

			string ColOrRowToChange = result;
			if (result == null)
				return;
			if (result.Length < 3) {
				result = "Col";
			}
			else if (result.Contains ("day")) {
				result = "Row";
			}
			//Check the value result picked up from the button clicked and navigate to the menu or function requested
			switch (result)
			{
				case "Back": 
					GlobalState.State = State.Back;
					break;
				case "Save": 
					Save ();
					break;
				case "Change":
					_toChange = true;
					Handle ();
					break;
				case "Row":
					_changeRow = true;
					//Prevents the timetable from resetting 
					//Before this was included, every block was set to NO and then the selected Row was toggled
					newTimetable = true;
					ToChange (ColOrRowToChange);
					Handle ();
					break;
				case "Col":
					_changeCol = true;
					//Prevents the timetable from resetting 
					//Before this was included, every block was set to NO and then the selected Col was toggled
					newTimetable = true;
					ToChange (ColOrRowToChange);
					Handle ();
					break;
				default:
					break;
			} 
		}

		/// <summary>
		/// Changes to rowOrCol variable to reflect which row or col the user has selected 
		/// </summary>
		private void ToChange (string Test)
		{
			//User has selected a row (Day of the week)
			if (_changeRow) 
			{
				for (int i = 0; i< 7; i++) 
				{
					if (((Day)i).ToString () == Test) 
					{
						rowOrCol = i;
					}
				}
			}
			//User has selected a column (Hour of the day)
			else
			{
				for (int i = 0; i< 24; i++) 
				{
					if (i.ToString ("D2") == Test) 
					{
						rowOrCol = i * 2;
					}
						
				}		
			}
		}

		/// <summary>
		/// Changes the availability of the selected columns
		/// </summary>
		private void ChangeCol (int col)
		{
			//Toggles between the three availabilities for what should be changed to next
			Availability _avail = SwapAvailability (_timetable.Times [col, 0].Availability);
			//Goes through timetable and changes all blocks in corresponding column
			for (int j = 0; j< 7; j++) 
			{
				for (int i = 0; i< 48; i++) 
				{
					if (i == col) {
						_timetable.Times [i, j].Availability = _avail;
						_timetable.Times [i+1, j].Availability = _avail;
					} 
				}
			}
			//Stops call for ChangeCol()
			_changeCol = false;
		}

		/// <summary>
		/// Changes the availability of the selected row
		/// </summary>
		private void ChangeRow (int row)
		{
			//Toggles between the three availabilities for what should be changed to next
			Availability _avail = SwapAvailability (_timetable.Times [0, row].Availability);
			//Goes through timetable and changes all blocks in corresponding row
			for (int j = 0; j< 7; j++) 
			{
				for (int i = 0; i< 48; i++) 
				{
					if (j == row) 
					{
						_timetable.Times [i, j].Availability = _avail;
					}
				}
			}
			//Stops call for ChangeRow()
			_changeRow = false;
		}

		/// <summary>
		/// Toggles the availability variable
		/// </summary>
		private Availability SwapAvailability (Availability avail)
		{
			switch (avail) {
			case Availability.Y:
				return Availability.M;
			case Availability.M:
				return Availability.N;
			default:
				return Availability.Y;
			}
		}

		/// <summary>
		/// Method that allows the user to change all blocks at once
		/// </summary>
		public void ChangeAll ()
		{
			//Checks what the current state is and set the state to change to accordingly
			switch (_currentState) 
			{
				case Availability.Y:
					_changeTo = Availability.M;
					_currentState = Availability.M;
					break;
				case Availability.M:
					_changeTo = Availability.N;
					_currentState = Availability.N;
					break;
				default:
					_changeTo = Availability.Y;
					_currentState = Availability.Y;
					break;
			}

			//Change the state of all the blocks
			foreach (Block b in _timetable.Times) 
			{
				b.Availability = _changeTo;
			}

			//Set the _toChange trigger back to false
			_toChange = false;
		}

		/// <summary>
		/// Method to save entered data into a textfile and save it
		/// </summary>
		private void Save ()
		{
			//Hides the timetable in the draw function called in the save loop
			_timetabledisplayed = false;

			//Reads user input
			SwinGame.StartReadingText (Color.Black, 20, Resources.GetFont ("Courier"), 500, 400);

			//While the user is entering text into the dialog
			while (SwinGame.ReadingText ()) 
			{
				//Process user input
				SwinGame.ProcessEvents ();

				//Draw the main menu
				this.Draw ();

				//If the user clicks the back button stop saving
				if (SwinGame.MouseClicked (MouseButton.LeftButton) && this.clicked (SwinGame.MousePosition ()) == "Back" ||
				    SwinGame.KeyTyped (KeyCode.EscapeKey))
				{
					GlobalState.State = State.Manage;
					SwinGame.EndReadingText ();
					newTimetable = true;
					return;
				} 
			}

			_name = SwinGame.TextReadAsASCII ();
			ValidateNameEntry (_name.ToCharArray());
			if (!correctName) 
			{
				this.Save ();
				return;
			}

			using (StreamWriter File = new StreamWriter (_name + ".txt", false)) 
			{
				foreach (Block b in _timetable.Times) 
				{
					File.Write (b.Availability);
				}
			}
			// The next timetable accessed will be a new timetable
			newTimetable = false;
			//Set the global state to back so the user is directed to the main menu
			GlobalState.State = State.Back;
		}
		/// <summary>
		/// Validates the name entry.
		/// </summary>
		public void ValidateNameEntry (char [] name)
		{
			correctName = true;
			//checks for invalid character "*"
			foreach (char c in name) 
			{
				if (c == '*') 
				{
					correctName = false;
					return;
				}
			}
			//Confirms that the user has entered data
			if (name.Length == 0)
			{
				correctName = false;
				return;
			}
		}
		/// <summary>
		/// Method to output the manage menu to the screen, including buttons and text. Called by the command loops.
		/// </summary>
		public void Draw ()
		{
			SwinGame.ClearScreen ();
			SwinGame.LoadBitmapNamed ("background", "background.png");
			SwinGame.DrawBitmap ("background", 0, 0);
			//If "Change ALL" was clicked, run the function
			if (_toChange) 
			{
				ChangeAll ();
			}
			//Checks if the user has selected to change an entire row or column
			else if (_changeRow) 
			{
                ChangeRow (rowOrCol);
			} else if (_changeCol) 
			{
				ChangeCol (rowOrCol);
			}

			//Check if the user is saving and display the relevant screen
			if (_timetabledisplayed)
			{
				TTDraw ();
				foreach (Button b in _buttons) 
				{
					b.Draw ();
				}
			}
			else
			{
				//Display the Name Entry screen
				SwinGame.ClearScreen (SwinGame.RGBColor (50, 156, 255));
				SwinGame.DrawText ("Press ENTER to save", Color.Black, Resources.GetFont ("Courier"), 435, 385);	
				SwinGame.DrawText ("Name: ", Color.Black, Resources.GetFont ("Courier"), 450, 400);
				_buttons [0].Draw ();
				//Message to display if the name entered is blocked by the Access Control Lists
				if (!correctName) 
				{
					//variables used to properly centre the text
					string outputSanitize = "Invalid Name. Please try again";
					int offsetSanitize = 7 * outputSanitize.ToCharArray ().Length / 2;
					SwinGame.DrawText (outputSanitize, Color.Black, Resources.GetFont ("Courier"), 500 - offsetSanitize, 350);
				}
			}

			SwinGame.RefreshScreen (60);
		}

		//Display the timetable
		private void TTDraw ()
		{
			//Draw titles
			for (int i = 0; i < 7; i++) 
			{
				SwinGame.DrawText (((Day)i).ToString (), Color.Black, 3, 100 + (20 * i));
			}
			for (int i = 0; i < 24; i++) 
			{
				DateTime time = new DateTime (0);
				time = time + TimeSpan.FromHours (i);
				SwinGame.DrawText (time.ToString ("htt"), Color.Black, 80 + (40 * i), 80);
			}
			_timetable.Draw ();
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

			//If the timetable is displayed, also check the blocks
			if (_timetabledisplayed)
			{
				_timetable.clicked (pt);	
			}
			return null;
		}

		/// <summary>
		/// Readonly property to get the timetable (used in testing)
		/// </summary>
		/// <returns>Timetable</returns>
		public Timetable Timetable
		{
			get
			{
				return _timetable;
			}
		}

		/// <summary>
		/// Readonly property to get the correctname bool (used in testing)
		/// </summary>
		/// <returns>bool</returns>
		public bool CorrectName
		{
			get
			{
				return correctName;
			}
		}
	}
}

