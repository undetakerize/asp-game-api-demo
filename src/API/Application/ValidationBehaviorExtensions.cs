using System.Reflection;
using FluentValidation;
using GameService.Application.Common.Behaviors;
using MediatR;

namespace GameService.Application;

public static class ValidationBehaviorExtensions
{
    public static IServiceCollection AddValidationBehaviorExtensions(this IServiceCollection services)
    {
        // Register FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // Register MediatR pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
        return services;
    }
}