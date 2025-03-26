﻿

using Microsoft.AspNetCore.Mvc;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.API.Controllers;

[ApiController]
//[Route("api")]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
        => _movieRepository = movieRepository;

    //[HttpPost("movies")]
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        //var movie = new Movie
        //{
        //    Id = Guid.NewGuid(),
        //    Title = request.Title,
        //    YearOfRelease = request.YearOfRelease,
        //    Genres = request.Genres.ToList()
        //};
        var movie = request.MapToMovie();
        await _movieRepository.CreateAsync(movie);
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

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieRepository.GetByIdAsync(id)
            : await _movieRepository.GetBySlugAsync(idOrSlug);
        if (movie is null)
        {
            return NotFound();
        }

        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();
        var moviesResponse = movies.MapToResponse();
        return Ok(moviesResponse);
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        var updated = await _movieRepository.UpdateAsync(movie);
        if(updated is false)
        {
            return NotFound();
        }

        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _movieRepository.DeleteByIdAsync(id);
        if(deleted is false)
        {
            return NotFound();
        }

        return Ok();
    }
}
