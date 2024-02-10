using Models.Album;
using Models.Rating;
using MongoDB.Driver;
using Services;

namespace BackgroundServices
{
  public class AvregeRatingBGService : IHostedService
  {
    private MongoService mongoService;

    public AvregeRatingBGService()
    {
      mongoService = new MongoService();
      
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      IMongoCollection<Album> albumCollection =  mongoService.database.GetCollection<Album>("Album");
      IMongoCollection<Rating> ratingCollection = mongoService.database.GetCollection<Rating>("Rating");
      while(!cancellationToken.IsCancellationRequested)
      {
        var albums = await albumCollection.Find(Builders<Album>.Filter.Empty).ToListAsync();
        foreach(var album in albums)
        {
          if(album.RatingCount != 0 || (album.RatingCount == 0 && album.AvregeRating != 0))
          {
         
            var ratings = await ratingCollection.Find(Builders<Rating>.Filter.Eq(r=>r.AlbumId,album.Id)).ToListAsync();
            double totalRating = 0;
            foreach(var rating in ratings)
            {
              totalRating+=rating.Grade;
            }
            double avrgRating = totalRating/album.RatingCount;

            if(album.RatingCount == 0)
              avrgRating = 0;
              
            var filter = Builders<Album>.Filter.Eq(r=>r.Id,album.Id);
            var update = Builders<Album>.Update.Set(r=>r.AvregeRating,avrgRating);
            albumCollection.UpdateOneAsync(filter,update);
          }
        }

      }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}