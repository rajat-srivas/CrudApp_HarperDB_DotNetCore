using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozab.SharedUtilities.Models
{
	public class PostCommentVM
	{
		public string Comment { get; set; }
		public string PostedBy { get; set; }
		public string PostId { get; set; }
		public string ParentCommentId { get; set; }
	}
}
