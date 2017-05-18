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

		/// <summary>
		/// Adds the back, save and change all buttons to the array to be placed on the screen
		/// </summary>
		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 0, 	550, 50, 100, "Back", 3, "Back"));
			_buttons.Add (new Button (Color.DarkGray, 850, 	525, 75, 200, "Save", 2, "Save"));
			_buttons.Add (new Button (Color.DarkGray, 0, 	0, 	 50, 200, "Change All", 3, "Change"));
		}

		/// <summary>
		/// Method to handle user input and run the main command loop of the manage menu
		/// </summary>
		public void Handle ()
		{
			//Display and create the timetable
			_timetabledisplayed = true;
			_timetable.Create ();

			//Initialize the result of the command loop to null
			string result = null;

			//While the result string is null or the window close is not requested by the user
			while ((result == null)&&(!(SwinGame.WindowCloseRequested())))
			{
				//Process user input
				SwinGame.ProcessEvents ();

				//Draw the manage menu
				this.Draw ();
				//If the user clicks or holds the left mouse button call the clicked function and set result to the return string
				if (SwinGame.MouseClicked (MouseButton.LeftButton)) 
				{
					//Add all blocks in the timetable to the NotYetAltered list, part of the click and drag functionality
					foreach (Block b in _timetable.Times) 
					{
						b.NotYetAltered = true;
					}
					result = this.clicked (SwinGame.MousePosition ());
				}
				//Used for click and drag when changing timetable blocks
				//Only in effect when the mouse is within the boundaries of the timetable
				else if (SwinGame.MouseDown (MouseButton.LeftButton) &&
				         SwinGame.MouseY() > 90 && SwinGame.MouseY() < 230 &&
				         SwinGame.MouseX() > 20 )
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
			case "Save":
				Save ();
				break;
			case "Change":
				_toChange = true;
				Handle ();
				break;
			default:  
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
				if (SwinGame.MouseClicked (MouseButton.LeftButton) && this.clicked (SwinGame.MousePosition ()) == "Back") 
				{
					GlobalState.State = State.Manage;
					SwinGame.EndReadingText ();
					return;
				} 
			}

			//Saves the user's data with the input as the corresponding file name adding a .txt
			_name = SwinGame.TextReadAsASCII ();
			using (StreamWriter File = new StreamWriter (_name + ".txt", false)) 
			{
				foreach (Block b in _timetable.Times) 
				{
					File.Write (b.Availability);
				}
			}

			//Set the global state to back so the user is directed to the main menu
			GlobalState.State = State.Back;
		}

		/// <summary>
		/// Method to output the manage menu to the screen, including buttons and text. Called by the command loops.
		/// </summary>
		public void Draw ()
		{
			SwinGame.ClearScreen ();

			//If "Change ALl" was clicked, run the function
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
				SwinGame.ClearScreen ();
				SwinGame.DrawText ("Press ENTER to save", Color.Black, Resources.GetFont ("Courier"), 435, 385);	
				SwinGame.DrawText ("Name: ", Color.Black, Resources.GetFont ("Courier"), 450, 400);
				_buttons [0].Draw ();
			}

			SwinGame.RefreshScreen (60);
		}

		private void TTDraw ()
		{
			//Display the timetable
			//TODO loop this text drawing
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

