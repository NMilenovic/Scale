namespace Models.Artist
{
  public class ArtistInfo{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string>? GenresNames { get; set; }
    public string? PictureURL{ get; set; }
  }
}