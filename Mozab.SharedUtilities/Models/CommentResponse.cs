using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozab.SharedUtilities.Models
{
	public class CommentResponse
	{
		public string id { get; set; }
		public string Comment { get; set; }

		public DateTime PostedAt { get; set; }

		public string PostedBy { get; set; }

		public string PostId { get; set; }

		public string ParentCommentId { get; set; }

		public List<PostComment> CommentReplies { get; set; }
	}
}
