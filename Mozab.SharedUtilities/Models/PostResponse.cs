using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozab.SharedUtilities.Models
{
	public class PostResponse
	{
		public string id { get; set; }

		public string Content { get; set; }

		public string PostedBy { get; set; }

		public DateTime PostedAt { get; set; }

		public string ImageURL { get; set; }

		public List<CommentResponse> CommentsThread { get; set; }
	}
}
