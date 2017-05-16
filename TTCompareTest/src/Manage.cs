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

		public Manage ()
		{
			_buttons = new List<Button> ();
			_timetable = new Timetable ();
			Fill_buttons ();
		}

		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 0, 550, 50, 100, "Back", 3, "Back"));
			_buttons.Add (new Button (Color.DarkGray, 850, 525, 75, 200, "Save", 2, "Save"));
			_buttons.Add (new Button (Color.DarkGray, 0, 0, 50, 100, "Clear", 3, "Clear"));
		}

		public void Handle ()
		{
			_timetabledisplayed = true;
			_timetable.Create ();
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
			case "Save":
				Save ();
				GlobalState.State = State.Back;
				break;
			case "Clear":
				Handle ();
				break;
			default:  
				break;  
			}
		}

		private void Save ()
		{
			/* The commented out code at the start and end of this function was attempting to 
			 * Allow the user to go back to the timetable entry page while entering a name
			 * It hasn't yet succeeded and will be left for the time being*/
			bool mouseCheck = false;
			bool textEntered = false;
			do {
				if (SwinGame.MouseClicked (MouseButton.LeftButton) && this.clicked (SwinGame.MousePosition ()) == "Back") 
				{
					mouseCheck = true;
				} else {
					_timetabledisplayed = false;
					SwinGame.StartReadingText (Color.Black, 20, Resources.GetFont ("Courier"), 500, 400);
					while (SwinGame.ReadingText ()) 
					{
						SwinGame.ProcessEvents ();
						this.Draw ();
					}
					_name = SwinGame.TextReadAsASCII ();
					using (StreamWriter File = new StreamWriter (_name + ".txt", false)) 
					{
						foreach (Block b in _timetable.Times) 
						{
							File.Write (b.Availability);
						}
					}
					//textEntered = true;
				}
			} while (!mouseCheck || !textEntered);
		}

		public void Draw ()
		{
			SwinGame.ClearScreen ();
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
			//I can probs think of a clever loop for this but it is do late at night for the thinking now (Lewis)
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

		public string clicked (Point2D pt)
		{
			foreach (Button b in _buttons)
			{
				if (b.IsAt (pt))
				{
					return b.Value;
				}
			}
			if (_timetabledisplayed)
			{
				_timetable.clicked (pt);	
			}
			return null;
		}
	}
}

