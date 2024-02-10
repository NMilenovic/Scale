import { NavBar } from "../../Components/NavBar.js";
import { AlbumPage } from "../AlbumPage/AlbumPage.js";

export class Genre{
  constructor(host,user,genreId)
  {
    this.container = host;
    this.user = user;
    this.genreId = genreId;
    this.genre;
  }

  async Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    let navBar = new NavBar(this.container,this.user);
    navBar.Draw();
      
    let contentDiv = document.createElement("div");
    contentDiv.className = "Genre";
    this.container.appendChild(contentDiv);

    let response = await fetch("http://localhost:5272/Genre/GetGenreById/"+this.genreId);
    let genre = await response.json();
    this.genre = genre;
    console.log(genre);

    let infoDiv = document.createElement("div");
    infoDiv.className = "Genre-Info";
    contentDiv.appendChild(infoDiv);

    let img = document.createElement("img");
    img.src = this.genre.imageURL;
    infoDiv.appendChild(img);

    let infoOfGenre = document.createElement("div");
    infoOfGenre.className = "Genre-InfoOfGenre"
    infoDiv.appendChild(infoOfGenre);

    let row = document.createElement("div");
    row.className = "title";
    infoOfGenre.appendChild(row);

    let l = document.createElement("label");
    l.innerHTML = this.genre.name;
    row.appendChild(l);

    l = document.createElement("label");
    l.innerHTML = "Description";
    infoOfGenre.appendChild(l);

    let descriptionDiv = document.createElement("div");
    descriptionDiv.className = "Genre-Description";
    infoOfGenre.appendChild(descriptionDiv);

    l = document.createElement("label");
    l.innerHTML = this.genre.description;
    descriptionDiv.appendChild(l);

    let bestRatedDiv = document.createElement("div");
    bestRatedDiv.classList.add("Genre-BestRatedDiv");
    contentDiv.appendChild(bestRatedDiv)

    let h = document.createElement("h3");
    h.innerHTML = "Best rated albums in "+genre.name;
    bestRatedDiv.appendChild(h);

    console.log(this.genre);
    this.DrawBestRatedAlbums(bestRatedDiv);
  }
  DrawBestRatedAlbums(host)
  {
    let bestAlbumsDiv = document.createElement("div");
    bestAlbumsDiv.classList.add("Genre-BestAlbums");
    host.appendChild(bestAlbumsDiv);
    this.genre.bestAlbums.forEach(album =>{
      let img = document.createElement("img");
      img.src = album.imageURL;
      img.classList.add("clickable");
      img.onclick = (ev) =>{
        let ap = new AlbumPage(this.container,album.albumId,this.user);
        ap.Draw();
      }
      bestAlbumsDiv.appendChild(img);
    });
  }
}