using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repository;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton<IRatingRepository, RatingRepository>();
            services.AddSingleton<IRatingService, RatingService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
            
            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connenctionString)
        {
            services.AddSingleton<IDbConnectionFactory>( _ => 
            new NpgsqlConnectionFactory(connenctionString));

            services.AddSingleton<DbInitializer>();
            return services;
        }
    }
}
