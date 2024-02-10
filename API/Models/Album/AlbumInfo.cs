using System.Runtime.CompilerServices;
using Models.Genre;
using MongoDB.Bson;

namespace Models.Album
{
  public class AlbumInfo
  {
    public string? Title { get; set; }
    public DateTime ReleseDate { get; set; }
    public string?  ImageURL {get;set;}
    public List<Track>? TrackList { get; set; }
    public List<string>? GenresNames { get; set; }
    public string? ArtistName { get; set; }
  }
}