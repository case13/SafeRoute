using Microsoft.Extensions.DependencyInjection;
using SafeRoute.BlazorServer.Services;
using SafeRoute.BlazorServer.Services.Implementations;
using SafeRoute.BlazorServer.Services.Implmentations;
using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.BlazorServer.ViewModels.Implementations;
using SafeRoute.BlazorServer.ViewModels.Interfaces;

namespace SafeRoute.BlazorServer.Configurations
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddFrontendServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthHttpService, AuthHttpService>();
            services.AddScoped<IProjectHttpService, ProjectHttpService>();
            services.AddScoped<IRuleViolationHttpService, RuleViolationHttpService>();
            services.AddScoped<IUserHttpService, UserHttpService>();
            services.AddScoped<ICompanyHttpService, CompanyHttpService>();

            services.AddScoped<ILoginViewModel, LoginViewModel>();
            services.AddScoped<IProjectViewModel, ProjectViewModel>();
            services.AddScoped<IRuleViolationViewModel, RuleViolationViewModel>();
            services.AddScoped<IUserViewModel, UserViewModel>();
            services.AddScoped<ICompanyViewModel, CompanyViewModel>();
            services.AddScoped<IAuthViewModel, AuthViewModel>();

            return services;
        }
    }
}
