using Mozab.SharedUtilities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mozab.Service.Comments.Services
{
	public interface ICommentsService
	{
		public Task<PostComment> AddNewComment(PostCommentVM comment);
		public Task<List<CommentResponse>> GetAllCommentsByPost(string postId);
		public Task<CommentResponse> GetCommentById(string commentId);

		public Task<PostComment> UpdateComment(string id, PostCommentVM comment);

		public Task<bool> DeleteCommentById(string commentId);

		public Task<bool> DeleteCommentByPost(string postId);
	}
}
