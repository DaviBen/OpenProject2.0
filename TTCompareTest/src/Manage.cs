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

		public Manage ()
		{
			_buttons = new List<Button> ();
			_timetable = new Timetable ();
			Fill_buttons ();
		}

		public void Fill_buttons ()
		{
			_buttons.Add (new Button (Color.DarkGray, 0, 	550, 50, 100, "Back", 3, "Back"));
			_buttons.Add (new Button (Color.DarkGray, 850, 	525, 75, 200, "Save", 2, "Save"));
			_buttons.Add (new Button (Color.DarkGray, 0, 	0, 	 50, 200, "Change All", 3, "Change"));
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
					foreach (Block b in _timetable.Times) 
					{
						b.NotYetAltered = true;
					}
					result = this.clicked (SwinGame.MousePosition ());
				}
				else if (SwinGame.MouseDown (MouseButton.LeftButton))
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
				break;
			case "Change":
				_toChange = true;
				Handle ();
				break;
			default:  
				break;  
			}
		}

		//Allows user to change all blocks to YES, MAYBE or NO at once
		private void ChangeAll ()
		{
			//Checks what the current state is and changes all the blocks accordingly
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

			foreach (Block b in _timetable.Times) 
			{
				b.Availability = _changeTo;
			}

			_toChange = false;
			SwinGame.Delay (500);
		}

		private void Save ()
		{
			_timetabledisplayed = false;
			//Reads user input while back button hasn't been clicked
			SwinGame.StartReadingText (Color.Black, 20, Resources.GetFont ("Courier"), 500, 400);
			while (SwinGame.ReadingText ()) 
			{
				SwinGame.ProcessEvents ();
				this.Draw ();
				if (SwinGame.MouseClicked (MouseButton.LeftButton) && this.clicked (SwinGame.MousePosition ()) == "Back") 
				{
					GlobalState.State = State.Manage;
					SwinGame.EndReadingText ();
					return;
				} 
			}
			//Saves the user's data with the input as the corresponding file name
			_name = SwinGame.TextReadAsASCII ();
			using (StreamWriter File = new StreamWriter (_name + ".txt", false)) 
			{
				foreach (Block b in _timetable.Times) 
				{
					File.Write (b.Availability);
				}
			}
			GlobalState.State = State.Back;
		}

		public void Draw ()
		{
			SwinGame.ClearScreen ();
			if (_toChange) 
			{
				ChangeAll ();
			}
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

