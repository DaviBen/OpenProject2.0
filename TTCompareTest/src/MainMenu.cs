/* The main menu screen for the GUI
 * includes functionality to draw itself and handle any user input
 */
using System;
using SwinGameSDK;
using System.Collections.Generic;

namespace TTCompare
{
	public class MainMenu
	{
		private List<Button> _buttons;

		/// <summary>
		/// The class to control the main menu
		/// </summary>
		public MainMenu ()
		{
			_buttons = new List<Button> ();
			Fill_buttons ();
		}

		/// <summary>
		/// Adds the manage and compare buttons to the array to be placed on the screen
		/// </summary>
		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 225, 300, 100, 200, "Manage Timetables", 5, "Manage"));
			_buttons.Add (new Button (Color.DarkGray, 575, 300, 100, 200, "Compare Timetables", 5, "Compare"));
		}

		/// <summary>
		/// Method to handle user input and run the main command loop of the main menu
		/// </summary>
		public void Handle ()
		{
			//Initialize the result of the command loop to null
			string result = null;

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

			//Check the value result picked up from the button clicked and navigate to the menu requested
			switch (result)
			{
			case "Manage":
				GlobalState.State = State.Manage;
				break;
			case "Compare":
				GlobalState.State = State.Compare;
				break;
			default:  
				break;  
			}
		}

		/// <summary>
		/// Method to output the main menu to the screen, including buttons and text. Called by the command loops.
		/// </summary>
		public void Draw ()
		{
			SwinGame.ClearScreen (SwinGame.RGBColor (50, 156, 255));

			//Draw the title to the screen
			SwinGame.LoadBitmapNamed ("logo", "background2.png");
			SwinGame.DrawBitmap ("logo", 0, 0);
		
			//SwinGame.DrawText ("Timetable Comparer", Color.Black, Resources.GetFont("Courier"), 425, 100);

			//Foreach button in the list, tell it to draw
			foreach (Button b in _buttons) 
			{
				b.Draw ();
			}
			//Refresh the screen so the menu is visable to the user
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
	}
}

