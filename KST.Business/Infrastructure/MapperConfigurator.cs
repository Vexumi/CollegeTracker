using AutoMapper;
using KST.Business.Infrastructure.MapperConfigs;

namespace KST.Business.Infrastructure;

public static class MapperConfigurator
{
    public static void Configure(IMapperConfigurationExpression cfg)
    {
        var profiles = typeof(UserMapperProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
        foreach (var profile in profiles)
        {
            cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
        }
    }
}