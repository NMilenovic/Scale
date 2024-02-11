import { GenrePage } from "../js/GenrePage/GenrePage.js";
import { IndexPage } from "../js/IndexPage/IndexPage.js";
import { LoginIndexPage } from "../js/LoginIndexPage/LoginIndexPage.js";
import { SearchResultsPage } from "../js/SearchPage/SearchResultsPage.js";
import { ProfilePage } from "../js/UserPage/ProfilePage.js";

export class NavBar{
  constructor(host,user)
  {
    this.container = host;
    this.user = user;
  }

  Draw()
  {
    let div = document.createElement("div");
    div.className = "NavBarMainDiv";

    this.container.appendChild(div);
    let button = document.createElement("button");
    button.classList.add("NavBarButton","fa-solid","fa-house")
    button.onclick = (ev) =>{
      var indexPage = new IndexPage(this.container,this.user);
      indexPage.Draw();
    }
    div.appendChild(button);
    button = document.createElement("button");
    button.classList.add("NavBarButton","fa-solid","fa-music");
    button.onclick = (ev) =>{
      var genrePage = new GenrePage(this.container,this.user);
      genrePage.Draw();
    }
    div.appendChild(button);

    let serachDiv = document.createElement("div");
    serachDiv.className = "NavBar-SearchDiv";
    div.appendChild(serachDiv);

    let searchInput = document.createElement("input");
    serachDiv.appendChild(searchInput);

    let select = document.createElement("select");
    serachDiv.appendChild(select);

    let op = document.createElement("option");
    op.innerHTML = "Artist";
    op.value = "Artist";
    select.appendChild(op);

    op = document.createElement("option");
    op.innerHTML = "Album";
    op.value = "Album";
    select.appendChild(op);

    op = document.createElement("option");
    op.innerHTML = "User";
    op.value = "User";
    select.appendChild(op);

    op = document.createElement("option");
    op.innerHTML = "List";
    op.value = "List";
    select.appendChild(op);

    let serachButton = document.createElement("button");
    serachButton.classList.add("NavBarButton","fa-solid","fa-magnifying-glass");
    serachButton.onclick = (ev) =>{
      if(searchInput.value.length <=2)
      {
        alert("Search querry must be longer than 2 charachters!");
        return;
      }
      let searchResults = new SearchResultsPage(this.container,this.user,select.value,searchInput.value);
      searchResults.Draw();
    }
    serachDiv.appendChild(serachButton);

    button = document.createElement("button");
    button.classList.add("NavBarButton","fa-solid","fa-right-to-bracket");
    if(this.user == null)
    {
      button.onclick = (ev)=>{
        var lip = new LoginIndexPage(this.container);
        lip.Draw();
      }
    }
    else
    {
      button.classList.remove("fa-right-to-bracket");
      button.classList.add("fa-user");
      button.onclick = (ev) =>{
        var pp = new ProfilePage(this.container,this.user,this.user.id);
        pp.Draw();
      }
    }
   
    div.appendChild(button);

  }
}