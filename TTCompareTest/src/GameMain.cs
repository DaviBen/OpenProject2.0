using System;
using SwinGameSDK;

namespace TTCompare
{
    public class MainClass
    {
		public static void Main(string[] args)
        {
			//Initialize the screen
            SwinGame.OpenGraphicsWindow("GameMain", 1050, 600);

			//Clear the screen to get rid of the default content
			SwinGame.ClearScreen ();

			//Designating that the GUI starts on the main menu by changing the global state to main
			GlobalState.State = State.Main;

			//Initialize the differnt pages for the GUI
			MainMenu MainMenu = new MainMenu ();
			Manage ManageMenu = new Manage ();
			Compare CompareMenu = new Compare ();

			//Load in any requried resources
			Resources.LoadResources ();

			//Manage User Selections from each page of the GUI
			//Exits if the state is exit or window close is requested
			while ((GlobalState.State != State.Exit)&&(!(SwinGame.WindowCloseRequested())))
			{
				//If the global state is back, set it to main to steer the user back to the main meny when they click a back button on a sub page
				if (GlobalState.State == State.Back)
				{
					GlobalState.State = State.Main;
				}

				//Main control case
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