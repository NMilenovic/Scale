using Models;
using Models.Album;
using Models.Rating;
using Models.Review;
using Models.User;
using MongoDB.Bson;
using MongoDB.Driver;
namespace Services
{
  public class RatingService
  {
    private MongoService mongoService;

    public RatingService()
    {
      mongoService = new MongoService();
    }

    public async Task RateAlbum(string UserID,string AlbumID,RatingInfo ratingInfo)
    {
      ObjectId userId = new ObjectId(UserID);
      ObjectId albumId = new ObjectId(AlbumID);
      IMongoCollection<Rating> ratingCollection;
      IMongoCollection<Album> albumCollection;
      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");

      var exist = mongoService.database.ListCollectionNames().ToList().Contains("Review");
      if(!exist)
      {
        await mongoService.database.CreateCollectionAsync("Rating");
        ratingCollection = mongoService.database.GetCollection<Rating>("Rating");

        var albumUserIndex = new CreateIndexModel<Rating>(
          Builders<Rating>.IndexKeys.Ascending(r => r.AlbumId).Ascending(r => r.UserId),
          new CreateIndexOptions {Name = "album_user_rating_index"}
        );
        var userIndex = new CreateIndexModel<Rating>(
          Builders<Rating>.IndexKeys.Ascending(r=>r.UserId),
          new CreateIndexOptions {Name = "user_rating_index"}
        );
        var task1 =  ratingCollection.Indexes.CreateOneAsync(albumUserIndex);
        var task2 = ratingCollection.Indexes.CreateOneAsync(userIndex);
        await Task.WhenAll(task1,task2);
      }
      ratingCollection = mongoService.database.GetCollection<Rating>("Rating");
      albumCollection = mongoService.database.GetCollection<Album>("Album");

      Rating rating = new Rating{
        UserId = userId,
        AlbumId = albumId,
        Grade = ratingInfo.Grade,
        DateOfRating = DateTime.Now,
        AlbumName = ratingInfo.AlbumName,
        ArtistName = ratingInfo.ArtistName,
        AlbumImageURL = ratingInfo.AlbumImageURL
      };
      
      Task t1 = ratingCollection.InsertOneAsync(rating);

      var filter = Builders<Album>.Filter.Eq(r=>r.Id,albumId);
      var update = Builders<Album>.Update.Inc("RatingCount",1);

      Task t2 = albumCollection.UpdateOneAsync(filter,update);

      await Task.WhenAll(t1,t2);
      //Recent ratings
      var userFilter = Builders<User>.Filter.Eq(r=>r.Id,userId);
      var user = userCollection.Find(userFilter).FirstOrDefault();

      var album = albumCollection.Find(filter).FirstOrDefault();
      RecentRating recentRating = new RecentRating{
        AlbumTitle = ratingInfo.AlbumName,
        AlbumImageURL = album.ImageURL,
        AlbumId = albumId.ToString(),
        ArtistName = ratingInfo.ArtistName,
        ArtistId = album.Artist.Id.ToString(),
        DateOfRating = DateTime.Now,
        Grade = ratingInfo.Grade
      };
      var recentRatingInsert = Builders<User>.Update.AddToSet(r=>r.RecentRatings,recentRating);
      if(user.RecentRatings.Count <= 9 )
      {
        userCollection.UpdateOne(userFilter,recentRatingInsert);
      }
      else
      {
        
      }
    }

    public async Task<RatingPage> GetRating(string userId,string albumId)
    {

      IMongoCollection<Rating> ratingCollection;
      ObjectId aID = new ObjectId(albumId);
      ObjectId uID = new ObjectId(userId);
      ratingCollection = mongoService.database.GetCollection<Rating>("Rating");

      FilterDefinition<Rating> filter = new BsonDocument
      {
        { "AlbumId", aID }, 
        { "UserId", uID }
      };
      var ratings = await ratingCollection.Find(filter).ToListAsync();
      if(ratings.Count == 0)
        throw new Exception("Rating dosent exist!");
      Rating rating =  ratings.First();
      RatingPage page = new RatingPage{
        Id = rating.Id.ToString(),
        AlbumId = rating.AlbumId.ToString(),
        UserId = rating.UserId.ToString(),
        Grade = rating.Grade,
        DateOfRating = rating.DateOfRating,
        AlbumName = rating.AlbumName,
        ArtistName = rating.ArtistName,
        AlbumImageURL = rating.AlbumImageURL,
      };
      return page;

    }
    public async Task DeleteRating(string ratingId)
    {
      IMongoCollection<Rating> ratingCollecton = mongoService.database.GetCollection<Rating>("Rating");
      IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");

      ObjectId rId = new ObjectId(ratingId);
      
      var filter = Builders<Rating>.Filter.Eq(r=>r.Id,rId);
      var rating = await ratingCollecton.Find(filter).FirstOrDefaultAsync();


      var update = Builders<Album>.Update.Inc(r => r.RatingCount,-1);
      await albumCollection.UpdateOneAsync(Builders<Album>.Filter.Eq(r=>r.Id,rating.AlbumId),update);
      await ratingCollecton.DeleteOneAsync(filter);

      var userFilter = Builders<User>.Filter.Eq(u => u.Id, rating.UserId);
      var userUpdate = Builders<User>.Update.PullFilter(u => u.RecentRatings, r => r.AlbumId == rating.AlbumId.ToString());
      await userCollection.UpdateOneAsync(userFilter, userUpdate);
    }
    public async Task<List<RecentRating>> GetAllRatings(string userId)
    {
      IMongoCollection<Rating> ratingCollection = mongoService.database.GetCollection<Rating>("Rating");
      var ratings = await ratingCollection.Find(Builders<Rating>.Filter.Eq(r=>r.UserId,new ObjectId(userId))).ToListAsync();
      List<RecentRating> pages = ratings.Select(rating =>{
        return new RecentRating{
          AlbumTitle = rating.AlbumName,
          AlbumImageURL = rating.AlbumImageURL,
          AlbumId = rating.AlbumId.ToString(),
          ArtistName = rating.ArtistName,
          DateOfRating = rating.DateOfRating,
          Grade = rating.Grade
        };
      }).ToList();
      return pages;
    }
    
  }
}