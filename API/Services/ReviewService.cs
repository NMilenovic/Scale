using Amazon.Runtime.Internal.Util;
using Models.Album;
using Models.Review;
using Models.User;
using MongoDB.Bson;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace Services
{
  public class ReviewService
  {
    private MongoService mongoService;

    public ReviewService()
    {
      mongoService = new MongoService();
    }

    public async Task CreateReview(ReviewInfo reviewInfo)
    {
      IMongoCollection<Review> reviewCollection;
      IMongoCollection<User> userCollection = mongoService.database.GetCollection<User>("User");
      var user = userCollection.Find(Builders<User>.Filter.Eq(r =>r.Id,new ObjectId(reviewInfo.UserId))).FirstOrDefault();

      var exist = mongoService.database.ListCollectionNames().ToList().Contains("Review");
      if(!exist)
      {
        await mongoService.database.CreateCollectionAsync("Review");
        reviewCollection = mongoService.database.GetCollection<Review>("Review");

        var albumUserIndex = new CreateIndexModel<Review>(
          Builders<Review>.IndexKeys.Ascending(r=>r.AlbumId).Ascending(r=>r.UserId),
          new CreateIndexOptions {Name = "album_user_review_index"}
        );
        await reviewCollection.Indexes.CreateOneAsync(albumUserIndex);
      }
      reviewCollection = mongoService.database.GetCollection<Review>("Review");
      Review review = new Review{
        AlbumId = new ObjectId(reviewInfo.AlbumId),
        UserId = new ObjectId(reviewInfo.UserId),
        Username = user.Username,
        CreatedAt = DateTime.Now.Date,
        LastModification = DateTime.Now.Date,
        Content = reviewInfo.Content
      };

      await reviewCollection.InsertOneAsync(review);

      await LatestReviewsUpdate(reviewInfo.AlbumId);
    }
  public async Task AddFeatureReview(string reviewId)
  {
    ObjectId rId = new ObjectId(reviewId);
    IMongoCollection<FeaturedReview> featuredReviewCollection;
    IMongoCollection<Review> reviewCollection = mongoService.database.GetCollection<Review>("Review");
    IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
    var exist = mongoService.database.ListCollectionNames().ToList().Contains("FeatureReview");
    if(!exist)
    {
      await mongoService.database.CreateCollectionAsync("FeaturedReview");
    }
    var review = await reviewCollection.Find(Builders<Review>.Filter.Eq(r=>r.Id,rId)).FirstOrDefaultAsync();
    var album = await albumCollection.Find(Builders<Album>.Filter.Eq(r=>r.Id,review.AlbumId)).FirstOrDefaultAsync();

    FeaturedReview featuredReview = new FeaturedReview{
      fReview = new ReviewCard{
        Id = review.Id.ToString(),
        AlbumId = review.AlbumId.ToString(),
        UserId = review.UserId.ToString(),
        Username = review.Username,
        CreatedAt = review.CreatedAt,
        LastModification = review.LastModification,
        Content = review.Content
      },
      ArtistId = new ObjectId(album.Artist.Id),
      ArtistName = album.Artist.Name,
      AlbumTitle= album.Title,
      AlbumImageURL = album.ImageURL
    };
    featuredReviewCollection = mongoService.database.GetCollection<FeaturedReview>("FeaturedReview");
    featuredReviewCollection.InsertOne(featuredReview);
  }
  public async Task<List<FeaturedReviewCard>> GetFeaturedReviews()
  {
    IMongoCollection<FeaturedReview> featuredReviewCollection = mongoService.database.GetCollection<FeaturedReview>("FeaturedReview");
    List<FeaturedReview> featuredReviews = await featuredReviewCollection.Find(_ => true).ToListAsync();
    List<FeaturedReviewCard> cards = new List<FeaturedReviewCard>();
    foreach(var fr in featuredReviews)
    {
      cards.Add(new FeaturedReviewCard{
        Id = fr.Id.ToString(),
        fReview = fr.fReview,
        ArtistId = fr.ArtistId.ToString(),
        ArtistName = fr.ArtistName,
        AlbumTitle = fr.AlbumTitle,
        AlbumImageURL = fr.AlbumImageURL
      });
    }
    return cards;
  }

  private async Task LatestReviewsUpdate(string aId)
  {
    var albumId = new ObjectId(aId);
    var reviewCollection = mongoService.database.GetCollection<Review>("Review");
    var albumCollection = mongoService.database.GetCollection<Album>("Album");

    var recentReviews = reviewCollection.Find(r => r.AlbumId == albumId).SortByDescending(r => r.CreatedAt).Limit(5).ToList();

    var recentReviewCards = recentReviews.Select(r => new ReviewCard
    {
      Id = r.Id.ToString(),
      AlbumId = r.AlbumId.ToString(),
      UserId = r.UserId.ToString(),
      Username = r.Username,
      CreatedAt = r.CreatedAt,
      LastModification = r.LastModification,
      Content = r.Content
    })
    .ToList();
    var albumFilter = Builders<Album>.Filter.Eq(a => a.Id, albumId);
    var album = albumCollection.Find(albumFilter).FirstOrDefault();
    album.LatestReviews = recentReviewCards;
    await albumCollection.ReplaceOneAsync(albumFilter, album);
  } 
  public async Task DeleteReview(string reviewId)
  {
    IMongoCollection<Review> reviewCollection = mongoService.database.GetCollection<Review>("Review");
    IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");
    var filterReview = Builders<Review>.Filter.Eq(r=>r.Id,new ObjectId(reviewId));
    var review = await reviewCollection.Find(filterReview).FirstOrDefaultAsync();

    var album = await albumCollection.Find(Builders<Album>.Filter.Eq(r=>r.Id,review.AlbumId)).FirstOrDefaultAsync();
    var albumUpdate = Builders<Album>.Update.PullFilter(a => a.LatestReviews, lr =>lr.Id == reviewId);
    await albumCollection.UpdateOneAsync(Builders<Album>.Filter.Eq(r=>r.Id,review.AlbumId), albumUpdate);
    await reviewCollection.DeleteOneAsync(filterReview);
  }
  public async Task<Review> UpdateReview(string reviewId,string newContnent)
  {
    IMongoCollection<Review> reviewCollection = mongoService.database.GetCollection<Review>("Review");
    IMongoCollection<Album> albumCollection = mongoService.database.GetCollection<Album>("Album");

    var filter = Builders<Review>.Filter.Eq(r=>r.Id,new ObjectId(reviewId));
    var contentUpdate = Builders<Review>.Update.Set(r=>r.Content,newContnent);
    var modificationUpdate = Builders<Review>.Update.Set(r=>r.LastModification,DateTime.Now.Date);

    Task tContentUnpdate = reviewCollection.UpdateOneAsync(filter,contentUpdate);
    Task tDateModificationUpdate = reviewCollection.UpdateOneAsync(filter,modificationUpdate);

    await Task.WhenAll(tContentUnpdate,tDateModificationUpdate);

    Review updatedReview = await reviewCollection.Find(filter).FirstOrDefaultAsync();
    var updateCard = new ReviewCard{
      Id = updatedReview.Id.ToString(),
      AlbumId = updatedReview.AlbumId.ToString(),
      UserId = updatedReview.UserId.ToString(),
      Username = updatedReview.Username,
      CreatedAt = updatedReview.CreatedAt,
      LastModification = updatedReview.LastModification,
      Content = updatedReview.Content
    };

    var albumFilter = Builders<Album>.Filter.Eq(a => a.Id, updatedReview.AlbumId);
    var pullFilter = Builders<Album>.Update.PullFilter(a => a.LatestReviews, lr => lr.Id == reviewId);
    await albumCollection.UpdateOneAsync(albumFilter, pullFilter);


    var pushUpdate = Builders<Album>.Update.Push(a => a.LatestReviews, updateCard);
    await albumCollection.UpdateOneAsync(albumFilter, pushUpdate);

    return updatedReview;
 }
 public async Task<ReviewCard> GetReview(string albumId,string userId)
 {
   IMongoCollection<Review> reviewCollection = mongoService.database.GetCollection<Review>("Review");
   var filter = Builders<Review>.Filter.Eq(r=>r.AlbumId,new ObjectId(albumId)) & Builders<Review>.Filter.Eq(r=>r.UserId,new ObjectId(userId));
   Review review = await reviewCollection.Find(filter).FirstOrDefaultAsync();
   var card = new ReviewCard{
    Id = review.Id.ToString(),
    AlbumId = review.AlbumId.ToString(),
    UserId = review.UserId.ToString(),
    Username = review.Username,
    CreatedAt = review.CreatedAt,
    LastModification = review.LastModification,
    Content = review.Content
   };

   return card;
 }
  }
}