import { AlbumPage } from "../AlbumPage/AlbumPage.js";

export class AlbumCard{
  constructor(page,host,album,user)
  {
    this.page = page;
    this.container = host;
    this.album = album;
    this.currentUser = user;
  }

  Draw()
  {
    let div = document.createElement("div");
    div.className = "AlbumCard";
    this.container.appendChild(div);

    //slika
    //title
    //releseday

    let img = document.createElement("img");
    img.src = this.album.imageURL;
    div.appendChild(img);

    let title = document.createElement("label");
    title.className = "clickableLabel";
    title.innerHTML = this.album.title;
    title.onclick = (ev) =>{
      var albumPage = new AlbumPage(this.page,this.album.albumId,this.currentUser);
      albumPage.Draw();
    };
    //Fix datum
    div.appendChild(title);
    var releseDate = new Date(this.album.releseDate);
    let releseDateLabel = document.createElement("label");
    releseDateLabel.innerHTML = releseDate.getDay()+"."+releseDate.getMonth()+"."+releseDate.getFullYear()+".";
    div.appendChild(releseDateLabel);
  }

}