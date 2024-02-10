using System.Diagnostics.CodeAnalysis;
using Models.User;
using MongoDB.Bson;
using MongoDB.Driver;
using Services;

namespace Service
{
  public class UserService
  {
    private MongoService mongoService;

    public UserService()
    {
      mongoService = new MongoService();
    }

    public async Task<UserPage> GetUser(string id)
    {
      var oId = new ObjectId(id);
      var userCollection = mongoService.database.GetCollection<User>("User");
     
      var user = await userCollection.Find(Builders<User>.Filter.Eq(r=>r.Id,oId)).FirstOrDefaultAsync();

      UserPage page = new UserPage
      {
        Id = user.Id.ToString(),
        Username = user.Username,
        FavoriteAlbums = user.FavoriteAlbums,
        RecentRatings = user.RecentRatings,
        ListCards = user.ListCards
      };

      return page;
    }

    public async Task<List<UserCard>> SearchUser(string patern)
    {
      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");
      var filter = Builders<User>.Filter.Regex(r=>r.Username, new BsonRegularExpression(".*"+patern+".*", "i"));
      List<User> users = await userCollection.Find(filter).ToListAsync();
      List<UserCard> searchResult = users.Select(user =>{
        return new UserCard{
          userId = user.Id.ToString(),
          Username = user.Username
        };
      }).ToList();
      return searchResult;
    }
  }
}