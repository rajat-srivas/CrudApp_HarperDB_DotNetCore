using System;
using System.Collections.Generic;

namespace Mozab.SharedUtilities.Models
{
	public class Posts
	{
		public string id { get; set; }

		public string Content { get; set; }

		public string PostedBy { get; set; }

		public DateTime PostedAt { get; set; }

		public string ImageURL { get; set; }
	}
	
}
