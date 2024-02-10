import { NavBar } from "../../Components/NavBar.js";
import { AlbumPage } from "../AlbumPage/AlbumPage.js";
import { ArtistPage } from "../ArtistPage/ArtistPage.js";
import { ListPage } from "../List/ListPage.js";
import { ProfilePage } from "../UserPage/ProfilePage.js";

export class SearchResultsPage
{
  constructor(host,user,searchFor,pattern)
  {
    this.container = host;
    this.user = user;
    this.searchFor = searchFor;
    this.pattern = pattern;
    this.results;
  }

  async Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    let navbar = new NavBar(this.container,this.user);
    navbar.Draw();


    var response = await fetch("http://localhost:5272/"+this.searchFor+"/Search"+this.searchFor+"/"+this.pattern);
    var results = await response.json();
    this.results = results;
    
    var contentDiv = document.createElement("contentDiv");
    this.container.appendChild(contentDiv);

    if(results[0] == null)
    {
      let noResults = document.createElement("div");
      noResults.className = "title";
      let label = document.createElement("label");
      label.innerHTML = "Sorry, there is no results for your query.";
      noResults.appendChild(label);
      contentDiv.appendChild(noResults);
    }
    else
    {
      if(this.searchFor == "Artist")
        this.DrawArtistsResults(contentDiv);
      if(this.searchFor == "Album")
        this.DrawAlbumsResults(contentDiv);
      if(this.searchFor == "List")
        this.DrawListsResults(contentDiv);
      if(this.searchFor == "User")
        this.DrawUsersResults(contentDiv);
    }
  }
  DrawArtistsResults(host)
  {
    let entry = document.createElement("div");
    entry.className = "Search-Entry";
    host.appendChild(entry);

    this.results.forEach(result =>{
      let img = document.createElement("img");
      img.src = result.imageURL;
      entry.appendChild(img);

      let infoDiv = document.createElement("div");
      infoDiv.className = "Search-Entry-Info";
      entry.appendChild(infoDiv);

      let l = document.createElement("label");
      l.className = "clickableLabel";
      l.innerHTML = result.name;
      l.onclick = (ev) =>{
        let artistPage = new ArtistPage(this.container,result.id,this.user);
        artistPage.Draw();
      };
      infoDiv.appendChild(l);
    })
  }
  DrawAlbumsResults(host)
  {
    let entry = document.createElement("div");
    entry.className = "Search-Entry";
    host.appendChild(entry);
    this.results.forEach(result =>{
      let img = document.createElement("img");
      img.src = result.imageURL;
      entry.appendChild(img);

      let infoDiv = document.createElement("div");
      infoDiv.className = "Search-Entry-Info";
      entry.appendChild(infoDiv);

      let l = document.createElement("label");
      l.className = "clickableLabel";
      l.innerHTML = result.title;
      l.onclick = (ev) =>{
        let albumPage = new AlbumPage(this.container,result.albumId,this.user);
        albumPage.Draw();
      };
      infoDiv.appendChild(l);
    });
  }
  DrawListsResults(host)
  {
    console.log(this.results);
    let entry = document.createElement("div");
    entry.className = "Search-Entry";
    host.appendChild(entry);
    this.results.forEach(result =>{
      let img = document.createElement("img");
      img.src = result.listImageURL;
      entry.appendChild(img);

      let infoDiv = document.createElement("div");
      infoDiv.className = "Search-Entry-Info";
      entry.appendChild(infoDiv);

      let l = document.createElement("label");
      l.className = "clickableLabel";
      l.innerHTML = result.title;
      l.onclick = (ev) =>{
        console.log("click");
        let lp = new ListPage(this.container,result.listId,this.user);
        lp.Draw();
      };
      infoDiv.appendChild(l);
    });
  }
  DrawUsersResults(host)
  {
    console.log(this.results);
    let entry = document.createElement("div");
    entry.className = "Search-Entry";
    host.appendChild(entry);
    this.results.forEach(result =>{
      let infoDiv = document.createElement("div");
      infoDiv.className = "Search-Entry-Info";
      entry.appendChild(infoDiv);

      let l = document.createElement("label");
      l.className = "clickableLabel";
      l.innerHTML = result.username;
      l.onclick = (ev) =>{
        let profilePage = new ProfilePage(this.container,this.user,result.userId);
        profilePage.Draw();
      };
      infoDiv.appendChild(l);
    });
  }
}