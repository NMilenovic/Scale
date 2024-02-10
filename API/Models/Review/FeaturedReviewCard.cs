namespace Models.Review
{
  public class FeaturedReviewCard
  {
    public string? Id { get; set; }
    public ReviewCard? fReview { get; set; }
    public string? ArtistId { get; set; }
    public string? ArtistName {get;set;}
    public string? AlbumTitle { get; set; }
    public string? AlbumImageURL { get; set; }
  }
}