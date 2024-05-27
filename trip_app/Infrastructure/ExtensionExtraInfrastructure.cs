using trip_app.Repository;

namespace trip_app.OutputFolder;

public static class ExtensionExtraInfrastructure
{
    public static void RegisterInfraServices(this IServiceCollection app)
    {
        app.AddScoped<ITripRepository, TripRepository>();
        app.AddDbContext<ApbdContext>();
    }
}