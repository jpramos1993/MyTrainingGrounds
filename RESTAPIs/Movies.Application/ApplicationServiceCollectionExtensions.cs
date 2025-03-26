
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;

namespace Movies.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMovieRepository, MovieRepository>();
        return serviceCollection;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddSingleton<IDbConnectionFactory>(_ => 
            new NpgsqlConnectionFactory(connectionString));
        serviceCollection.AddSingleton<DbInitializer>();
        return serviceCollection;
    }
}
