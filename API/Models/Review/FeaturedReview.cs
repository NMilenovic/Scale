using MongoDB.Bson;

namespace Models.Review
{
  public class FeaturedReview
  {
    public ObjectId Id { get; set; }
    public ReviewCard? fReview {get;set;}
    public ObjectId ArtistId {get;set;}
    public string? ArtistName {get;set;}
    public string? AlbumTitle { get; set; }
    public string? AlbumImageURL { get; set; }
  }
}