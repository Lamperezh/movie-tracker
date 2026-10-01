using Microsoft.AspNetCore.Mvc;
using movie_tracker.DTOs.Actor;
using movie_tracker.Models;

namespace movie_tracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private static readonly List<Actor> Actors =
    [
        new Actor
        {
            Id = 1,
            Name = "Matthew McConaughey",
            Biography = "American actor known for roles in drama and science fiction films.",
            BirthDate = new DateTime(1969, 11, 4)
        },
        new Actor
        {
            Id = 2,
            Name = "Christian Bale",
            Biography = "British actor known for roles in drama and action films.",
            BirthDate = new DateTime(1974, 1, 30)
        }
    ];
    
    [HttpGet]
    public ActionResult<IEnumerable<ActorDto>> GetAll()
    {
        var actors = Actors.Select(actor => new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            Biography = actor.Biography,
            BirthDate = actor.BirthDate
        });

        return Ok(actors);
    }
    
    [HttpGet("{id}")]
    public ActionResult<ActorDto> GetById(int id)
    {
        var actor = Actors.FirstOrDefault(actor => actor.Id == id);

        if (actor == null)
        {
            return NotFound();
        }

        var actorDto = new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            Biography = actor.Biography,
            BirthDate = actor.BirthDate
        };

        return Ok(actorDto);
    }
}