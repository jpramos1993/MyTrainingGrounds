

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.API.Controllers;

//[Authorize] 
[ApiController]
//[Route("api")]
public class MoviesController : ControllerBase
{
    //private readonly IMovieRepository _movieRepository;
    private readonly IMovieService _movieService;

    //public MoviesController(IMovieRepository movieRepository)
    //=> _movieRepository = movieRepository;
    public MoviesController(IMovieService movieService)
        => _movieService = movieService;

    //[HttpPost("movies")]
    [Authorize(AuthConstants.TrustMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request,
        CancellationToken token)
    {
        //var movie = new Movie
        //{
        //    Id = Guid.NewGuid(),
        //    Title = request.Title,
        //    YearOfRelease = request.YearOfRelease,
        //    Genres = request.Genres.ToList()
        //};
        var movie = request.MapToMovie();
        await _movieService.CreateAsync(movie, token);
        var response = new MovieResponse
        {
            Id = movie.Id,
            Slug = movie.Slug,
            Title = movie.Title,
            Genres = movie.Genres,
            YearOfRelease = movie.YearOfRelease
        };
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
        //return Created($"/{ApiEndpoints.Movies.Create}/{movie.Id}", response);
    }

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id, userId, token)
            : await _movieService.GetBySlugAsync(idOrSlug, userId, token);
        if (movie is null)
        {
            return NotFound();
        }

        var response = movie.MapToResponse();
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movies = await _movieService.GetAllAsync(userId, token);
        var moviesResponse = movies.MapToResponse();
        return Ok(moviesResponse);
    }

    [Authorize(AuthConstants.TrustMemberPolicyName)]
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request,
        CancellationToken token)
    {
        var movie = request.MapToMovie(id);
        var userId = HttpContext.GetUserId();

        var updated = await _movieService.UpdateAsync(movie, token);
        if(updated is null)
        {
            return NotFound();
        }

        var response = movie.MapToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, 
        CancellationToken token)
    {
        var deleted = await _movieService.DeleteByIdAsync(id, token);
        if(deleted is false)
        {
            return NotFound();
        }

        return Ok();
    }
}
