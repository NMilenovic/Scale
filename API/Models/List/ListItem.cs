using MongoDB.Bson;

namespace Models.List
{
  public class ListItem
  {
    public int Place { get; set; }
    public string? ArtistId { get; set; }
    public string? ArtistName { get; set; }
    public string? AlbumId { get; set; }
    public string? AlbumName { get; set; }
    public string? Description { get; set; }
  }
}