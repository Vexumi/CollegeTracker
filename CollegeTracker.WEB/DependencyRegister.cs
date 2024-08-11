using System.Reflection;
using CollegeTracker.Business.Infrastructure;
using CollegeTracker.Business.Interfaces;
using CollegeTracker.Business.Services;
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
        
        // Common Services
        builder.Services.AddAutoMapper(MapperConfigurator.Configure);
        // builder.Services.AddHangfire(); //TODO add hangfire
        
        // Custom Services
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
        builder.Services.AddTransient<ISpecialityService, SpecialityService>();
        builder.Services.AddTransient<IGroupService, GroupService>();
        builder.Services.AddTransient<ISubjectService, SubjectService>();
        builder.Services.AddTransient<IStudentService, StudentService>();

        
        // Main Services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }
}