using System;
using System.Collections;

namespace MemoryGame
{
	public class UserResult : IComparable
	{
		public string userName = "";
		public string day = "";
		public int wastedChances = 0;
		public int seconds = 0;


         int IComparable.CompareTo(object obj)
        {
            UserResult result = (UserResult)obj;
            if (result.wastedChances < this.wastedChances)
                return 1;
            if (result.wastedChances > this.wastedChances)
                return -1;
            else if (result.seconds < this.seconds)
                return 1;
            else if (result.seconds > this.seconds)
                return -1;
            else
                return 0;
        }
    }
}

