using System;
using SwinGameSDK;

namespace TTCompare
{
    public class MainClass
    {
		public static void Main(string[] args)
        {
            SwinGame.OpenGraphicsWindow("GameMain", 1050, 600);
			SwinGame.ClearScreen ();
			//Designating that the GUI starts on "MAIN PAGE"
			GlobalState.State = State.Main;
			// Creating the differnt pages for the GUI
			MainMenu MainMenu = new MainMenu ();
			Manage ManageMenu = new Manage ();
			Compare CompareMenu = new Compare ();
			Resources.LoadResources ();
			//Manage User Selections from each page of the GUI
			while ((GlobalState.State != State.Exit)&&(!(SwinGame.WindowCloseRequested())))
			{
				if (GlobalState.State == State.Back)
				{
					GlobalState.State = State.Main;
				}
				switch (GlobalState.State)
				{
				case State.Main:
					MainMenu.Handle ();
					break;
				case State.Manage:
					ManageMenu.Handle ();
					break;
				case State.Compare:  
					CompareMenu.Handle ();
					break;  
				}
			}
        }
    }
}