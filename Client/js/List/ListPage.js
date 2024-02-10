import { NavBar } from "../../Components/NavBar.js";
import { AlbumPage } from "../AlbumPage/AlbumPage.js";
import { ArtistPage } from "../ArtistPage/ArtistPage.js";
import { ProfilePage } from "../UserPage/ProfilePage.js";

export class ListPage{
  constructor(host,listId,currentUser)
  {
    this.container = host;
    this.listId = listId;
    this.currentUser = currentUser;
    this.list;
  }
  async Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)
    await this.FetchList();
    console.log(this.list);

    let navBar = new NavBar(this.container,this.currentUser);
    navBar.Draw();  

    this.DrawTopDiv();
    this.DrawContentDiv();
    
  }
  async FetchList()
  {
    let response = await fetch("http://localhost:5272/List/GetListById/"+this.listId);
    this.list = await response.json();
  }
  DrawTopDiv()
  {
    let topDiv = document.createElement("div");
    topDiv.classList.add("ListPage-TopDiv");
    this.container.appendChild(topDiv);

    let leftTopDiv = document.createElement("div");
    topDiv.appendChild(leftTopDiv);
    leftTopDiv.classList.add("ListPage-LeftTopDiv");

    let img = document.createElement("img");
    leftTopDiv.appendChild(img);
    img.src = this.list.listImageURL;

    let rightTopDiv = document.createElement("div");
    topDiv.appendChild(rightTopDiv);
    rightTopDiv.classList.add("ListPage-RightTopDiv");

    let titleDiv = document.createElement("div");
    titleDiv.className = "title";
    rightTopDiv.appendChild(titleDiv);
    let l = document.createElement("label");
    l.innerHTML = this.list.title;
    titleDiv.appendChild(l);

    let row = document.createElement("div");
    row.className = row;
    rightTopDiv.appendChild(row);
    l = document.createElement("label");
    l.innerHTML = "List created by: "
    row.appendChild(l);
    l = document.createElement("label");
    l.innerHTML = this.list.ownerUsername;
    l.classList.add("clickableLabel");
    l.onclick = (ev) =>{
      let pp = new ProfilePage(this.container,this.currentUser,this.list.ownerId);
      pp.Draw();
    }
    row.appendChild(l);

    row = document.createElement("div");
    row.className = "centeredDiv";
    rightTopDiv.appendChild(row);

    l = document.createElement("label");
    l.innerHTML = this.list.description;
    row.appendChild(l);

  }
  DrawContentDiv()
  {
    let contentDiv = document.createElement("div");
    contentDiv.className = "contentDiv";
    this.container.appendChild(contentDiv);
    if(this.list.listItems[0] == null)
    {
      let label = document.createElement("label");
      label.innerHTML = "This list is currently empty";
      contentDiv.appendChild(label);
    }
    let listItemDiv;
    this.list.listItems.forEach(listItem =>{
      listItemDiv = document.createElement("div");
      listItemDiv.classList = "ListPage-ListItemDiv";
      contentDiv.appendChild(listItemDiv);
      
      let div = document.createElement("div");
      listItemDiv.appendChild(div);

      let l = document.createElement("label");
      l.className = "clickableLabel";
      l.innerHTML = listItem.artistName;
      l.onclick = (ev) =>{
        let ap = new ArtistPage(this.container,listItem.artistId,this.currentUser);
        ap.Draw();
      }
      div.appendChild(l);
      
      l = document.createElement("label");
      l.className = "clickableLabel";
      l.innerHTML = listItem.albumName;
      l.onclick = (ev) =>{
        let ap = new AlbumPage(this.container,listItem.albumId,this.currentUser);
        ap.Draw();
      }
      div.appendChild(l);

      l = document.createElement("label");
      l.innerHTML = listItem.description;
      div.appendChild(l);
      if(this.currentUser.id == this.list.ownerId)
      {
        let i = document.createElement("i");
        i.classList.add("fa-solid","fa-x","clickable");
        listItemDiv.appendChild(i);

        i.onclick = async (ev) =>{
          await fetch("http://localhost:5272/List/DeleteAlbumFromList/"+this.list.id+"/"+listItem.place,{
            method: "PUT",
            headers: {"Content-type":"Application/json"}
          });
          let lp = new ListPage(this.container,this.listId,this.currentUser);
          lp.Draw();
        }
      }



    });
  }
}