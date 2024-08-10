using System.Reflection;
using CollegeTracker.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CollegeTracker.WEB;

public static class DependencyRegister
{
    public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
    {
        // Register Configuration
        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

        // Register DbContext
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<CollegeTrackerDbContext>(options => options.UseNpgsql(connectionString));
        
        // Custom Services
        // builder.Services.RegisterServices();
        
        // Main Services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }
}