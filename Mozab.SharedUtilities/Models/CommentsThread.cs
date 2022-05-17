using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozab.SharedUtilities.Models
{
	public class CommentsThread
	{
		public PostComment Comment { get; set; }

		public List<PostComment> CommentReplies { get; set; }
	}
}
