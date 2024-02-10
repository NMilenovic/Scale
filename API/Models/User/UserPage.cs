using Models.Album;
using Models.List;
using Models.Rating;

namespace Models.User
{
  public class UserPage
  {
    public string? Id { get; set; }
    public string? Username { get; set; }
    public List<FavoriteAlbum>? FavoriteAlbums {get;set;}
    public List<RecentRating>? RecentRatings {get;set;}
    public List<ListCard> ListCards { get; set; }
    

    
  }
}