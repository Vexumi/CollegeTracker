using System.Reflection;
using KST.Business.Infrastructure;
using KST.Business.Interfaces;
using KST.Business.Services;
using KST.DataAccess;
using KST.WEB.Infrastructure.ExceptionHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KST.WEB.Infrastructure;

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
        builder.Services.AddDbContext<KSTDbContext>(options => options.UseNpgsql(connectionString));
        
        // Common Services
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddAutoMapper(MapperConfigurator.Configure);
        // builder.Services.AddHangfire(); //TODO add hangfire
        
        
        // Add Authorization
        builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Authorization"));
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Authorization:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Authorization:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Authorization:SecretKey"]!)),
                    ValidateIssuerSigningKey = true,
                };
            });
        builder.Services.AddAuthorization();
        
        // Add Cors
        builder.Services.AddCors(options =>
        {
            //var host = builder.Configuration.GetValue<string>("FrontendOrigin"); //TODO: doesnt working
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.AllowAnyOrigin()//.WithOrigins(host)
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        
        // Custom Services
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
        builder.Services.AddTransient<IUserService, UserService>();
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