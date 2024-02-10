using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Models;
using Models.Album;
using MongoDB.Bson;
using Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AlbumController : ControllerBase
{
  private AlbumService albumService;

  public AlbumController()
  {
    albumService = new AlbumService();
  }

  [HttpPost("CreateAlbum")]
  public async Task<IActionResult> CreateAlbum ([FromBody]AlbumInfo albumInfo)
  {
    if(String.IsNullOrWhiteSpace(albumInfo.Title) || albumInfo.Title.Length > 200)
      return UnprocessableEntity("Title of album must be between 1 and 200 charachters");
    if(albumInfo.TrackList == null)
      return UnprocessableEntity("Album must contains at least one song.");
    if(albumInfo.GenresNames == null)
      return UnprocessableEntity("Album  must be at least in one genre");
    if(albumInfo.ArtistName == null)
      return UnprocessableEntity("Album must belong to artist");
    try
    {
      await albumService.CreateAlbum(albumInfo);
      return Ok("Album created");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpGet("GetAlbumById/{id}")]
  public IActionResult GetAlbumById(string id)
  {
    if(String.IsNullOrWhiteSpace(id))
      return BadRequest("ID of album is required");
    try
    {
      AlbumPage album = albumService.GetAlbum(id).Result;
      return Ok(album);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpGet("SearchAlbum/{patern}")]
  public async Task<IActionResult> SearchAlbum(string patern)
  {
    if(String.IsNullOrWhiteSpace(patern))
      return UnprocessableEntity("Pattern must exist");
    if(patern.Length <3)
      return UnprocessableEntity("Search querry must be 3 or more charachters");
    try
    {
      List<AlbumCard> searchResult = await albumService.SearchAlbum(patern);
      return Ok(searchResult);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpPost("AddFavoriteAlbum/{albumId}/{userId}")]
  public async Task<IActionResult> AddFavoriteAlbum(string albumId,string userId)
  {
    if(String.IsNullOrWhiteSpace(albumId))
      return UnprocessableEntity("AlbumID must exist");
    if(String.IsNullOrWhiteSpace(userId))
      return UnprocessableEntity("UserID must exist");
    try
    {
      await albumService.AddFavoriteAlbum(albumId,userId);
      return Ok("Favorite album added");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpDelete("RemoveFavoriteAlbum/{albumId}/{userId}")]
  public async Task<IActionResult> RemoveFavoriteAlbum(string albumId,string userId)
  {
    if(String.IsNullOrWhiteSpace(albumId))
      return UnprocessableEntity("AlbumID must exist");
    if(String.IsNullOrWhiteSpace(userId))
      return UnprocessableEntity("UserID must exist");
    try
    {
      await albumService.RemoveFavoriteAlbum(albumId,userId);
      return Ok("Favorite album removed");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpGet("IsFavorite/{albumId}/{userId}")]
  public async Task<IActionResult> IsFavorite(string albumId,string userId)
  {
    if(String.IsNullOrWhiteSpace(albumId))
      return UnprocessableEntity("AlbumID must exist");
    if(String.IsNullOrWhiteSpace(userId))
      return UnprocessableEntity("UserID must exist");
    try
    {
      var exist = await albumService.IsFavorite(albumId,userId);
      return Ok(exist);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpPost("SendMessage/{albumId}/{userId}/{message}")]
  public async Task<IActionResult> SendMessage(string albumId,string userId,string message)
  {
    if(String.IsNullOrWhiteSpace(albumId))
      return UnprocessableEntity("AlbumID must exist");
    if(String.IsNullOrWhiteSpace(userId))
      return UnprocessableEntity("UserID must exist");
    if(String.IsNullOrWhiteSpace(message))
      return UnprocessableEntity("Message must exist");
    if(message.Length > 1000)
      return UnprocessableEntity("Message must be between 1 and 1000 charachters");
    try
    {
      await albumService.SendMessage(albumId,userId,message);
      return Ok("Message sent");
    }
    catch(Exception e)
    {
       return BadRequest(e.Message);
    }
  }
}
