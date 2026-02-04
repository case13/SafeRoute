using SafeRoute.Api.Configurations;
using SafeRoute.Infrastructure.Configurations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        // Infrastructure
        builder.Services.AddInfrastructure(builder.Configuration);

        // Swagger
        builder.Services.AddSwaggerConfiguration();

        // Auth
        builder.Services.AddJwtAuthentication(builder.Configuration);

        builder.Services.AddAuthorizationConfiguration();



        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeRoute.Api v1");
                c.RoutePrefix = "swagger";
            });
        }
        Console.WriteLine("JWT KEY => " + builder.Configuration["JwtSettings:Key"]);
        Console.WriteLine("JWT ISSUER => " + builder.Configuration["JwtSettings:Issuer"]);
        Console.WriteLine("JWT AUDIENCE => " + builder.Configuration["JwtSettings:Audience"]);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}