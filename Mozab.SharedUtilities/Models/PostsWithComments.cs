using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozab.SharedUtilities.Models
{
	public class PostsWithComments
	{
		public Posts Post { get; set; }

		public List<CommentsThread> PostComments { get; set; }
	}
}
