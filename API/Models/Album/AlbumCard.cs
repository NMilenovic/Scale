using MongoDB.Bson;

namespace Models.Album
{
  public class AlbumCard
  {
    public string? AlbumId { get; set; }
    public string? Title { get; set; }
    public DateTime ReleseDate { get; set; }
    public string? ImageURL { get; set; }
  }
}