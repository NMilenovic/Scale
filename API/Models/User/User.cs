using Models.Album;
using Models.List;
using Models.Rating;
using MongoDB.Bson;

namespace Models.User
{
  public class User{
    public ObjectId Id {get;set;}
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole Role { get; set; }
    public List<FavoriteAlbum>? FavoriteAlbums {get;set;}
    public List<RecentRating>? RecentRatings {get;set;}
    public List<ListCard>? ListCards {get;set;}
  }
}