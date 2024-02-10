using Microsoft.AspNetCore.Mvc;
using Services;
using Models.Genre;
namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GenreController : ControllerBase
{
    GenreService  genreService;
    public GenreController()
    {
        genreService = new GenreService();
    }

    [HttpPost("AddGenre")]
    public async Task<IActionResult> AddGenre([FromBody]GenreInfo genreInfo)
    {
        if(String.IsNullOrWhiteSpace(genreInfo.Name))
            return UnprocessableEntity("Genre must have name!");
        if(String.IsNullOrWhiteSpace(genreInfo.Description))
            return UnprocessableEntity("Genre must have name!");
        if(String.IsNullOrWhiteSpace(genreInfo.imageURL))
            return UnprocessableEntity("Genre must have image!");
        try
        {
            await genreService.AddGenre(genreInfo);
            return Ok("Genre added");
        }
        catch(Exception e)
        {
           return BadRequest(e.Message);
        }
    }
    [HttpGet("GetGenreById/{id}")]
    public async Task<IActionResult> GetGenreById(string id)
    {
      try
      {
        var genre = await genreService.GetGenreById(id);
        return Ok(genre);
      }
      catch(Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpDelete("DeleteGenre/{id}")]
    public async Task<IActionResult>DeleteGenre(string id)
    {
      try
      {
        await genreService.DeleteGenre(id);
        return Ok($"Genre with ID {id} is deleted");
      }
      catch(Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("GetGenresTabs")]
    public async Task<IActionResult> GetGenresTabs()
    {
        var g = await genreService.GetGenresTabs();
        return Ok(g);
    }

}
