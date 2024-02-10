using Microsoft.Extensions.ObjectPool;
using Models;
using Models.Album;
using Models.Artist;
using Models.Discussion;
using Models.Genre;
using Models.Review;
using Models.User;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
  public class AlbumService
  {
    private MongoService mongoService;
    public AlbumService()
    {
      mongoService = new MongoService();
    }
    public async Task CreateAlbum(AlbumInfo albumInfo)
    {
      var exist = mongoService.database.ListCollectionNames().ToList().Contains("Album");
      if(!exist)
      {
        mongoService.database.CreateCollection("Album");
       
        var albumCollection = mongoService.database.GetCollection<Album>("Album");

        var DefinitionOfIndexKey = Builders<Album>.IndexKeys.Ascending(r=>r.Title);
        await albumCollection.Indexes.CreateOneAsync(new CreateIndexModel<Album>(DefinitionOfIndexKey));
      }

      
      var collection = mongoService.database.GetCollection<Album>("Album");
      var artistCollection = mongoService.database.GetCollection<Artist>("Artist");
      var genreCollection = mongoService.database.GetCollection<Genre>("Genre");

      var genresList = new List<GenreCard>();
      foreach(var genreName in albumInfo.GenresNames)
      {
        var filter = Builders<Genre>.Filter.Eq(r => r.Name,genreName);
        var genre = await genreCollection.Find(filter).ToListAsync();
        var first = genre.First();
        var card = new GenreCard{
          Name = first.Name,
          Id = first.Id.ToString()
        };
        genresList.Add(card);
      }
      var artistFilter = Builders<Artist>.Filter.Eq(r => r.Name,albumInfo.ArtistName);
      var artists = await artistCollection.Find(artistFilter).ToListAsync();
      var artist = artists.First();
      var artistCard = new ArtistCard{
        Name = artist.Name,
        Id = artist._id.ToString()
      };

      Album album = new Album{
        Title = albumInfo.Title,
        ImageURL = albumInfo.ImageURL,
        ReleaseDate = albumInfo.ReleseDate,
        TrackList = albumInfo.TrackList,
        Genres = genresList,
        Artist = artistCard,
        LatestReviews = new List<ReviewCard>(),
        Messages = new List<Models.Discussion.Message>()
      };
      collection.InsertOne(album);
      var albumId = album.Id;
      AlbumCard albumCard = new AlbumCard{
        AlbumId =  albumId.ToString(),
        Title = albumInfo.Title,
        ReleseDate = albumInfo.ReleseDate,
        ImageURL = albumInfo.ImageURL,
      };
      var update = Builders<Artist>.Update.AddToSet(a => a.Albums,albumCard);
      await artistCollection.UpdateOneAsync(artistFilter,update);
    }
    public async Task<AlbumPage> GetAlbum(string id)
    {
      Object oId = new ObjectId(id);
      var albumCollection = mongoService.database.GetCollection<Album>("Album");
      var filter = Builders<Album>.Filter.Eq(r=>r.Id,oId);
      var albums = await albumCollection.Find(filter).ToListAsync();
      if(albums.Count == 0)
        throw new Exception("Album dosent exist!");
      Album album = albums.First();
      List<AlbumPage> latestReviews = new List<AlbumPage>();

      AlbumPage page = new AlbumPage{
        AlbumId = album.Id.ToString(),
        Title = album.Title,
        ImageURL = album.ImageURL,
        ReleaseDate = album.ReleaseDate,
        RatingCount = album.RatingCount,
        AvregeRating = album.AvregeRating,
        TrackList = album.TrackList,
        Genres = album.Genres,
        Artist = album.Artist,
        LatestReviews = album.LatestReviews,
        Messages = album.Messages
      };
      return page;
    }

    public async Task<List<AlbumCard>> SearchAlbum(string patern)
    {
      IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
      var filter = Builders<Album>.Filter.Regex(r=>r.Title, new BsonRegularExpression(".*"+patern+".*", "i"));
      List<Album> albums = await albumCollection.Find(filter).ToListAsync();
      List<AlbumCard> searchResult = albums.Select(album =>{
        return new AlbumCard{
          AlbumId = album.Id.ToString(),
          Title = album.Title,
          ReleseDate = album.ReleaseDate,
          ImageURL = album.ImageURL
        };
      }).ToList();
    return searchResult;
    }

    public async Task AddFavoriteAlbum(string albumId,string userId)
    {
      var aId = new ObjectId(albumId);
      var uId = new ObjectId(userId);

      IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");

      var album = await albumCollection.Find(Builders<Album>.Filter.Eq(p=>p.Id,aId)).FirstOrDefaultAsync();


      var favoriteAlbum = new FavoriteAlbum{
        AlbumId = album.Id.ToString(),
        AlbumImageURL = album.ImageURL
      };

      var update = Builders<User>.Update.AddToSet(p=>p.FavoriteAlbums,favoriteAlbum);
      userCollection.UpdateOne(Builders<User>.Filter.Eq(p=>p.Id,uId),update);
    }
    public async Task RemoveFavoriteAlbum(string albumId,string userId)
    {
      var aId = new ObjectId(albumId);
      var uId = new ObjectId(userId);

      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");
      IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
      var album = await albumCollection.Find(Builders<Album>.Filter.Eq(p=>p.Id,aId)).FirstOrDefaultAsync();


      var favoriteAlbum = new FavoriteAlbum{
        AlbumId = album.Id.ToString(),
        AlbumImageURL = album.ImageURL
      };

      var update = Builders<User>.Update.Pull(p=>p.FavoriteAlbums,favoriteAlbum);
      userCollection.UpdateOne(Builders<User>.Filter.Eq(p=>p.Id,uId),update);
    }

    public async Task<bool> IsFavorite(string albumId,string userId)
    {
      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");
      var user = await userCollection.Find(Builders<User>.Filter.Eq(r=>r.Id,new ObjectId(userId))).FirstOrDefaultAsync();
      var isFavorite = user.FavoriteAlbums.Any(fav => fav.AlbumId == albumId);
      return isFavorite;
    }
    public async Task SendMessage(string albumId,string userId,string message)
    {
      IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");

      var albumFilter = Builders<Album>.Filter.Eq(r=>r.Id,new ObjectId(albumId));
      var user = await userCollection.Find(Builders<User>.Filter.Eq(r=>r.Id,new ObjectId(userId))).FirstOrDefaultAsync();
      var album = await albumCollection.Find(albumFilter).FirstOrDefaultAsync();

      var mess = new Message{
        Username = user.Username,
        Content = message
      };

      var pushUpdate = Builders<Album>.Update.AddToSet(r=>r.Messages,mess);
      if(album.Messages != null)
      {
        if(album.Messages.Count < 100)
        {
          await albumCollection.UpdateOneAsync(albumFilter,pushUpdate);
        }
        else
        {
          album.Messages.RemoveRange(0, 20);
          album.Messages.Add(mess);
          await albumCollection.ReplaceOneAsync(albumFilter, album);
        }
      }
      else
      {
        throw new Exception("Messages dosent exist");
      }
    }
  }
}