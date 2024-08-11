using CollegeTracker.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CollegeTracker.WEB.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        CollegeTrackerDbContext context)
    {
        context.Administrators.FirstOrDefault();
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<object> Get()
    {
        throw new DivideByZeroException("fdsafasdf");
    }
}