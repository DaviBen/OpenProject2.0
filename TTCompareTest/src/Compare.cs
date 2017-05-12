/* The Compare Timetables screen for the GUI
 * includes functionality to draw itself and handle any user input
 */
using System;
using SwinGameSDK;
using System.Collections.Generic;

namespace TTCompare
{
	public class Compare
	{
		private List<Button> _buttons;

		public Compare ()
		{
			_buttons = new List<Button> ();
			Fill_buttons ();
		}

		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 0, 550, 50, 100, "Back", 3, "Back"));
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
			default:  
				break;  
			}
		}

		public void Draw ()
		{
			SwinGame.ClearScreen ();
			SwinGame.DrawText ("Compare screen to be filled", Color.Black, 325, 100);
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

