using Models.Album;
using Models.Genre;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services{
  public class GenreService
  {
    public MongoService mongoService;


    public GenreService()
    {
      mongoService = new MongoService();
    }
    public async Task AddGenre(GenreInfo genreInfo)
    {
        var exist = mongoService.database.ListCollectionNames().ToList().Contains("Genre");
        if(!exist)
        {
          mongoService.database.CreateCollection("Genre");
        }
        var genresCollection = mongoService.database.GetCollection<Genre>("Genre");
        var newGenre = new Genre{
          Name = genreInfo.Name,
          ImageURL = genreInfo.imageURL,
          Description = genreInfo.Description,
          BestAlbums = new List<AlbumCard>()
        };
        await genresCollection.InsertOneAsync(newGenre);
    }
    public async Task<GenrePage> GetGenreById(string id)
    {
      var oId = new ObjectId(id);
      var genreCollection =  mongoService.database.GetCollection<Genre>("Genre");
      var filter = Builders<Genre>.Filter.Eq(r=>r.Id,oId);
      var exist = await genreCollection.Find(filter).ToListAsync();
      if(exist.Count == 0) 
        throw new Exception($"Genre with ID {id} dosent exist");
      var genres = genreCollection.Find(filter).ToList();
      var genre = genres.First();
      var genrePage = new GenrePage{
        GenreId = genre.Id.ToString(),
        Name = genre.Name,
        ImageURL = genre.ImageURL,
        Description = genre.Description,
        BestAlbums = genre.BestAlbums
      };
      return genrePage;
    }
    public async Task DeleteGenre(string id)
    {
      var oId = new ObjectId(id);
      var genreCollection =  mongoService.database.GetCollection<Genre>("Genre");
      var filter = Builders<Genre>.Filter.Eq(r => r.Id,oId);
      var exist = await genreCollection.Find(filter).ToListAsync();
      if(exist.Count == 0) 
        throw new Exception($"Genre with ID {id} dosent exist");
      
      genreCollection.DeleteOne(filter);
    }
    public async Task<List<GenreTab>> GetGenresTabs()
    {
      var genres = mongoService.database.GetCollection<Genre>("Genre");
      var genress = await genres.Find(_ => true).ToListAsync();
      List<GenreTab> tabs = new List<GenreTab>();
      foreach (var genre in genress)
      {
        tabs.Add(new GenreTab{
          Id = genre.Id.ToString(),
          Name = genre.Name,
          ImageURL = genre.ImageURL,
          Description = genre.Description
        });
      }
      return tabs;
    }
  }
    
  

}