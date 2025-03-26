
using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into movies (id, slug, title, yearofrelease)
            values (@Id, @Slug, @Title, @YearOfRelease)
            """, movie));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    insert into genres (movieId, name)
                    values (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }));
            }
        }

        transaction.Commit();

        return result > 0;

    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(new CommandDefinition("""
            select m.*, string_agg(g.name, ',') as genres
            from movies m left join genres g on m.id = g.movieid
            group by id
            """));

        return result.Select(x => new Movie
        {
            Id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                select * from movies where id = @id
                """, new { id }));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                select name from genres where movieid = @id
                """, new { id }));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                select * from movies where id = @id
                """, new { slug }));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                select name from genres where movieid = @id
                """, new { id = movie.Id }));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            delete from genres where movieid = @id
            """, new { id = movie.Id }));

        foreach(var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                insert into genres (movieId, name)
                values (@MovieId, @Name)
                """, new { MovieId = movie.Id, Name = genre }));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease
            where id = @Id
            """, movie));

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    }

    public async Task<bool> ExistsById(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from movies where id = @id
            """, new { id }));
    }
}

public class HardcodedMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();

    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        var movie = _movies.SingleOrDefault(x => x.Slug == slug);
        return Task.FromResult(movie);
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        var movieIndex = _movies.FindIndex(x => x.Id == movie.Id);
        if (movieIndex == -1)
        {
            return Task.FromResult(false);
        }

        _movies[movieIndex] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _movies.RemoveAll(x => x.Id == id);
        var movieRemoved = removedCount > 0;
        return Task.FromResult(movieRemoved);
    }

    public async Task<bool> ExistsById(Guid id)
    {
        return _movies.Any(x => x.Id == id);
    }
}
