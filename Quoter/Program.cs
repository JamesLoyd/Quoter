using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quoter;
using Quoter.Behaviors;
using Quoter.Commands.Abstractions;
using Quoter.Queries;

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
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                services.Scan(scan =>
                    scan.FromAssemblyOf<Program>().FromApplicationDependencies()
                        .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
                        .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
                        .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime());
            
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                // cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.NotificationPublisher = new TaskWhenAllPublisher();
            });
        
        });
}