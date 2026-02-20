using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeRoute.Application.Services.Implementations;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Currents.Implementations;
using SafeRoute.Domain.Currents.Interfaces;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Domain.Security.Interfaces;
using SafeRoute.Infrastructure.Data;
using SafeRoute.Infrastructure.Repositories.Implementations;
using SafeRoute.Infrastructure.Security.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Infrastructure.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<SafeRouteDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                )
            );

            // Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();

            // Currents
            services.AddScoped<ICurrentCompany, CurrentCompany>();
            services.AddScoped<ICurrentUser, CurrentUser>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrentCompany, CurrentCompany>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRuleViolationRepository, RuleViolationRepository>();

            // Services
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IRuleViolationService, RuleViolationService>();
            services.AddScoped<IProjectIngestionService, ProjectIngestionService>();
            services.AddScoped<IRulesEngineService, RulesEngineService>();
            services.AddScoped<IDoorRulesService, DoorRulesService>();
            services.AddScoped<IRampRulesService, RampRulesService>();

            // Security
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Token Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
