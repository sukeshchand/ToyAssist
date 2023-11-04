namespace ToyAssist.Web.Helpers
{
    using Microsoft.Extensions.Configuration;
    public class ConfigurationReader
    {
        public IConfigurationRoot Configuration { get; set; }

        public ConfigurationReader()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the path to your config file
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public string GetSetting(string key)
        {
            return Configuration[key];
        }
    }
}

