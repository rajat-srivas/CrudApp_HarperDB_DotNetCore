using System;

namespace Mozab.SharedUtilities
{
	public class CustomException: Exception
	{
		public CustomException() { }

		public CustomException(string message)
			: base(message)
		{

		}
	}
}
