using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Models.Rating
{
  public class Rating
  {
    public ObjectId Id { get; set; }
    public ObjectId AlbumId { get; set; }
    public ObjectId UserId { get; set; }
  
    [Range(1,5)]
    public uint Grade { get; set; }
    public DateTime DateOfRating { get; set; }
    public string? AlbumName { get; set; }
    public string? ArtistName { get; set; }
    public string? AlbumImageURL { get; set; }

  }
}