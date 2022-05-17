using Microsoft.AspNetCore.Mvc;
using Mozab.Service.Services;
using Mozab.SharedUtilities;
using Mozab.SharedUtilities.Models;
using System;
using System.Threading.Tasks;

namespace Mozab.Service.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private IPostService _service;
		public PostsController(IPostService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllPosts()
		{
			try
			{
				var response = await _service.GetAllPosts();
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

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPostById(string id)
		{
			try
			{
				var response = await _service.GetPostById(id);
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
		public async Task<IActionResult> AddNewPost(PostVM postToAdd)
		{
			try
			{
				var response = await _service.AddNewPost(postToAdd);
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
		public async Task<IActionResult> UpdatePost(string id, PostVM postToUpdate)
		{
			try
			{
				var response = await _service.UpdatePostById(id, postToUpdate);
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
		public async Task<IActionResult> DeletePostById(string id)
		{
			try
			{
				var response = await _service.DeletePostById(id);
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

	}
}
