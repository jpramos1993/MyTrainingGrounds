using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;
    private readonly IRatingRepository _ratingRepository;

    public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator, IRatingRepository ratingRepository)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
        _ratingRepository = ratingRepository;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        return await _movieRepository.CreateAsync(movie, token);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token)
        => _movieRepository.DeleteByIdAsync(id, token);

    public Task<IEnumerable<Movie>> GetAllAsync(Guid? userId, CancellationToken token)
        => _movieRepository.GetAllAsync(userId, token);

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId, CancellationToken token)
        => _movieRepository.GetByIdAsync(id, userId, token);

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId, CancellationToken token )
        => _movieRepository.GetBySlugAsync(slug, userId, token);

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId, CancellationToken token)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        var movieExists = await _movieRepository.ExistsById(movie.Id, token);
        if(!movieExists)
        {
            return null;
        }
        await _movieRepository.UpdateAsync(movie, token);
        
        if(!userId.HasValue)
        {
            var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
            movie.Rating = rating;
            return movie;
        }

        var ratings = await _ratingRepository.GetRatingAsync(movie.Id, userId.Value, token);
        movie.Rating = ratings.Rating;
        movie.UserRating = ratings.UserRating;
        return movie;
    }
}
