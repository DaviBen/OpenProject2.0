using System;

namespace TTCompare
{
	public static class GlobalState
	{
		static State state;

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

