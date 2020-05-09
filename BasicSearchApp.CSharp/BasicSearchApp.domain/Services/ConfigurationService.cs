using System.Configuration;

namespace BasicSearchApp.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}