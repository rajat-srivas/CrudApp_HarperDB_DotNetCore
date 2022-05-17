using HarperNetClient.models;
using Microsoft.Extensions.Configuration;

namespace Mozab.Service.Comments.Services
{
	public class HaperConfigurations : IHarperConfiguration
	{
		private IConfiguration _config;
		public HaperConfigurations(IConfiguration configs)
		{
			_config = configs;
		}
		public HarperDbConfiguration GetHarperConfigurations()
		{
			var dbConfigs = _config.GetSection("ConnectionString").Get<HarperDbConfiguration>();
			return dbConfigs;
		}
	}
}
