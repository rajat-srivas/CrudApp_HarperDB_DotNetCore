using Microsoft.AspNetCore.Mvc;
using Mozab.Service.Services;
using Mozab.SharedUtilities;
using System;
using System.Threading.Tasks;

namespace Mozab.Service.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SchemaController : ControllerBase
	{
		private ISchemaService _service;
		public SchemaController(ISchemaService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> CreateSchema(string schema)
		{
			try
			{
				var response = await _service.CreateSchema(schema);
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
		[HttpPost("table")]
		public async Task<IActionResult> CreateTable(string table, string schema)
		{
			try
			{
				var response = await _service.CreateTable(table, schema);
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

		[HttpDelete("table")]
		public async Task<IActionResult> DeleteTable(string table, string schema)
		{
			try
			{
				var response = await _service.DeleteTable(table, schema);
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

		[HttpDelete]
		public async Task<IActionResult> DeleteSchema(string schema)
		{
			try
			{
				var response = await _service.DeleteSchema(schema);
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

		[HttpPost("table/attribute")]
		public async Task<IActionResult> CreateAttribute(string table, string schema, string attribute)
		{
			try
			{
				var response = await _service.CreateAttribute(schema, table, attribute);
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

		[HttpDelete("table/attribute")]
		public async Task<IActionResult> DeleteAttribute(string schema, string table, string attribute)
		{
			try
			{
				var response = await _service.DeleteAttribute(schema, table, attribute);
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
