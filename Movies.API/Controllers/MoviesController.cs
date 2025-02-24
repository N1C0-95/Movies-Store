using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Model;
using Movies.Application.Repository;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System.Diagnostics;

namespace Movies.API.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        //private readonly IMovieRepository _movieService; tolgo la dipendenza con il repository
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token )
        {
            var movie = request.MapToMovie();

            await _movieService.CreateAsync(movie, token);

            
            return CreatedAtAction(nameof(GetById), new { idOrSlug = movie.Id }, movie.MapToMovieResponse());

            //return Created($"{ApiEndpoints.Movies.Create}/{movie.Id}",movie.MapToMovieResponse());
        }

        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> GetById([FromRoute] string idOrSlug, CancellationToken token )
        {
            var userId = HttpContext.GetUserId();
                       
            var movie = Guid.TryParse(idOrSlug, out var id) 
                ? await _movieService.GetByIdAsync(id,userId, token) 
                : await _movieService.GetBySlugAsync(idOrSlug, userId, token);

            if (movie is null) { 
                return NotFound();
            }

            var response = movie.MapToMovieResponse();
            
            return Ok(response);
        }

        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var options = request.MapTopOptions()
                .WithUser(userId);
            var movie = await _movieService.GetAllAsync(options,token);
            var movieCount = await _movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
            
            return Ok(movie.MapToResponse(request.Page, request.PageSize, movieCount));
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token )
        {
            var userId = HttpContext.GetUserId();
            var movie = request.MapToMovie(id);

            var updatedMovie = await _movieService.UpdateAsync(movie,userId, token);
            if (updatedMovie is null)
            {
                return NotFound();
            }

            return Ok(updatedMovie.MapToMovieResponse());
        }
        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _movieService.DeleteAsync(id, token);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
