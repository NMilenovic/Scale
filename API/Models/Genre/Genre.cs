using System.ComponentModel.DataAnnotations;
using Models.Album;
using MongoDB.Bson;

namespace Models.Genre
{
  public class Genre{
    public ObjectId Id { get; set; }
    public string? Name { get; set; }
    public string? ImageURL { get; set; }
    public string? Description { get; set; }
    public List<AlbumCard>? BestAlbums { get; set; }

  }
}