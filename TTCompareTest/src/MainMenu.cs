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

		public MainMenu ()
		{
			_buttons = new List<Button> ();
			Fill_buttons ();
		}

		//Add the two buttons to the list of buttons
		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 225, 300, 100, 200, "Manage Timetables", 5, "Manage"));
			_buttons.Add (new Button (Color.DarkGray, 575, 300, 100, 200, "Compare Timetables", 5, "Compare"));
		}

		//Handle user input
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
		// Draw the buttons and title to the screen
		public void Draw ()
		{
			SwinGame.ClearScreen ();
			SwinGame.DrawText ("Timetable Comparer", Color.Black, 425, 100);
			foreach (Button b in _buttons)
			{
				b.Draw ();
			}
			SwinGame.RefreshScreen (60);
		}

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

