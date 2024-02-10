using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Models.Review
{
  public class ReviewCard
  {
    public string? Id { get; set; }
    public string? AlbumId { get; set; }
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModification { get; set; }
    [MaxLength(5000)]
    public string? Content { get; set; }
  }
}