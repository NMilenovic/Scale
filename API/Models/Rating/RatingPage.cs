namespace Models.Rating
{
  public class RatingPage
  {
    public string? Id { get; set; }
    public string? AlbumId { get; set; }
    public string? UserId { get; set; }
    public uint Grade { get; set; }
    public DateTime DateOfRating { get; set; }
    public string? AlbumName { get; set; }
    public string? ArtistName { get; set; }
    public string? AlbumImageURL { get; set; }

  }
}