using AutoMapper;
using HarperNetClient;
using HarperNetClient.models;
using Mozab.SharedUtilities;
using Mozab.SharedUtilities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constants = Mozab.SharedUtilities.Constants;

namespace Mozab.Service.Comments.Services
{
	public class CommentsService : ICommentsService
	{
		private IHarperClientAsync _client;
		private IHarperConfiguration _config;
		private HarperDbConfiguration _dbConfigs;
		private string Table_Name = "comments";
		private IMapper _mapper;

		public CommentsService(IHarperConfiguration configs, IMapper mapper)
		{
			_config = configs;
			_dbConfigs = _config.GetHarperConfigurations();
			_dbConfigs.Schema = "Mozabs";
			_client = new HarperClientAsync(_dbConfigs, Table_Name);
			_mapper = mapper;

		}

		public async Task<PostComment> AddNewComment(PostCommentVM commentToAdd)
		{
			try
			{
				if (string.IsNullOrEmpty(commentToAdd.Comment))
					throw new CustomException(Constants.CommentCannotBeEmpty);

				if (!string.IsNullOrEmpty(commentToAdd.ParentCommentId))
				{
					if (CheckIfCommentExists(commentToAdd.ParentCommentId) == null)
						throw new CustomException(Constants.InvalidParentCommentId);
				}

				var comment = _mapper.Map<PostComment>(commentToAdd);
				comment.PostedAt = DateTime.Now;

				var response = await _client.CreateRecordAsync<PostComment>(comment);
				var insertedCommentId = JsonConvert.DeserializeObject<Content>(response.Content).Inserted_Hashes[0];
				comment.id = insertedCommentId;
				return comment;
			}
			catch
			{
				throw;
			}
		}

		public async Task<bool> DeleteCommentById(string commentId)
		{
			try
			{
				if (CheckIfCommentExists(commentId) == null)
					throw new CustomException(Constants.InvalidCommentId);

				string queryToDeleteMainComment = $"DELETE FROM {_dbConfigs.Schema}.{Table_Name} WHERE id = \"{commentId}\"";
				var response = await _client.ExecuteQueryAsync(queryToDeleteMainComment);

				if (response.IsSuccessful)
				{
					string queryToDeleteCommentReplies = $"DELETE FROM {_dbConfigs.Schema}.{Table_Name} WHERE ParentCommentId = \"{commentId}\"";
					response = await _client.ExecuteQueryAsync(queryToDeleteCommentReplies);
				}
				return response.IsSuccessful;
			}
			catch
			{
				throw;
			}
		}

		public async Task<bool> DeleteCommentByPost(string postId)
		{
			try
			{
				if (CheckIfCommentsExistsForPost(postId) == null)
					throw new CustomException(Constants.NoCommentForTheJab);

				string queryToGetMainComments = $"SELECT id FROM {_dbConfigs.Schema}.{Table_Name} WHERE PostId = \"{postId}\" ";
				var response = await _client.ExecuteQueryAsync(queryToGetMainComments);
				var mainComments = (JsonConvert.DeserializeObject<List<PostComment>>(response.Content));
				foreach (var pc in mainComments)
				{
					await DeleteCommentById(pc.id);
				}
				return response.IsSuccessful;
			}
			catch
			{
				throw;
			}
		}

		public async Task<List<CommentResponse>> GetAllCommentsByPost(string postId)
		{
			try
			{
				string query = $"SELECT * FROM {_dbConfigs.Schema}.{Table_Name} WHERE PostId = \"{postId}\"";
				var response = await _client.ExecuteQueryAsync(query);
				var threads = new List<CommentResponse>();
				if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
				{
					var mainComments = (JsonConvert.DeserializeObject<List<PostComment>>(response.Content));
					foreach (var pc in mainComments)
					{
						threads.Add(await GetCommentById(pc.id));
					}
				}
				return threads;
			}
			catch
			{
				throw;
			}
		}

		public async Task<CommentResponse> GetCommentById(string commentId)
		{
			try
			{
				if (string.IsNullOrEmpty(commentId))
					throw new CustomException(Constants.InvalidCommentId);

				string query = $"SELECT * FROM {_dbConfigs.Schema}.{Table_Name} WHERE id = \"{commentId}\" OR ParentCommentId =  \"{commentId}\" ";
				var response = await _client.ExecuteQueryAsync(query);

				var commentsResponse = JsonConvert.DeserializeObject<List<PostComment>>(response.Content);

				if (commentsResponse != null && commentsResponse.Count > 0)
				{
					var parentComment = commentsResponse.Where(x => x.id == commentId).ToList()[0];
					var childComments = commentsResponse.Where(x => x.id != commentId).ToList();

					var comments = _mapper.Map<CommentResponse>(parentComment);
					comments.CommentReplies = childComments;
					return comments;
				}
				else
				{
					return new CommentResponse();
				}
			}
			catch
			{
				throw;
			}
		}

		public async Task<PostComment> UpdateComment(string id, PostCommentVM commentToUpdate)
		{
			try
			{
				var comment = _mapper.Map<PostComment>(commentToUpdate);

				if (CheckIfCommentExists(id) == null)
					throw new CustomException(Constants.InvalidCommentId);

				comment.id = id;
				var response = await _client.UpdateRecordAsync<PostComment>(comment);

				if (response.IsSuccessful)
				{
					return comment;
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

		public async Task<PostComment> CheckIfCommentExists(string commentId)
		{
			try
			{
				var response = await _client.GetByIdAsync(commentId);

				var comment = (JsonConvert.DeserializeObject<List<PostComment>>(response.Content));
				if (comment != null && comment.Count > 0)
					return comment[0];
				else
					return null;
			}
			catch
			{
				throw;
			}

		}

		public async Task<PostComment> CheckIfCommentsExistsForPost(string postId)
		{
			try
			{
				string query = $"SELECT * FROM {_dbConfigs.Schema}.{Table_Name} WHERE PostId = \"{postId}\"";
				var response = await _client.ExecuteQueryAsync(query);

				var comment = (JsonConvert.DeserializeObject<List<PostComment>>(response.Content));
				if (comment != null && comment.Count > 0)
					return comment[0];
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
