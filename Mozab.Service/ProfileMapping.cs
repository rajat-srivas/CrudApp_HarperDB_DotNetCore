using AutoMapper;
using Mozab.SharedUtilities.Models;

namespace Mozab.Service.Comments
{
	public class ProfileMapping : Profile
	{
		public ProfileMapping()
		{
			CreateMap<PostVM, Posts>().ReverseMap();
			CreateMap<PostCommentVM, PostComment>().ReverseMap();
			CreateMap<Posts, PostResponse>().ReverseMap();
			CreateMap<PostComment, CommentResponse>().ReverseMap();

		}
	}
}
