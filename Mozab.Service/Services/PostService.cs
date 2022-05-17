using AutoMapper;
using HarperNetClient;
using HarperNetClient.models;
using Mozab.Service.Comments.Services;
using Mozab.SharedUtilities;
using Mozab.SharedUtilities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Constants = Mozab.SharedUtilities.Constants;

namespace Mozab.Service.Services
{
	public class PostService : IPostService
	{
		private IHarperClientAsync _client;
		private IHarperConfiguration _config;
		private HarperDbConfiguration _dbConfigs;
		private string Table_Name = "posts";
		private IMapper _mapper;
		private ICommentsService _comments;
		public PostService(IHarperConfiguration configs, IMapper mapper, ICommentsService service)
		{
			_config = configs;
			_dbConfigs = _config.GetHarperConfigurations();
			_dbConfigs.Schema = "Mozabs";
			_client = new HarperClientAsync(_dbConfigs, Table_Name);
			_mapper = mapper;
			_comments = service;
		}
		public async Task<SharedUtilities.Models.Posts> AddNewPost(PostVM postToAdd)
		{
			try
			{
				if (string.IsNullOrEmpty(postToAdd.Content))
					throw new CustomException("Posts content cannot be empty");

				var post = _mapper.Map<Posts>(postToAdd);
				post.PostedAt = DateTime.Now;

				var response = await _client.CreateRecordAsync<Posts>(post);
				var insertedCommentId = JsonConvert.DeserializeObject<Content>(response.Content).Inserted_Hashes[0];
				post.id = insertedCommentId;
				return post;
			}
			catch
			{
				throw;
			}
		}

		public async Task<bool> DeletePostById(string id)
		{
			try
			{
				if (CheckIfPostExist(id) == null)
					throw new CustomException(Constants.PostNotFound);

				string query = $"DELETE FROM {_dbConfigs.Schema}.{Table_Name} WHERE id = \"{id}\"";
				var response = await _client.ExecuteQueryAsync(query);
				if (response.IsSuccessful)
				{
					await _comments.DeleteCommentByPost(id);
				}
				return response.IsSuccessful;
			}
			catch
			{
				throw;
			}
		}

		public async Task<List<PostResponse>> GetAllPosts()
		{
			try
			{
				string query = $"SELECT * FROM {_dbConfigs.Schema}.{Table_Name}";
				var response = await _client.ExecuteQueryAsync(query);
				var postResponse = JsonConvert.DeserializeObject<List<Posts>>(response.Content);
				if (postResponse != null && postResponse.Count > 0)
				{
					var posts = new List<PostResponse>();
					foreach (var pr in postResponse)
					{
						posts.Add(await GetPostById(pr.id));
					}
					return posts;
				}
				else
				{
					return new List<PostResponse>();
				}
			}
			catch
			{
				throw;
			}
		}

		public async Task<PostResponse> GetPostById(string postId)
		{
			try
			{
				if (string.IsNullOrEmpty(postId))
					throw new CustomException(Constants.PostNotFound);

				string query = $"SELECT * FROM {_dbConfigs.Schema}.{Table_Name} WHERE id = \"{postId}\" ";
				var response = await _client.ExecuteQueryAsync(query);
				var postResponse = JsonConvert.DeserializeObject<List<Posts>>(response.Content);

				if (postResponse != null && postResponse.Count > 0)
				{
					var commentThread = await _comments.GetAllCommentsByPost(postResponse[0].id);
					var postsData = _mapper.Map<PostResponse>(postResponse[0]);
					postsData.CommentsThread = commentThread;
					return postsData;

				}
				else
				{
					return new PostResponse();
				}

			}
			catch
			{
				throw;
			}
		}

		public async Task<SharedUtilities.Models.Posts> UpdatePostById(string id, PostVM postToUpdate)
		{
			try
			{
				var post = _mapper.Map<Posts>(postToUpdate);

				if (CheckIfPostExist(id) == null)
					throw new CustomException(Constants.PostNotFound);

				post.id = id;
				var response = await _client.UpdateRecordAsync<Posts>(post);

				if (response.IsSuccessful)
				{
					return post;
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public async Task<SharedUtilities.Models.Posts> CheckIfPostExist(string id)
		{
			try
			{
				var response = await _client.GetByIdAsync(id);

				var post = (JsonConvert.DeserializeObject<List<Posts>>(response.Content));
				if (post != null && post.Count > 0)
					return post[0];
				else
					return null;
			}
			catch
			{
				throw;
			}
		}
	}
}
