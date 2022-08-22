using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using VDM.Pastelaria.Data;
using VDM.Pastelaria.Data.Repositories;
using VDM.Pastelaria.Domain;
using VDM.Pastelaria.Domain.Contracts.Repositories;
using VDM.Pastelaria.Domain.Pipelines;
using VDM.Pastelaria.Shareable;

namespace VDM.Pastelaria.IoC;
[ExcludeFromCodeCoverage]
public static class AppServiceCollectionExtensions
{
    public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration); // AppConfig
        services.AddMediatR(new[] { typeof(DomainEntryPoint).Assembly });
        services.ConfigurePipelineBehaviors();
        services.ConfigureFluentValidation(typeof(ShareableEntryPoint).Assembly);
        services.AddTransient<LoggedHttpClientHandler>();
        services.AddHeaderPropagation(options => options.Headers.Add("X-Correlation-Id"));
        services.ConfigurePastelariaContext();
    }

    private static void ConfigurePastelariaContext(this IServiceCollection services)
    {
        services.AddDbContext<PastelariaContext>(options =>
            options.UseInMemoryDatabase("Pastelaria").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        services.AddHealthChecks().AddDbContextCheck<PastelariaContext>("Pastelaria", tags: new[] { "database" });

        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPastelRepository, PastelRepository>();
    }

    private static void ConfigurePipelineBehaviors(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ExceptionPipelineBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    }

    private static void ConfigureFluentValidation(this IServiceCollection services, params Assembly[] assemblies)
    {
        var abstractValidatorType = typeof(AbstractValidator<>);
        var validators = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(type => type.BaseType?.IsGenericType is true && type.BaseType.GetGenericTypeDefinition() == abstractValidatorType)
            .Select(Activator.CreateInstance)
            .ToArray();

        foreach (var validator in validators)
            services.AddSingleton(validator!.GetType().BaseType!, validator);
    }
}