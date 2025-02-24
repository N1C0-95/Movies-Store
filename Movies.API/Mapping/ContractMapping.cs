using Movies.Application.Model;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.API.Mapping
{
    public static class ContractMapping
    {
        /*
         * this indica che è un extension method.
         * Un extension method è un metodo statico che permette di aggiungere funzionalità a un tipo esistente senza modificarne direttamente la definizione
         * In questo caso, MapToMovie è un metodo di estensione per la classe CreateMovieRequest, il che significa che puoi chiamarlo come se fosse un metodo di istanza su qualsiasi oggetto CreateMovieRequest.
         * cosi facendo non si modifica direttamente la classe originale, inoltre si evita di aggiungere metodi direttamente a classi che dovrebbero essere modelli di dati (DTO, Entity, ecc)
         */
        public static Movie MapToMovie(this CreateMovieRequest request) 
        {
            return new Movie()
            {
                Id =Guid.NewGuid(),
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
        {
            return new Movie()
            {
                Id = id,
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static MovieResponse MapToMovieResponse(this Movie movie)
        {
            return new MovieResponse()
            {
                Id = movie.Id,
                Slug = movie.Slug,
                Title = movie.Title,
                Rating = movie.Rating,
                UserRating = movie.UserRating,
                YearOfRealease = movie.YearOfRelease,
                Genres = movie.Genres
            };
        }

        public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies,
            int page, int pageSize, int totalCount) 
        {
            return new MoviesResponse()
            {
                Items = movies.Select(MapToMovieResponse),
                Page = page,
                PageSize = pageSize,
                Total = totalCount
                
            };
        

        }
        public static IEnumerable<MovingRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
        {
            return ratings.Select(x => new MovingRatingResponse
            {
                MovieId = x.MovieId,
                Rating = x.Rating,
                Slug = x.Slug
            });


        }
        public static GetAllMoviesOptions MapTopOptions(this GetAllMoviesRequest request)
        {
            return new GetAllMoviesOptions
            {
                Title = request.Title,
                YearOfRelease = request.Year,
                Page = request.Page,
                PageSize = request.PageSize
                
            };
        }

        public static GetAllMoviesOptions WithUser(this GetAllMoviesOptions options, Guid? userId)
        {
            return new GetAllMoviesOptions
            {
                Title = options.Title,
                YearOfRelease = options.YearOfRelease,
                UserId = userId,
                Page = options.Page,
                PageSize = options.PageSize
            };
        }
    }
}
