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
		bool newTimetable = false;
		bool correctName = true;
		Availability _currentState = Availability.N;
		Availability _avail = Availability.Y;
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

			for (int i = 0; i < 7; i++) 
			{
				//TODO: blue for debug, set to transparent later
				_buttons.Add (new Button (Color.Blue, 60, 100 + (i * 20), 10, 15, "", 1, ((Day)i).ToString ()));
			}

			for (int i = 0; i < 24; i++) {
				DateTime time = new DateTime (0);
				time = time + TimeSpan.FromHours (i);
				//TODO: blue for debug, set to transparent later
				_buttons.Add (new Button (Color.Blue, 80 + (40 * i), 80, 10, 15, "", 1, time.ToString ("HH")));
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
						 SwinGame.MouseY () > 90 && SwinGame.MouseY () < 230 &&
						 SwinGame.MouseX () > 20) {
					result = this.clicked (SwinGame.MousePosition ());
				}
			}

			//Check the value result picked up from the button clicked and navigate to the menu or function requested
			if (result == "Back") {
				GlobalState.State = State.Back;
			} else if (result == "Save") {
				Save ();
			} else if (result == "Change") {
				_toChange = true;
				Handle ();
			} else if (result == null) { 
			} else if (result.Contains ("day")) {
				for (int i = 0; i < 7; i++) {
					if (((Day)i).ToString () == result)
						ChangeRow (i);
					
				}
			} else if (result.Length < 3) {
				for (int i = 0; i < 24; i++) {
					if (i.ToString ("D2")== result)
						ChangeCol (i*2);
				}
			} 
		}

		//changes the column to a next availability
		//TODO: Unsure why each buttons changes whole timetable to N
		//TODO: could be _avail shouldnt be global
		private void ChangeCol (int col)
		{
			for (int i = 0; i < 7; i++) {
				_timetable.Times [col, i].Availability = _avail;
			}
			_timetable.Draw ();
			switch (_avail) {
			case Availability.Y:
				_avail = Availability.M;
				break;
			case Availability.M:
				_avail = Availability.N;
				break;
			default:
				_avail = Availability.Y;
				break;
			}
		}

		//changes the row to a next availability
		//TODO: Unsure why each buttons changes whole timetable to N
		//TODO: could be _avail shouldnt be global
		private void ChangeRow (int row)
		{
			for (int i = 0; i < 48; i++) 
			{
				_timetable.Times [i, row].Availability = _avail;
			}
			_timetable.Draw ();
			switch (_avail) {
			case Availability.Y:
				_avail = Availability.M;
				break;
			case Availability.M:
				_avail = Availability.N;
				break;
			default:
				_avail = Availability.Y;
				break;
			}
		}

		// Might be more useful to use this or not. Will see.
		private void NextAvail (Availability avail)
		{
			switch (avail) {
			case Availability.Y:
				avail = Availability.M;
				break;
			case Availability.M:
				avail = Availability.N;
				break;
			default:
				avail = Availability.Y;
				break;
			}
		}

		/// <summary>
		/// Method that allows the user to change all blocks at once
		/// </summary>
		private void ChangeAll ()
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
			//SwinGame.Delay (500);
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
		private void ValidateNameEntry (char [] name)
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

			//If "Change ALL" was clicked, run the function
			if (_toChange) 
			{
				ChangeAll ();
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
				SwinGame.ClearScreen ();
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

		private void TTDraw ()
		{
			//Display the timetable
			//TODO loop this text drawing
			for (int i = 0; i < 7; i++) 
			{
				SwinGame.DrawText (((Day)i).ToString (), Color.Black, 8, 100 + (20 * i));
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
	}
}

