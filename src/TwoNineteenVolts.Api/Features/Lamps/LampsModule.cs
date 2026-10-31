namespace TwoNineteenVolts.Api.Features.Lamps;

public static class LampsModule
{
    public static IServiceCollection AddLamps(this IServiceCollection services)
    {
        services.AddOptions<ShellyOptions>()
            .BindConfiguration(ShellyOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<ShellyClient>();

        return services;
    }
}
