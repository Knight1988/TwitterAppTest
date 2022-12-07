using Microsoft.Extensions.Configuration;

namespace TwitterApp.Tests;

public static class Helper
{
    public static IConfiguration GetConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", true)
            .AddEnvironmentVariables() 
            .Build();
        return config;
    }
}