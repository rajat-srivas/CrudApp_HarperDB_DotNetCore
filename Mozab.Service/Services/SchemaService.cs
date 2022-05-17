using HarperNetClient;
using HarperNetClient.models;
using Mozab.Service.Comments.Services;
using Mozab.SharedUtilities;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Constants = Mozab.SharedUtilities.Constants;

namespace Mozab.Service.Services
{
	public class SchemaService : ISchemaService
	{
		private IHarperClientAsync _client;
		private IHarperConfiguration _config;
		private HarperDbConfiguration _dbConfigs;

		public SchemaService(IHarperConfiguration configs)
		{
			_config = configs;
			_dbConfigs = _config.GetHarperConfigurations();
			_client = new HarperClientAsync(_dbConfigs);
		}

		public async Task<string> CreateSchema(string schemaName)
		{
			try
			{
				if (string.IsNullOrEmpty(schemaName))
					throw new CustomException(Constants.InvalidTableOrSchemaName);

				var schemaExists = await CheckIfSchemaExists(schemaName);
				if (schemaExists)
					throw new CustomException(Constants.SchemaAlreadyExists);


				var response = await _client.CreateSchemaAsync(schemaName);
				if (response.IsSuccessful) _dbConfigs.Schema = schemaName;
				return JsonConvert.DeserializeObject<Content>(response.Content).Message;
			}
			catch
			{
				throw;
			}
		}

		public async Task<string> CreateTable(string tableName, string schemaName)
		{
			try
			{
				if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(schemaName))
					throw new CustomException(Constants.InvalidTableOrSchemaName);
				_dbConfigs.Schema = schemaName;

				var tableExists = await CheckIfTableExists(tableName, schemaName);
				if (tableExists) throw new CustomException(Constants.TableAlreadyExists);

				var response = await _client.CreateTableAsync(tableName, schemaName);
				return JsonConvert.DeserializeObject<Content>(response.Content).Message;
			}
			catch
			{
				throw;
			}
		}


		public async Task<string> CreateAttribute(string schema, string table, string attribute)
		{
			try
			{
				if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(schema))
					throw new CustomException(Constants.InvalidTableOrSchemaName);
				_dbConfigs.Schema = schema;

				if (string.IsNullOrEmpty(attribute))
					throw new CustomException("Attribute name cannot be empty");

				var tableExists = await CheckIfTableExists(table, schema);
				if (!tableExists)
					throw new CustomException(Constants.TableNotFound);

				var response = await _client.CreateAttributeAsync(schema, table, attribute);
				return JsonConvert.DeserializeObject<Content>(response.Content).Message;
			}
			catch
			{
				throw;
			}
		}

		public async Task<string> DeleteAttribute(string schema, string table, string attribute)
		{
			try
			{
				if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(schema))
					throw new CustomException(Constants.InvalidTableOrSchemaName);
				_dbConfigs.Schema = schema;

				if (string.IsNullOrEmpty(attribute))
					throw new CustomException("Attribute name cannot be empty");

				var tableExists = await CheckIfTableExists(table, schema);
				if (!tableExists)
					throw new CustomException(Constants.TableNotFound);

				var response = await _client.DropAttributeAsync(table, schema, attribute);
				return JsonConvert.DeserializeObject<Content>(response.Content).Message;
			}
			catch
			{
				throw;
			}
		}

		public async Task<string> DeleteSchema(string schema)
		{
			try
			{
				if (string.IsNullOrEmpty(schema))
					throw new CustomException(Constants.InvalidTableOrSchemaName);
				_dbConfigs.Schema = schema;

				var schemaExists = await CheckIfSchemaExists(schema);
				if (!schemaExists)
					throw new CustomException(Constants.SchemaNotFound);

				var response = await _client.DropSchemaAsync(schema);
				return JsonConvert.DeserializeObject<Content>(response.Content).Message;
			}
			catch
			{
				throw;
			}
		}

		public async Task<string> DeleteTable(string table, string schema)
		{
			try
			{
				if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(schema))
					throw new CustomException(Constants.InvalidTableOrSchemaName);
				_dbConfigs.Schema = schema;

				var tableExists = await CheckIfTableExists(table, schema);
				if (!tableExists)
					throw new CustomException(Constants.TableNotFound);

				var response = await _client.DropTableAsync(table, schema);
				return JsonConvert.DeserializeObject<Content>(response.Content).Message;
			}
			catch
			{
				throw;
			}
		}

		private async Task<bool> CheckIfSchemaExists(string schema)
		{
			try
			{
				var response = await _client.DescribeSchemaAsync(schema);
				return response.StatusCode == System.Net.HttpStatusCode.OK ? true : false;
			}
			catch
			{
				throw;
			}
		}

		private async Task<bool> CheckIfTableExists(string table, string schema)
		{
			try
			{
				var response = await _client.DescribeTableAsync(table, schema);
				return response.StatusCode == System.Net.HttpStatusCode.OK ? true : false;
			}
			catch
			{
				throw;
			}
		}

	}
}
