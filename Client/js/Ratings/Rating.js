export class Rating
{
  constructor(rating)
  {
    this.albumTitle = rating.albumName;
    this.albumImageUrl = rating.albumImageURL;
    this.albumId = rating.albumId;
    this.artistName = rating.artistName;
    this.dateOfRating = rating.dateOfRating;
    this.grade = rating.grade;
  }
}