export class RecentRating{
  constructor(recentRating)
  {
    this.albumTitle = recentRating.albumTitle;
    this.albumImageUrl = recentRating.albumImageURL;
    this.albumId = recentRating.albumId;
    this.artistName = recentRating.artistName;
    this.dateOfRating = recentRating.dateOfRating;
    this.artistId = recentRating.artistId;
    this.grade = recentRating.grade;
  }
}