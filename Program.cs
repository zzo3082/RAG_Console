// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Memory;
using RAG_Console.Hosts;
using RAG_Console.Services;
using RAG_Console.Services.Interfaces;
using Serilog;
using System.Text;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Logging;

class Program
{
    private const string embeding_CollectionName = "demo";
    static async Task Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.None);
            logging.ClearProviders();
        })
             .ConfigureHostConfiguration(config =>
             {
                 config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                 if (args != null)
                 {
                     config.AddCommandLine(args);
                 }
             })
.ConfigureAppConfiguration((hostContext, config) =>
{
    config.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
    .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true);
    /// 有其他環境的話
    //.AddJsonFile(path: $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    //Console.Write($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json");

})
.UseSerilog((hostingContext, loggerConfig) =>
loggerConfig.ReadFrom.Configuration(hostingContext.Configuration)
)
.ConfigureServices((hostContext, services) =>
{
    var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

    /// 注入服務
    services.AddScoped<IChatApiService, ChatApiService>();
    services.AddScoped<ILoginService, LoginService>();
    services.AddScoped<IRegisterService, RegisterService>();
    services.AddHostedService<ChatHost>();



    services.PostConfigure<HostOptions>(option =>
    {
        option.ShutdownTimeout = TimeSpan.FromSeconds(60);
    });
});
}
