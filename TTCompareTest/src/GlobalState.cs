using System;

namespace TTCompare
{
	public static class GlobalState
	{
		static State state;

		/// <summary>
		/// Static property to get/set the global state which controls the flow of the GUI
		/// </summary>
		/// <returns>string</returns>
		public static State State
		{
			get
			{
				return state;
			}
			set
			{
				state = value;
			}
		}
	}
}

