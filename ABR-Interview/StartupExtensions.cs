namespace ABR_Interview;

public static class StartupExtensions
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheSettingsOptions>(configuration.GetSection(CacheSettingsOptions.SectionName));
        return services;
    }

    public static IServiceCollection InstallApplication(this IServiceCollection services)
    {
        services.AddSingleton<GuestSessionService>();
        return services;
    }

    public static IServiceCollection InstallInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<FileService>();
        return services;
    }
}
