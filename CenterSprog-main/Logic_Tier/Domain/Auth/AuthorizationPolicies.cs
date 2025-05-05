using Microsoft.Extensions.DependencyInjection;

namespace Domain.Auth;

public static class AuthorizationPolicies
{
    public static void AddPolicies(IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("MustBeStudent", a =>
                a.RequireAuthenticatedUser().RequireAssertion(context => context.User.IsInRole("student")
                ));

            options.AddPolicy("MustBeTeacher", a =>
                a.RequireAuthenticatedUser().RequireAssertion(context => context.User.IsInRole("teacher")
                ));

            options.AddPolicy("MustBeUser", a =>
                a.RequireAuthenticatedUser().RequireAssertion(context => context.User.IsInRole("teacher") || context.User.IsInRole("student")
                ));

            options.AddPolicy("MustBeAdmin", a =>
                a.RequireAuthenticatedUser().RequireAssertion(context => context.User.IsInRole("admin")
                ));
        });
    }
}