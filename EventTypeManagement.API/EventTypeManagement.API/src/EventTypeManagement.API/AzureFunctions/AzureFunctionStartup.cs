using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using EventTypeManagement.API.Data;

[assembly: FunctionsStartup(typeof(EventTypeManagement.API.AzureFunctions.AzureFunctionStartup))]

namespace EventTypeManagement.API.AzureFunctions
{
    public class AzureFunctionStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<MongoDbContext>();
            builder.Services.AddHttpClient();
        }
    }
}