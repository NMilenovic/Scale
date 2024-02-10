import { NavBar } from "../../Components/NavBar.js";
import { Genre } from "./Genre.js";

export class GenrePage{
  constructor(host,user)
  {
    this.container = host;
    this.user = user;
  }

  Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    let navBar = new NavBar(this.container,this.user);
    navBar.Draw();
  
    //ContentDiv 
    let contentDiv = document.createElement("div");
    contentDiv.className = "contentDiv";
    this.container.appendChild(contentDiv);

    let genreDiv;
    
  
    fetch("http://localhost:5272/Genre/GetGenresTabs")
    .then(p =>{
      p.json().then(genres =>{
        genres.forEach(genre =>{
          genreDiv = document.createElement("div");
          genreDiv.className = "GenrePage-GenreDiv";
          let img = document.createElement("img");
          img.src = genre.imageURL;
          genreDiv.appendChild(img);

          let infoDiv = document.createElement("div");
          infoDiv.className = "GenrePage-InfoDiv";
          genreDiv.appendChild(infoDiv);

          let h4 = document.createElement("h4");
          h4.innerHTML = genre.name;
          h4.className = "clickableLabel";
          h4.onclick = (ev) =>{
            let zanr = new Genre(this.container,this.user,genre.id);
            zanr.Draw();
          }
          infoDiv.appendChild(h4);

          let p = document.createElement("p");
          p.innerHTML = genre.description;
          infoDiv.append(p);
          contentDiv.appendChild(genreDiv);
        })
      })
    });
  }
 
}