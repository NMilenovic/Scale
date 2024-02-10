using API.Controllers;
using Microsoft.Extensions.ObjectPool;
using Models.Album;
using Models.List;
using Models.User;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
  public class ListService
  {
    MongoService mongoService;

    public ListService()
    {
      mongoService = new MongoService();
    }
    public async Task CreateList(ListInfo listInfo)
    {
      IMongoCollection<Models.List.List> listCollection;
      IMongoCollection<User> userCollection;
      var exist = mongoService.database.ListCollectionNames().ToList().Contains("List");
      if(!exist)
      {
        mongoService.database.CreateCollection("List");
        listCollection = mongoService.database.GetCollection<List>("List");
        var DefinitionOfIndexKey = Builders<List>.IndexKeys.Ascending(r=>r.Title);
        await listCollection.Indexes.CreateOneAsync(new CreateIndexModel<List>(DefinitionOfIndexKey));
      }
      listCollection = mongoService.database.GetCollection<Models.List.List>("List");
      userCollection = mongoService.database.GetCollection<User>("User");

      var uId = new ObjectId(listInfo.UserId);
      var  user = await userCollection.Find(Builders<User>.Filter.Eq(r=>r.Id,uId)).FirstOrDefaultAsync();

      Models.List.List listObject = new Models.List.List {
        UserId = uId,
        Username = user.Username,
        Title = listInfo.Title,
        Description = listInfo.Description,
        ListImageURL = listInfo.ListImageURL,
        ListItems = new List<ListItem>()
      };
      

      await listCollection.InsertOneAsync(listObject);

      ListCard listCard = new ListCard{
        ListId = listObject.Id.ToString(),
        Title = listInfo.Title,
        ListImageURL = listInfo.ListImageURL
      };

     if(user.ListCards != null)
     {
      user.ListCards.Add(listCard);
      await userCollection.ReplaceOneAsync(Builders<User>.Filter.Eq(r=>r.Id,uId),user);
     }
    }
  public async Task RemoveList(string listId,string UserId)
  {
    var lId = new ObjectId(listId);
    IMongoCollection<Models.List.List> listCollection = mongoService.database.GetCollection<Models.List.List>("List");
    var filter = Builders<Models.List.List>.Filter.Eq(r=>r.Id,lId);
   
    IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");
    var userFilter = Builders<User>.Filter.Eq(r=>r.Id,new ObjectId(UserId));
    var userUpdate = Builders<User>.Update.PullFilter(u => u.ListCards, lc => lc.ListId == listId);

    var t1 =  listCollection.DeleteOneAsync(filter);
    var t2 = userCollection.UpdateOneAsync(userFilter, userUpdate);

    await Task.WhenAll(t1,t2);

  }
  public async Task AddAlbumToList(string listId,string albumId,string description)
  {
    var lId = new ObjectId(listId);
    var aId = new ObjectId(albumId);
    IMongoCollection<Models.List.List> listCollection = mongoService.database.GetCollection<Models.List.List>("List");
    IMongoCollection<Album> albumCollectin = mongoService.database.GetCollection<Album>("Album");

    var filter = Builders<Models.List.List>.Filter.Eq(r=>r.Id,lId);
    var albumFilter = Builders<Album>.Filter.Eq(r=>r.Id,aId);
   
    var album = albumCollectin.Find(albumFilter).FirstOrDefault();
    var list = listCollection.Find(filter).FirstOrDefault();

    ListItem listItem = new ListItem{
      Place = list.ListItems.Count+1,
      ArtistId = album.Artist.Id.ToString(),
      ArtistName = album.Artist.Name,
      AlbumId = album.Id.ToString(),
      AlbumName = album.Title,
      Description = description
    };

    list.ListItems.Add(listItem);

    await listCollection.ReplaceOneAsync(filter,list);
  }
  public async Task<List<ListCard>> SearchList(string patern)
  {
    IMongoCollection<Models.List.List> listCollection = mongoService.database.GetCollection<Models.List.List>("List");
    var filter = Builders<Models.List.List>.Filter.Regex(r=>r.Title, new BsonRegularExpression(".*"+patern+".*", "i"));
    List<Models.List.List> lists = await listCollection.Find(filter).ToListAsync();
    List<ListCard> searchResult = lists.Select(list =>{
      return new ListCard{
        ListId = list.Id.ToString(),
        Title = list.Title,
        ListImageURL = list.ListImageURL
      };
    }).ToList();
    return searchResult;
  }
  public async Task DeleteAlbumFromList(string listId,int place)
  {
    var lId = new ObjectId(listId);
    IMongoCollection<Models.List.List> listCollection = mongoService.database.GetCollection<Models.List.List>("List");

    var filter = Builders<Models.List.List>.Filter.Eq(r => r.Id, lId);
    var updateDefinition = Builders<Models.List.List>.Update.PullFilter("ListItems", Builders<ListItem>.Filter.Eq("Place", place));

    await listCollection.UpdateOneAsync(filter, updateDefinition);
  }

  public  ListPage GetListById(string listId)
  {
    IMongoCollection<List> listCollection = mongoService.database.GetCollection<List>("List");
    var list = listCollection.Find(Builders<List>.Filter.Eq(r=>r.Id,new ObjectId(listId))).FirstOrDefault();
    ListPage page = new ListPage{
      Id = list.Id.ToString(),
      OwnerId = list.UserId.ToString(),
      OwnerUsername = list.Username,
      Title = list.Title,
      Description = list.Description,
      ListImageURL = list.ListImageURL,
      ListItems = list.ListItems
    };
    return page;

  }
}
}