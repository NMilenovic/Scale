using Models.Album;

namespace Models.Genre
{
  public class GenrePage
  {
    public string? GenreId { get; set; }
    public string? Name { get; set; }
    public string? ImageURL { get; set; }
    public string? Description { get; set; }
    public List<AlbumCard>? BestAlbums { get; set; }
  }
}