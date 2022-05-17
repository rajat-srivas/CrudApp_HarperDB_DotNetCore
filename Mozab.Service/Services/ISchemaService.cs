using System.Threading.Tasks;

namespace Mozab.Service.Services
{
	public interface ISchemaService
	{
		public Task<string> CreateSchema(string schemaName);
		public Task<string> CreateTable(string tableName, string schemaName);

		public Task<string> DeleteSchema(string schema);
		public Task<string> DeleteTable(string table, string schema);
		public Task<string> CreateAttribute(string schema, string table, string attribute);
		public Task<string> DeleteAttribute(string schema, string table, string attribute);
	}
}
