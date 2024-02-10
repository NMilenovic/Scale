using Models.Album;
using Models.Genre;
using MongoDB.Bson;

namespace Models.Artist
{
 public class Artist
 {
  public ObjectId _id { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public List<GenreCard>? Genres { get; set; }
  public string? PictureURL{ get; set; }
  public List<AlbumCard>? Albums { get; set; }
 } 
}