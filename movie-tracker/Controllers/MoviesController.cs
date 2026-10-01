using Microsoft.AspNetCore.Mvc;
using movie_tracker.DTOs.Movie;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private static readonly List<Movie> Movies =
    [
        new Movie
        {
            Id = 1,
            Title = "Interstellar",
            Description = "A science fiction film about space and time.",
            ReleaseYear = 2014,
            Genre = "Science Fiction",
            Rating = 8.7
        },
        new Movie
        {
            Id = 2,
            Title = "The Dark Knight",
            Description = "A superhero film about Batman.",
            ReleaseYear = 2008,
            Genre = "Action",
            Rating = 9.0
        }
    ];
    
    [HttpGet]
    public ActionResult<IEnumerable<MovieDto>> GetAll(string? genre, int? releaseYear)
    {
        var movies = Movies.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(genre))
        {
            movies = movies.Where(movie =>
                movie.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
        }

        if (releaseYear.HasValue)
        {
            movies = movies.Where(movie =>
                movie.ReleaseYear == releaseYear.Value);
        }

        var movieDtos = movies.Select(movie => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            Genre = movie.Genre,
            Rating = movie.Rating
        });

        return Ok(movieDtos);
    }
    
    [HttpGet("{id}")]
    public ActionResult<MovieDto> GetById(int id)
    {
        var movie = Movies.FirstOrDefault(movie => movie.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        var movieDto = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            Genre = movie.Genre,
            Rating = movie.Rating
        };

        return Ok(movieDto);
    }
    
    [HttpPost]
    public ActionResult<MovieDto> Create(CreateMovieDto dto)
    {
        var movie = new Movie
        {
            Id = Movies.Count == 0 ? 1 : Movies.Max(movie => movie.Id) + 1,
            Title = dto.Title,
            Description = dto.Description,
            ReleaseYear = dto.ReleaseYear,
            Genre = dto.Genre,
            Rating = 0
        };

        Movies.Add(movie);

        var movieDto = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            Genre = movie.Genre,
            Rating = movie.Rating
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = movie.Id },
            movieDto);
    }
    
    [HttpPut("{id}")]
    public ActionResult<MovieDto> Update(int id, UpdateMovieDto dto)
    {
        var movie = Movies.FirstOrDefault(movie => movie.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.ReleaseYear = dto.ReleaseYear;
        movie.Genre = dto.Genre;

        var movieDto = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            Genre = movie.Genre,
            Rating = movie.Rating
        };

        return Ok(movieDto);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var movie = Movies.FirstOrDefault(movie => movie.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        Movies.Remove(movie);

        return NoContent();
    }
}