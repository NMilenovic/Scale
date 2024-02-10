using Microsoft.AspNetCore.Mvc;
using Models.Review;
using Services;

namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
  private ReviewService reviewService;

  public ReviewController()
  {
    reviewService = new ReviewService();
  }

  [HttpPost("CreateReview")]
  public async Task<IActionResult> CreateReview([FromBody]ReviewInfo reviewInfo)
  {
    if(String.IsNullOrWhiteSpace(reviewInfo.AlbumId))
      return UnprocessableEntity("AlbumID must exist");
    if(String.IsNullOrWhiteSpace(reviewInfo.UserId))
      return UnprocessableEntity("UserID must exist");
    if(String.IsNullOrWhiteSpace(reviewInfo.Content) || reviewInfo.Content.Length > 5000)
      return UnprocessableEntity("Review content must be between 1 and 5000 charachters");
    try
    {
      await  reviewService.CreateReview(reviewInfo);
      return Ok("Review created");
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message+" Stack trace: "+e.StackTrace);
    }
  }
  [HttpPost("AddFeatureReview/{reviewId}")]
  public async Task<IActionResult> AddFeatureReview(string reviewId)
  {
    if(String.IsNullOrWhiteSpace(reviewId))
      return UnprocessableEntity("ReviewID must exist.");
    try
    {
      await reviewService.AddFeatureReview(reviewId);
      return Ok("Featured review added");
    }
    catch(Exception e)
    {
       return BadRequest("Internal server error: "+e.Message+" Stack trace: "+e.StackTrace);
    }
  }
  [HttpGet("GetFeaturedReviews")]
  public async Task<IActionResult> GetFeaturedReviews()
  {
    try
    {
      List<FeaturedReviewCard> featuredReviews = await reviewService.GetFeaturedReviews();
      return Ok(featuredReviews);
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message+" Stack trace: "+e.StackTrace);
    }
  }
  [HttpGet("GetReview/{userId}/{albumId}")]
  public async Task<IActionResult> GetReview(string userId,string albumId)
  {
    try
    {
      ReviewCard review = await reviewService.GetReview(albumId,userId);
      return Ok(review);
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message+" Stack trace: "+e.StackTrace);
    }
  }
  [HttpDelete("DeleteReview/{reviewId}")]
  public async Task<IActionResult> DeleteReview(string reviewId)
  {
    if(String.IsNullOrWhiteSpace(reviewId))
      return UnprocessableEntity("ReviewID must exist");
    try
    {
      await reviewService.DeleteReview(reviewId);
      return Ok("Review deleted");
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message+" Stack trace: "+e.StackTrace);
   }
  }
 [HttpPut("UpdateReview/{reviewId}/{newContent}")]
 public async Task<IActionResult> UpdateReview(string reviewId,string newContent)
 {
   if(String.IsNullOrWhiteSpace(reviewId))
     return UnprocessableEntity("ReviewID must exist");
    if(String.IsNullOrWhiteSpace(newContent) || newContent.Length > 5000)
     return UnprocessableEntity("Content must be between 1 and 5000 charachters");
   try
   {
     Review reviews = await reviewService.UpdateReview(reviewId,newContent);
     return Ok(reviews);
   }
   catch(Exception e)
   {
     return BadRequest("Internal server error: "+e.Message+" Stack trace: "+e.StackTrace);
   }
 }
}