using Mozab.SharedUtilities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mozab.Service.Services
{
	public interface IPostService
	{
		Task<List<PostResponse>> GetAllPosts();

		Task<PostResponse> GetPostById(string zabId);

		Task<SharedUtilities.Models.Posts> AddNewPost(PostVM zabToAdd);

		Task<SharedUtilities.Models.Posts> UpdatePostById(string id, PostVM zabToUpdate);

		Task<bool> DeletePostById(string id);
	}
}
