using FluentValidation;
using Movies.Application.Model;
using Movies.Application.Repository;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class MovieValidator : AbstractValidator<Movie>
    {
        private readonly IMovieRepository _movieRepository;
        public MovieValidator(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Genres)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.YearOfRelease)
                .LessThanOrEqualTo(DateTime.UtcNow.Year);

            RuleFor(x => x.Slug)
                .MustAsync(ValidateSlug)
                .WithMessage("This movie already exists in the system");
        }

        private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken token)
        {
            var existMovie = await _movieRepository.GetBySlugAsync(slug);

            if (existMovie is not null)
            {
                return existMovie.Id == movie.Id;
            }

            return existMovie is null;
        }
    }
}
