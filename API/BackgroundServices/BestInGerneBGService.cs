using Models.Album;
using Models.Genre;
using MongoDB.Driver;
using Services;

namespace BackgroundServices
{
  public class BestInGenreBGService : IHostedService
  {
    private MongoService mongoService;

    public BestInGenreBGService()
    {
      mongoService = new MongoService();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      while(!cancellationToken.IsCancellationRequested)
      {
        DateTime tomorrow = DateTime.Today.AddDays(1);
        TimeSpan timeUntilTomorrow = tomorrow - DateTime.Now;
        //await Task.Delay(timeUntilTomorrow, cancellationToken);

        //Napravi indeks za ovo
        IMongoCollection<Genre> genreCollection = mongoService.database.GetCollection<Genre>("Genre");
        IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
        var genres = await genreCollection.Find(Builders<Genre>.Filter.Empty).ToListAsync();
        foreach(var genre in genres)
        {
          var filter = Builders<Album>.Filter.Where(album => album.Genres.Any(g => g.Name == genre.Name));

          var sort= Builders<Album>.Sort.Descending(album => album.AvregeRating);
          var topAlbums = await albumCollection.Find(filter).Sort(sort).Limit(10).ToListAsync();
          
          var cards = topAlbums.Select(album =>{
            return new AlbumCard{
              AlbumId = album.Id.ToString(),
              Title = album.Title,
              ReleseDate = album.ReleaseDate,
              ImageURL = album.ImageURL
            };
          }).ToList();
          genre.BestAlbums = cards;
          var genreFilter = Builders<Genre>.Filter.Eq(g => g.Id, genre.Id);
          await genreCollection.ReplaceOneAsync(genreFilter, genre);
        }
      }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}