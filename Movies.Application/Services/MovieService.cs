using FluentValidation;
using Movies.Application.Model;
using Movies.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public class MovieService : IMovieService
    {
        //qui andrebbe messo il dto perchè non dovremmo legare con la entità di dominio

        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<Movie> _movieValidator;
        private readonly IRatingRepository _ratingRepository;
        private readonly IValidator<GetAllMoviesOptions> _optionsValidator;

        public MovieService(IMovieRepository movieRepository, IRatingRepository ratingRepository, IValidator<Movie> movieValidator, IValidator<GetAllMoviesOptions> optionsValidator)
        {
            _movieRepository = movieRepository;
            _movieValidator = movieValidator;
            _ratingRepository = ratingRepository;
            _optionsValidator = optionsValidator;
            
        }
        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie, token);
            return await _movieRepository.CreateAsync(movie, token);
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options,CancellationToken token = default)
        {
            await _optionsValidator.ValidateAndThrowAsync(options, token);
            return await _movieRepository.GetAllAsync(options,token);
        }

        public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetByIdAsync(id, userId,token);
        }

        public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetBySlugAsync(slug, userId,token);
        }

        public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie,token);
            var movieExist = await _movieRepository.ExistsByIdAsync(movie.Id,token);
            if (!movieExist)
            {
                return null;
            }

            await _movieRepository.UpdateAsync(movie,token);

            if(!userId.HasValue)
            {
                var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
                movie.Rating = rating;
                return movie;

            }

            var result = await _ratingRepository.GetRatingAsync(movie.Id, userId.Value, token);
            movie.Rating = result.Rating;
            movie.UserRating = result.UserRating;

            return movie;



            return movie;
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.DeleteAsync(id,token);
        }

        public Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default)
        {
            return _movieRepository.GetCountAsync(title, yearOfRelease, token);
        }
    }
}
