using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Models.Review
{
  public class ReviewInfo
  {
    public string? AlbumId { get; set; }
    public string? UserId { get; set; }
    [MaxLength(5000)]
    public string? Content { get; set; }
  }
}