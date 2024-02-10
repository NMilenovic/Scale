using Models;
using Models.Album;
using Models.Artist;
using Models.Genre;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services{
  public class ArtistService{
    MongoService mongoService;

    public ArtistService()
    {
      mongoService = new MongoService();
    }
    public async Task AddArtist(ArtistInfo artistInfo)
    {
      var exist = mongoService.database.ListCollectionNames().ToList().Contains("Artist");
      if(!exist)
      {
        mongoService.database.CreateCollection("Artist");
        var artistCollection = mongoService.database.GetCollection<Artist>("Artist");

        var indexKeyDef = Builders<Artist>.IndexKeys.Ascending(r=>r.Name);
        await artistCollection.Indexes.CreateOneAsync(new CreateIndexModel<Artist>(indexKeyDef));
      }

       var collection = mongoService.database.GetCollection<Artist>("Artist");
       var genreCollection = mongoService.database.GetCollection<Genre>("Genre");
       
       var GeneresL = new List<GenreCard>();
       foreach(var gn in artistInfo.GenresNames)
       {
          var filter = Builders<Genre>.Filter.Eq(r => r.Name,gn);
          var genre = await genreCollection.Find(filter).ToListAsync();
          var first = genre.First();
          var card = new GenreCard{
            Name = first.Name,
            Id = first.Id.ToString()
          };
          GeneresL.Add(card);
       };

       var artist = new Artist{
        Name = artistInfo.Name,
        Description = artistInfo.Description,
        Genres = GeneresL,
        PictureURL = artistInfo.PictureURL,
        Albums = new List<AlbumCard>()
       };
       collection.InsertOne(artist);
    }
  public async Task<ArtistPage> GetArtist(string id)
  {
    ObjectId oId = new ObjectId(id);
    var artistCollection = mongoService.database.GetCollection<Artist>("Artist");
    var filter = Builders<Artist>.Filter.Eq(r=>r._id,oId);
    var artists = await artistCollection.Find(filter).ToListAsync();
    if(artists.Count == 0)
      throw new Exception("Artist dosent exist!");
    Artist artist = artists.First();
    ArtistPage page = new ArtistPage{
      ArtistId = artist._id.ToString(),
      Name = artist.Name,
      Description = artist.Description,
      Genres = artist.Genres,
      PictureURL = artist.PictureURL,
      Albums = artist.Albums
    };
    return page;
  }  
  public async Task<List<ArtistCard>> SearchArtist(string patern)
  {
    IMongoCollection<Artist> artistCollection = mongoService.database.GetCollection<Artist>("Artist");
    var filter = Builders<Artist>.Filter.Regex(r=>r.Name, new BsonRegularExpression(".*"+patern+".*", "i"));
    List<Artist> artists = await artistCollection.Find(filter).ToListAsync();
    List<ArtistCard> searchResult = artists.Select(artist =>{
      return new ArtistCard{
        Id = artist._id.ToString(),
        Name = artist.Name,
        ImageURL = artist.PictureURL
      };
    }).ToList();
    return searchResult;
  }

  }
}
