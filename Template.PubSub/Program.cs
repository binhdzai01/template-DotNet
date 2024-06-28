using Serilog;
using Template.PubSub;
using Template.PubSub.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var app = CreateHostBuilder(args).Build();

        var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetService<IPubSubService>().SubscribeInternal();

        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}