using System.Reflection;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.PostgreSql;
using KST.Business.Infrastructure;
using KST.Business.Interfaces;
using KST.Business.Notifications.Services;
using KST.Business.Services;
using KST.DataAccess;
using KST.WEB.Infrastructure.ExceptionHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KST.WEB.Infrastructure;

public static class DependencyRegister
{
    public static WebApplicationBuilder AddHangfire(this WebApplicationBuilder builder)
    {
#if DEBUG
        var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");

        builder.Services.AddHangfire(options =>
        {
            options.UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UsePostgreSqlStorage(opt => opt.UseNpgsqlConnection(connectionString));
        });
        
        builder.Services.AddHangfireServer(serverOptions => { serverOptions.WorkerCount = 1; });
        builder.Services.Configure<HangfireOptions>(builder.Configuration.GetSection("HangfireSettings"));

#else
        builder.Services.AddSingleton<IBackgroundJobClient, FakeBackgroundJobClient>();
#endif
        return builder;
    }
    
    public static void UseHangfireUi(this IApplicationBuilder app)
    {
#if DEBUG
        var options = new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } };
        app.UseHangfireDashboard("/hangfire", options);
#endif
    }

    public static void AddEmailNotifications(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailNotificationOptions>(builder.Configuration.GetSection("EmailNotificationSettings"));
        builder.Services.AddMvc().AddRazorRuntimeCompilation();
        builder.Services.AddSingleton<ISmtpService, SmtpService>();
        builder.Services.AddScoped<RazorTemplateRenderer>();
        builder.Services.AddScoped<INotificationTemplateRendererService, NotificationTemplateRendererService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IHangfireNotificationService, HangfireNotificationService>();
    }
    
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
        builder.AddHangfire();
        builder.AddEmailNotifications();
        
        // Add File attachments
        builder.Services.Configure<AttachmentsOptions>(builder.Configuration.GetSection("AttachmentsSettings"));
        builder.Services.AddTransient<IFileUploadService, FileUploadService>();

        
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
            //var host = builder.Configuration.GetValue<string>("FrontendOrigin"); //TODO: doesnt work
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.AllowAnyOrigin()//.WithOrigins(host)
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        
        // Custom Services
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddScoped<ISpecialityService, SpecialityService>();
        builder.Services.AddScoped<IGroupService, GroupService>();
        builder.Services.AddScoped<IStudentService, StudentService>();
        builder.Services.AddScoped<ITeacherService, TeacherService>();
        builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        
        // Reports
        builder.Services.AddScoped<IExcelExportService, ExcelExportService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        
        // Main Services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }
}