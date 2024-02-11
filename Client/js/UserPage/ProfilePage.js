import { NavBar } from "../../Components/NavBar.js";
import { AlbumPage } from "../AlbumPage/AlbumPage.js";
import { ArtistPage } from "../ArtistPage/ArtistPage.js";
import { ListCard } from "../List/ListCard.js";
import { ListPage } from "../List/ListPage.js";
import { AllRatingsPage } from "../Ratings/AllRatingsPage.js";
import { RecentRating } from "../Ratings/RecentRating.js";
import { NewListForm } from "./NewListForm.js";

export class ProfilePage{
  constructor(host,currentUser,userId)
  {
    this.container = host;
    this.currentUser = currentUser;
    this.userId = userId;
    this.user;
  }

  async Draw()
  {
    var response = await fetch("http://localhost:5272/User/GetUser/"+this.userId);
    var user = await response.json();
    this.user = user;

    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    let navBar = new NavBar(this.container,this.currentUser);
    navBar.Draw();

    let profileDiv = document.createElement("div");
    profileDiv.className = "ProfilePage"
    this.container.appendChild(profileDiv);

    let basicInfoDiv = document.createElement("div");
    profileDiv.appendChild(basicInfoDiv);
    this.DrawBasicInfo(basicInfoDiv);

    let favoriteAlbumsDiv = document.createElement("div");
    profileDiv.appendChild(favoriteAlbumsDiv);
    this.DrawFavoriteAlbums(favoriteAlbumsDiv);

    let recentRatingsDiv = document.createElement("div");
    profileDiv.appendChild(recentRatingsDiv);
    this.DrawRecentRatings(recentRatingsDiv);

    let usersListDiv = document.createElement("div");
    profileDiv.appendChild(usersListDiv);
    this.DrawUserLists(usersListDiv);
  }   
  DrawBasicInfo(host)
  {
    host.className = "ProfilePage-BasicInfo"
    let title = document.createElement("div");
    host.appendChild(title);
    title.className = "bigTitle";

    let label = document.createElement("label");
    label.innerHTML = this.user.username;
    title.appendChild(label);
  }
  DrawFavoriteAlbums(host)
  {
    let title = document.createElement("div");
    title.className = "title";
    host.appendChild(title);
    let label = document.createElement("label");
    label.innerHTML = this.user.username+" favorite albums";
    title.appendChild(label);

    let favAlbums = document.createElement("div");
    favAlbums.className = "favAlbums";
    host.appendChild(favAlbums);

    host.className = "ProfilePage-FavoriteAlbums";
    if(this.user.favoriteAlbums[0] == null)
    {
      let label = document.createElement("label");
      label.innerHTML = "User didnt pick any album as their favorite.";
      favAlbums.appendChild(label);
    }
    else
    {
      let albumDiv;
      this.user.favoriteAlbums.forEach(album =>{
        albumDiv = document.createElement("div");
        albumDiv.className = "ProfilePage-FavAlbumDiv";
        let img = document.createElement("img");
        img.src = album.albumImageURL;
        img.classList.add("clickable");
        img.onclick = (ev) =>{
          let albumPage = new AlbumPage(this.container,album.albumId,this.currentUser);
          albumPage.Draw();
        }
        albumDiv.appendChild(img);
        favAlbums.appendChild(albumDiv);
      })
    }
  }
  DrawRecentRatings(host)
  {
    host.className = "ProfilePage-RecentRatings:";
    let title = document.createElement("div");
    host.appendChild(title);

    let label = document.createElement("label");
    label.innerHTML = this.user.username+" recent ratings:";
    title.appendChild(label);
    if(this.user.recentRatings[0] == null)
    {
      let centerDiv = document.createElement("div");
      centerDiv.className = "centeredDiv";
      host.appendChild(centerDiv);
      let label = document.createElement("label");
      label.innerHTML = "User didnt rate any album";
      centerDiv.appendChild(label);
    }
    else
    {
      let ratingDiv;
      this.user.recentRatings.forEach(rating =>{
        ratingDiv = document.createElement("div");
        ratingDiv.className = "ProfilePage-RatingDiv";
        host.appendChild(ratingDiv);
        let recentRating = new RecentRating(rating);

        let image = document.createElement("img");
        image.src = recentRating.albumImageUrl;
        ratingDiv.appendChild(image);

        let label = document.createElement("label")
        label.className = "clickableLabel";
        label.onclick = (ev) => {
          var artistPage = new ArtistPage(this.container,recentRating.artistId,this.currentUser);
          artistPage.Draw();
        }
        label.innerHTML = recentRating.artistName;
        ratingDiv.appendChild(label);

        label = document.createElement("label")
        label.className = "clickableLabel";
        label.onclick = (ev) =>{
          var albumPage = new AlbumPage(this.container,recentRating.albumId,this.currentUser);
          albumPage.Draw();
        }
        label.innerHTML = recentRating.albumTitle;
        ratingDiv.appendChild(label);

        label = document.createElement("label")
        const date = new Date(recentRating.dateOfRating);
        const day = date.getUTCDate();
        const month = date.getUTCMonth() + 1; 
        const year = date.getUTCFullYear();

        label.innerHTML = `${day < 10 ? '0' : ''}${day}.${month < 10 ? '0' : ''}${month}.${year}`;
        ratingDiv.appendChild(label);

        label = document.createElement("label")
        label.innerHTML = recentRating.grade + "/5";
        ratingDiv.appendChild(label);
      })
      let allRatings = document.createElement("button");
      allRatings.classList.add("smallBtn","purpleBtn");
      allRatings.innerHTML = "All reviews";
      allRatings.onclick = (ev) =>{
        console.log(this.user);
        let arp = new AllRatingsPage(this.container,this.userId,this.currentUser);
        arp.Draw();
      }
      host.appendChild(allRatings);
    }
  }
  DrawUserLists(host)
  {
    host.className = "ProfilePage-UsersLists";
    
    if(this.currentUser != null)
    {
    if(this.userId === this.currentUser.id)
    {
      let buttonDiv = document.createElement("div")
      buttonDiv.classList.add("centeredDiv","ProfilePageCreateList")
      host.appendChild(buttonDiv);
  
      let addListBtn = document.createElement("button");
      addListBtn.innerHTML = "Create new list";
      addListBtn.classList.add("bigBtn","purpleBtn")
      buttonDiv.appendChild(addListBtn);
      let formDiv = document.createElement("div");
      buttonDiv.appendChild(formDiv);
      addListBtn.onclick = (ev) =>{
        let listForm = new NewListForm(this.container,formDiv,this.currentUser);
        listForm.Draw();
      }
    }
    }
    if(this.user.listCards[0] == null)
    {
      let centerDiv = document.createElement("div");
      centerDiv.className = "centeredDiv";
      host.appendChild(centerDiv);
      let label = document.createElement("label");
      label.innerHTML = "User currently dont have any list ";
      centerDiv.appendChild(label);
    }
    else
    {

      let listCardDiv;
      this.user.listCards.forEach(card =>{
        listCardDiv = document.createElement("div");
        listCardDiv.className = "ProfilePage-ListCardDiv";
        host.appendChild(listCardDiv);
        let listCard = new ListCard(card);

        let img = document.createElement("img");
        img.src = listCard.listImageURL;
        listCardDiv.appendChild(img);

        let label = document.createElement("label");
        label.className = "clickableLabel";
        label.innerHTML = listCard.title;
        label.onclick = (ev) =>{
          let lp = new ListPage(this.container,card.listId,this.currentUser);
          lp.Draw();
        }
        listCardDiv.appendChild(label);

        if(this.currentUser.id = this.userId)
        {
        let i = document.createElement("i");
        i.classList = "fa-solid fa-x";
        listCardDiv.appendChild(i);
        i.onclick = (ev) =>{
          fetch("http://localhost:5272/List/RemoveList/"+card.listId+"/"+this.userId,{
            method: "DELETE",
            headers: {"Content-Type":"Application/json"}
          })
          .then(p =>{
            i.parentElement.remove();
          });
        }
      }
      });
    }

  }
}