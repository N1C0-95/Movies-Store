﻿using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Model;
using Movies.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;
        public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
        {
            _ratingRepository = ratingRepository;
            _movieRepository = movieRepository;
        }
        public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token)
        {
            if(rating is <= 0 or > 5)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure
                    {
                        PropertyName = "Rating",
                        ErrorMessage = "Rating must be between 1 and 5"
                    }
                }
                );
            }

            var movieExist =await _movieRepository.ExistsByIdAsync(movieId, token);
            if (!movieExist)
            {
               return false;
            }

            return await _ratingRepository.RateMovieAsync(movieId, rating, userId, token);
        }

        public async Task<bool> DeleteRateMovieAsync(Guid movieId, Guid userId, CancellationToken token = default)
        {
            return await _ratingRepository.DeleteRateMovieAsync(movieId, userId, token);
        }

        public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
        {
            return await _ratingRepository.GetRatingsForUserAsync(userId, token);
        }
    }
}
