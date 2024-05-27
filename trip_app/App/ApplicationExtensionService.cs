using trip_app.Service;

namespace trip_app.OutputFolder;

public static class ExtensionServiceApplication
{
    public static void RegisterApplicationServices(this IServiceCollection app)
    {
        app.AddScoped<ITripService, TripService>();
    }
}