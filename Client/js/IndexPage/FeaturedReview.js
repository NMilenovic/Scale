import { AlbumPage } from "../AlbumPage/AlbumPage.js";
import { ArtistPage } from "../ArtistPage/ArtistPage.js";
import { ProfilePage } from "../UserPage/ProfilePage.js";

export class FeaturedReview
{
  constructor(page,host,username,artistName,artistId,albumTitle,albumId,imageUrl,reviewContent,userId,user)
  {
    this.page = page,
    this.container = host,
    this.username = username,
    this.artistName = artistName,
    this.artistId = artistId,
    this.albumTitle = albumTitle,
    this.albumId = albumId,
    this.imageUrl = imageUrl,
    this.reviewContent = reviewContent,
    this.userId = userId
    this.user = user;
  }

  Draw()
  {
    let div = document.createElement("div");
    div.className = "FeaturedReview-MainDiv";
    this.container.appendChild(div);

    let leftDiv = document.createElement("div");
    let rightDiv = document.createElement("div");
    rightDiv.className = "FeaturedReview-RightDiv";

    let i = document.createElement("img");
    i.src = this.imageUrl;
    leftDiv.appendChild(i);
    
    let label = document.createElement("label");
    label.className = "clickableLabel";
    label.innerHTML = this.albumTitle;
    label.onclick = (ev)=>{
      var artistPage = new AlbumPage(this.page,this.albumId,this.user);
      artistPage.Draw();
    } 
    rightDiv.appendChild(label);

    label = document.createElement("label");
    label.className = "clickableLabel";
    label.onclick = (ev)=>{
      var artistPage = new ArtistPage(this.page,this.artistId,this.user);
      artistPage.Draw();
    } 
    label.innerHTML = this.artistName;
    rightDiv.appendChild(label);

    let row = document.createElement("div");
    row.className = "row";
    label = document.createElement("label");
    label.innerHTML = "Review by ";
    row.appendChild(label);
    label = document.createElement("label");
    label.className = "clickableLabel";
    label.innerHTML = this.username;
    label.onclick = (ev) =>{
      console.log(this.userId);
      var userPage = new ProfilePage(this.page,this.user,this.userId);
      userPage.Draw();
    }
    row.appendChild(label);
    rightDiv.appendChild(row);

    label = document.createElement("label");
    label.innerHTML = this.reviewContent;
    rightDiv.appendChild(label);


    div.appendChild(leftDiv);
    div.appendChild(rightDiv);

  }
}