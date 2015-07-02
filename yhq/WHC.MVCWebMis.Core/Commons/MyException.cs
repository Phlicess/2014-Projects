using System;

namespace WHC.Framework.Commons
{
	public class MyException : Exception
	{
		public MyException() : base("")
		{
		}

		public MyException(string message) : base(message)
		{
		}
	}
}
