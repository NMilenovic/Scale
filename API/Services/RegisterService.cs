using Models.Album;
using Models.List;
using Models.Rating;
using Models.User;
using MongoDB.Driver;

namespace Services{
  public class RegisterService
  {
    MongoService mongoService;

    public RegisterService()
    {
      mongoService = new MongoService();
    }

    public async Task<bool>Register(UserRegisterDTO urDTO)
    {
      var usersCollection = mongoService.database.GetCollection<User>("User");
      var filter =  Builders<User>.Filter.Eq(x => x.Email,urDTO.Email);
      var user = await usersCollection.Find(filter).ToListAsync();
      if(user.Count != 0)
        return false;
      var newUser = new User{Username = urDTO.Username,
                             Email = urDTO.Email,
                             Password = urDTO.Password1,
                             Role = urDTO.Role,
                             FavoriteAlbums = new List<FavoriteAlbum>(),
                             RecentRatings = new List<RecentRating>(),
                             ListCards = new List<ListCard>()
                             };//sifra u productionu je plaintext
      await usersCollection.InsertOneAsync(newUser);
      return true;
    }

  }
}