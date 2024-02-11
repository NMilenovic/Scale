import { NavBar } from "../../Components/NavBar.js";
import { AlbumPage } from "../AlbumPage/AlbumPage.js";
import { AlbumCard } from "../ArtistPage/AlbumCard.js";
import { Rating } from "./Rating.js";
import { RecentRating } from "./RecentRating.js";

export class AllRatingsPage{
  constructor(host,userId,user)
  {
    this.container = host;
    this.userId = userId;
    this.user = user;
  }
  async Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    let navbar = new NavBar(this.container,this.user);
    navbar.Draw();

    var response = await fetch("http://localhost:5272/Rating/GetAllRatings/"+this.userId);
    var results = await response.json();
    this.results = results;

    let contentDiv = document.createElement("div");
    this.container.appendChild(contentDiv);

    let ratingDiv;
    results.forEach(result =>{
      ratingDiv = document.createElement("div");
      ratingDiv.classList.add("AllRatings-Rating")
      contentDiv.appendChild(ratingDiv);
      let rating = new Rating(result);

      let img = document.createElement("img");
      img.src = rating.albumImageUrl;
      ratingDiv.appendChild(img);

      let l = document.createElement("label");
      l.innerHTML = rating.artistName;
      ratingDiv.append(l);

      l = document.createElement("label");
      console.log(rating)
      l.innerHTML = rating.albumTitle;
      l.classList.add("clickableLabel");
      l.onclick = (ev) =>{
        let albumPage = new AlbumPage(this.container,rating.albumId,this.user);
        albumPage.Draw();
      }
      ratingDiv.appendChild(l);

      let label = document.createElement("label")
      const date = new Date(rating.dateOfRating);
      const day = date.getUTCDate();
      const month = date.getUTCMonth() + 1; 
      const year = date.getUTCFullYear();

      label.innerHTML = `${day < 10 ? '0' : ''}${day}.${month < 10 ? '0' : ''}${month}.${year}`;
      ratingDiv.appendChild(label);

      label = document.createElement("label")
      label.innerHTML = rating.grade + "/5";
      ratingDiv.appendChild(label);
    });
  }
}