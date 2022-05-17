using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mozab.Service.Comments.Services;
using Mozab.SharedUtilities;
using Mozab.SharedUtilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mozab.Service.Comments.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentsController : ControllerBase
	{
		private ICommentsService _service;
		public CommentsController(ICommentsService service)
		{
			_service = service;
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetCommentById(string id)
		{
			try
			{
				var response = await _service.GetCommentById(id);
				return Ok(response);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest("Something went wrong!");
			}

		}

		[HttpGet("post/{id}")]
		public async Task<IActionResult> GetCommentsByPost(string id)
		{
			try
			{
				var response = await _service.GetAllCommentsByPost(id);
				return Ok(response);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest("Something went wrong!");
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateComment(PostCommentVM commentToAdd)
		{
			try
			{
				var response = await _service.AddNewComment(commentToAdd);
				return Created("", response);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest("Something went wrong!");
			}

		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateComment(string id, PostCommentVM postComment)
		{
			try
			{
				var response = await _service.UpdateComment(id, postComment); 
				return Ok(response);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest("Something went wrong!");
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCommentById(string id)
		{
			try
			{
				var response = await _service.DeleteCommentById(id);
				return Ok(response);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest("Something went wrong!");
			}
		}

		[HttpDelete("post/{id}")]
		public async Task<IActionResult> DeleteCommentByPost(string id)
		{
			try
			{
				var response = await _service.DeleteCommentByPost(id);
				return Ok(response);
			}
			catch(CustomException ex)
			{
				return BadRequest(ex.Message);
			}
			catch(Exception ex)
			{
				return BadRequest("Something went wrong!");
			}
		}
	}
}
