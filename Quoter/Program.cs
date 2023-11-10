using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quoter;

CreateHostBuilder().Build().Run();
return;

IHostBuilder CreateHostBuilder()
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IBot, Bot>();
            services.AddHostedService<QuoterService>();
            services.AddDbContext<QuoterContext>();
        });
}