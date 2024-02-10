namespace Models.Rating
{
  public class RecentRating
  {
    public string? AlbumTitle { get; set; }
    public string? AlbumImageURL { get; set; }
    public string? AlbumId { get; set; }
    public string? ArtistName { get; set; }
    public string? ArtistId { get; set; }
    public DateTime DateOfRating { get; set; }
    public uint Grade { get; set; }
  }
}