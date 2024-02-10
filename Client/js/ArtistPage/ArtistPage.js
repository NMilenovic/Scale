import { NavBar } from "../../Components/NavBar.js";
import { Genre } from "../GenrePage/Genre.js";
import { AlbumCard } from "./AlbumCard.js";

export class ArtistPage{
  constructor(host,artistId,user)
  {
    this.container = host;
    this.artistId = artistId;
    this.user = user;
    this.artist;
  }

  async Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    var response =  await fetch("http://localhost:5272/Artist/GetArtist/"+this.artistId);
    var artist = await response.json();
    let navbar = new NavBar(this.container,this.user);
    navbar.Draw();
    
    this.artist = artist;
     
    let div = document.createElement("div");
    div.className = "ArtistPage";
    this.container.appendChild(div);

    console.log(this.user);
    let topDiv = document.createElement("div");
    topDiv.className = "ArtistPage-TopDiv";
    div.appendChild(topDiv);

    this.DrawTopDiv(topDiv);

    let bottomDiv = document.createElement("div");
    div.appendChild(bottomDiv);
    bottomDiv.className = "ArtistPage-BottomDiv";
    this.DrawBottomDiv(bottomDiv);

  }
  DrawTopDiv(topDiv)
  {
    let img = document.createElement("img");
    img.src = this.artist.pictureURL;
    topDiv.appendChild(img);

    let rightTopDiv = document.createElement("div");
    rightTopDiv.className = "ArtistPage-RightTopDiv";
    topDiv.appendChild(rightTopDiv);

    let h3 = document.createElement("h3");
    h3.innerHTML = this.artist.name;
    rightTopDiv.appendChild(h3);

    let h5 = document.createElement("h5");
    h5.innerHTML = "Description";
    rightTopDiv.appendChild(h5);  

    let label = document.createElement("label");
    label.innerHTML = this.artist.description;
    rightTopDiv.appendChild(label);  

    h5 = document.createElement("h5");
    h5.innerHTML = "Genres";
    rightTopDiv.appendChild(h5);  

    let divForGenres = document.createElement("div")
    divForGenres.className = "ArtistPage-DivForGenres";
    rightTopDiv.appendChild(divForGenres);

    let genreLab;
    this.artist.genres.forEach(genre =>{
      genreLab = document.createElement("label");
      genreLab.innerHTML = genre.name;
      genreLab.className = "clickableLabel";
      genreLab.onclick = (ev) =>{
        var genrePage = new Genre(this.container,this.user,genre.id);
        genrePage.Draw();
      }
      divForGenres.appendChild(genreLab);
    });
  }
  DrawBottomDiv(bottomDiv)
  {
    let albumDiv;
    this.artist.albums.forEach(album =>{
      albumDiv = document.createElement("div");
      bottomDiv.appendChild(albumDiv);
      let albumCard = new AlbumCard(this.container,albumDiv,album,this.user);
      albumCard.Draw();
    })
  }
}