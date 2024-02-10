using System.ComponentModel.DataAnnotations;
using Models.Genre;
using Models.Review;
using MongoDB.Bson;

namespace Models.Album
{
  public class Album
  {
    public ObjectId Id { get; set; } 

    [MaxLength(200)]
    public string? Title { get; set; }
    public string? ImageURL { get; set; }
    public DateTime ReleaseDate { get; set; }
    [Required]
    public long RatingCount { get; set; } = 0;
    [Range(1.0,5.0)]
    public double AvregeRating { get; set; } = 1.0;
    [Required]
    public List<Track>? TrackList { get; set; }
    public List<GenreCard>? Genres { get; set; }
    public ArtistCard? Artist { get; set; }
    public List<ReviewCard>? LatestReviews { get; set; }
    public List<Models.Discussion.Message>? Messages {get;set;}
    
    
  }
}