using Microsoft.AspNetCore.Mvc;
using Models.Rating;
using Services;

namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class RatingController : ControllerBase
{
  private RatingService ratingService;

  public RatingController()
  {
    ratingService = new RatingService();
  }

  [HttpPost("RateAlbum/{UserID}/{AlbumID}")]
  public async Task<IActionResult> RateAlbum([FromBody]RatingInfo ratingInfo,string UserID,string AlbumID)
  {
    if(String.IsNullOrWhiteSpace(UserID))
      return UnprocessableEntity("UserID must exist.");
    if(String.IsNullOrWhiteSpace(AlbumID))
      return UnprocessableEntity("AlbumID must exist.");
    if(ratingInfo.Grade < 1 || ratingInfo.Grade > 5)
      return UnprocessableEntity("Grade of album must be between 1 and 5");
    if(String.IsNullOrWhiteSpace(ratingInfo.AlbumName))
      return UnprocessableEntity("Album name must exist.");
    if(String.IsNullOrWhiteSpace(ratingInfo.ArtistName))
      return UnprocessableEntity("Artist name must exist.");
    try
      {
        await ratingService.RateAlbum(UserID,AlbumID,ratingInfo);
        return Ok($"{ratingInfo.AlbumName} rated with {ratingInfo.Grade}");
      }
      catch(Exception e)
      {
        return BadRequest("Internal server error: "+e.Message);   
      }
  }
  [HttpGet("GetAllRatings/{userId}")]
  public async Task<IActionResult> GetAllRatings(string userId)
  {
    if(String.IsNullOrWhiteSpace(userId))
      return UnprocessableEntity("UserID must exist!");
    try
    {
      var allRatings =await ratingService.GetAllRatings(userId);
      return Ok(allRatings); 
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message);
    }
  }
  [HttpGet("GetRating/{albumId}/{userId}")]
  public async Task<IActionResult> GetRating(string albumId,string userId)
  {
    if(String.IsNullOrWhiteSpace(albumId))
      return UnprocessableEntity("AlbumID must exist!");
    if(String.IsNullOrWhiteSpace(userId))
      return UnprocessableEntity("UserID must exist!");
    try
    {
      RatingPage rating =await ratingService.GetRating(userId,albumId);
      return Ok(rating);
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message);
    }
  }
  [HttpDelete("DeleteRating/{ratingId}")]
  public async Task<IActionResult> DeleteRating(string ratingId)
  {
    if(String.IsNullOrWhiteSpace(ratingId))
      return UnprocessableEntity("RatingID must exist!");
    try
    {
      await ratingService.DeleteRating(ratingId);
      return Ok("Rating deleted");
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message);
    }
  }
}