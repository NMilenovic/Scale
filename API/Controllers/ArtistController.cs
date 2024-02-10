using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Artist;
using MongoDB.Bson;
using Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtistController : ControllerBase
{

  ArtistService artistService;

  public ArtistController()
  {
    artistService = new ArtistService();
  }


  [HttpPost("CreateArtist")]
  public async Task<IActionResult> CreateArtist([FromBody]ArtistInfo artistInfo)
  {
    if(artistInfo.Name == null || artistInfo.Name.Length > 100 || String.IsNullOrWhiteSpace(artistInfo.Name))
      return UnprocessableEntity("Artist name must be between 0 and 100 charachters.");
    if(artistInfo.GenresNames == null)
      return UnprocessableEntity("Artist must have at least 1 genre");
    try
    {
      await artistService.AddArtist(artistInfo);
      return Ok($"Artist {artistInfo.Name} is added in database");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpGet("GetArtist/{id}")]
  public async  Task<IActionResult> GetArtist(string id)
  {
    try
    {
      ArtistPage artist = await artistService.GetArtist(id);
      return Ok(artist);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpGet("SearchArtist/{patern}")]
  public async Task<IActionResult> SearchArtist(string patern)
  {
    if(String.IsNullOrWhiteSpace(patern))
      return UnprocessableEntity("Pattern must exist");
    if(patern.Length <3)
      return UnprocessableEntity("Search querry must be 3 or more characters");
    try
    {
      List<ArtistCard> searchResult = await artistService.SearchArtist(patern);
      return Ok(searchResult);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}