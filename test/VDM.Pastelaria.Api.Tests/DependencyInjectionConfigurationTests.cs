using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDM.Pastelaria.Domain;
using VDM.Pastelaria.IoC;

namespace VDM.Pastelaria.Api.Tests;
public class DependencyInjectionConfigurationTests
{
    [Fact]
    public void TodosOsRequestHandlersDevemPoderSerInstanciados()
    {
        var serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

        serviceCollection.AddLogging();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.ConfigureAppDependencies(configuration);

        var serviceProvider = serviceCollection.BuildServiceProvider(true);
        using var scope = serviceProvider.CreateScope();

        var requestHandlerType = typeof(IRequestHandler<,>);

        var handlers = typeof(DomainEntryPoint)
            .Assembly
            .ExportedTypes
            .SelectMany(type => type.GetInterfaces(), (@class, @interface) => (@class, @interface))
            .Where(t => t.@interface.IsGenericType)
            .Where(t => t.@interface.GetGenericTypeDefinition() == requestHandlerType);

        foreach (var (handlerType, @interface) in handlers)
        {
            scope.ServiceProvider.GetRequiredService(@interface);
        }

        scope.Should().NotBeNull();
    }
}
