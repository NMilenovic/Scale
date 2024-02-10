namespace Services{
  using MongoDB.Driver;
  public class MongoService{
    private MongoClient client;
    public IMongoDatabase database;

    public MongoService()
    {
     var settings = new MongoClientSettings()
      {
        Scheme = MongoDB.Driver.Core.Configuration.ConnectionStringScheme.MongoDB,
        Server = new MongoServerAddress("localhost", 27017)
      };

     client = new MongoClient(settings);
     database = client.GetDatabase("Scale");
    }
  }
}