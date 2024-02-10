using Models.Album;
using Models.Genre;

namespace Models.Artist
{
 public class ArtistPage
 {
  public string? ArtistId { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public List<GenreCard>? Genres { get; set; }
  public string? PictureURL{ get; set; }
  public List<AlbumCard>? Albums { get; set; }
 } 
}