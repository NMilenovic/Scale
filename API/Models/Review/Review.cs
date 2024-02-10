using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Models.Review
{
  public class Review
  {
    public ObjectId Id { get; set; }
    public ObjectId AlbumId { get; set; }
    public ObjectId UserId { get; set; }
    public string? Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModification { get; set; }
    [MaxLength(5000)]
    public string? Content { get; set; }
  }
}