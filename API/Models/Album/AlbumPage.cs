using Models.Genre;
using Models.Review;

namespace Models.Album
{
  public class AlbumPage
  {
    public string? AlbumId { get; set; }
    public string? Title { get; set; }
    public string? ImageURL { get; set; }
    public DateTime ReleaseDate { get; set; }
    public long RatingCount { get; set; } 
    public double AvregeRating { get; set; }
    public List<Track>? TrackList { get; set; }
    public List<GenreCard>? Genres { get; set; }
    public ArtistCard? Artist { get; set; }
    public List<ReviewCard>? LatestReviews {get;set;}
    public List<Models.Discussion.Message>? Messages { get; set; }
  }
}