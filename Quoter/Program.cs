using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quoter;
using Quoter.Behaviors;
using Quoter.Commands.Abstractions;
using Quoter.Queries;
using Quoter.Serialization;
using Serilog;

var builder = CreateHostBuilder();
builder.UseSerilog((context, serviceProvider, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console();
    loggerConfiguration.Enrich
        .WithProperty("Application", "Quoter")
        // .Enrich.WithSpan()
        // .Enrich.WithBaggage()
        .Enrich.FromLogContext();
});
builder.Build().Run();
return;

IHostBuilder CreateHostBuilder()
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IBot, Bot>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddHostedService<QuoterService>();
            services.AddDbContext<QuoterContext>();
            services.AddSingleton<ISerializer, DefaultSerializer>();
            services.Scan(scan => scan.FromAssemblyOf<Program>()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandRegistration)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            services.AddSingleton<ICommandRegister, CommandRegister>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                services.Scan(scan =>
                    scan.FromAssemblyOf<Program>().FromApplicationDependencies()
                        .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime()
                        .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime()
                        .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime());

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.NotificationPublisher = new TaskWhenAllPublisher();
            });
        });
}