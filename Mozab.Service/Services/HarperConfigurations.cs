using HarperNetClient.models;
using Microsoft.Extensions.Configuration;

namespace Mozab.Service.Comments.Services
{
	public class HarperConfigurations : IHarperConfiguration
	{
		private IConfiguration _config;
		public HarperConfigurations(IConfiguration configs)
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
