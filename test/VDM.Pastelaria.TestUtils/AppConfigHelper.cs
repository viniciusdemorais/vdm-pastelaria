using Microsoft.Extensions.Configuration;
using VDM.Pastelaria.Shareable.Models.Configs;

namespace VDM.Pastelaria.TestUtils;
public static class AppConfigHelper
{
    public static AppConfig GetAppConfig()
    {
        var configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json")
         .AddJsonFile("appsettings.Development.json")
         .Build();

        AppConfig appConfig = new();
        configuration.Bind(appConfig);
        return appConfig;
    }
}
