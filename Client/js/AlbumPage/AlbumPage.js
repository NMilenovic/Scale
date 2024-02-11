import { NavBar } from "../../Components/NavBar.js";
import { ArtistPage } from "../ArtistPage/ArtistPage.js";
import { Genre } from "../GenrePage/Genre.js";
import { LatestReview } from "./LatestReview.js";

export class AlbumPage{
  constructor(host,albumId,user)
  {
    this.container = host;
    this.albumId = albumId;
    this.album;
    this.user = user;
    this.review;
  }

  async Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)
  
    var response =  await fetch("http://localhost:5272/Album/GetAlbumById/"+this.albumId);
    var album = await response.json();
    let navbar = new NavBar(this.container,this.user);
    navbar.Draw();
    this.album = album;

    let div = document.createElement("div");
    div.className = "AlbumPage";
    this.container.appendChild(div);

  

    let centerDiv = document.createElement("div");
    centerDiv.className = "AlbumPage-CenterDiv"
    div.appendChild(centerDiv);

    let leftDiv = document.createElement("div");
    leftDiv.className = "AlbumPage-LeftDiv";
    centerDiv.appendChild(leftDiv);
    this.DrawLeft(leftDiv);


    let rightDiv = document.createElement("div");
    rightDiv.className = "AlbumPage-RightDiv";
    centerDiv.appendChild(rightDiv);
    this.DrawRight(rightDiv);

  }

  DrawLeft(leftDiv)
  {
    let i = document.createElement("img");
    i.src = this.album.imageURL;

    leftDiv.appendChild(i);


    let title = document.createElement("label");
    title.innerHTML = "Tracklist"
    title.className = "title";
    leftDiv.appendChild(title);

    let table  = document.createElement("table");
    leftDiv.appendChild(table);

    let tbody = document.createElement("tbody");
    table.appendChild(tbody);

    let tr;
    let th;
    let td;
    this.album.trackList.forEach(track =>{
      tr = document.createElement("tr");
      tbody.appendChild(tr);
      th = document.createElement("th");
      th.innerHTML = track.name;
      tr.appendChild(th);
      td = document.createElement("td");
      td.innerText = track.minutes+":"+track.seconds;
      tr.appendChild(td);
    }); 

    this.DrawDiscussion(leftDiv);
  }

  async DrawRight(rightDiv)
  {
    let h3 = document.createElement("h3");
    h3.innerHTML = this.album.title;
    rightDiv.appendChild(h3);

    if(this.user != null)
    {
      let i = document.createElement("i");
      i.classList = "clickable fa-star";
      h3.appendChild(i);

      if(await this.IsFavorite())
      {
        i.classList.add("fa-solid");
        i.onclick = async (ev) =>{
          await this.RemoveFavorite();
          i.classList.remove("fa-solid");
          i.classList.add("fa-regular");
        }
      }
      else
      {
        i.classList.add("fa-regular");
        i.classList = "clickable fa-star fa-regular"
        i.onclick = async(ev) =>{
          await this.AddFavorite();
          i.classList.remove("fa-regular");
          i.classList.add("fa-solid");
        }
      }
     
      let addToListBtn = document.createElement("button");
      addToListBtn.classList.add("smallBtn");
      addToListBtn.classList.add("purpleBtn")
      addToListBtn.innerHTML = "Add to list";
      h3.appendChild(addToListBtn);
      addToListBtn.onclick = async (ev) =>{
       await this.AddAlbumToList();
      }
    }
    let table  = document.createElement("table");
    rightDiv.appendChild(table);

    let tbody = document.createElement("tbody");
    table.appendChild(tbody);

    let tr = document.createElement("tr");
    let th = document.createElement("th");
    th.innerHTML = "Artist";
    tr.appendChild(th);
    let td = document.createElement("td");
    td.innerHTML = this.album.artist.name;
    td.className = "clickableLabel";
    td.onclick = (ev) =>{
      var artistPage = new ArtistPage(this.container,this.album.artist.id,this.user);
      artistPage.Draw();
    }
    tr.appendChild(td);
    tbody.appendChild(tr);

    tr = document.createElement("tr");
    th = document.createElement("th");
    th.innerHTML = "Relese date";
    tr.appendChild(th);
    td = document.createElement("td");
    const date = new Date(this.album.releaseDate);
    const day = date.getUTCDate();
    const month = date.getUTCMonth() + 1; 
    const year = date.getUTCFullYear();
    td.innerHTML = day+"."+month+"."+year+".";
    tr.appendChild(td);
    tbody.appendChild(tr);

    tr = document.createElement("tr");
    th = document.createElement("th");
    th.innerHTML = "Average grade";
    tr.appendChild(th);
    td = document.createElement("td");
    td.innerHTML = this.album.avregeRating;
    tr.appendChild(td);
    tbody.appendChild(tr);

    tr = document.createElement("tr");
    th = document.createElement("th");
    th.innerHTML = "Votes count";
    tr.appendChild(th);
    td = document.createElement("td");
    td.innerHTML = this.album.ratingCount;
    tr.appendChild(td);
    tbody.appendChild(tr);

    let h5 = document.createElement("h5");
    h5.innerHTML = "Genres";
    rightDiv.appendChild(h5);

    let divForGenres = document.createElement("div")
    divForGenres.className = "AlbumPage-DivForGenres";
    rightDiv.appendChild(divForGenres);

    let genreLab;
    this.album.genres.forEach(genre =>{
      genreLab = document.createElement("label");
      genreLab.innerHTML = genre.name;
      genreLab.className = "clickableLabel";
      genreLab.onclick = (ev) =>{
        let genrePage = new Genre(this.container,this.user,genre.id);
        genrePage.Draw();
      }
      divForGenres.appendChild(genreLab);
    });

    let ratingDiv = document.createElement("div");
    

    let ratingInput = document.createElement("select");
    ratingInput.className = "Rating";
    let op;
    for(let i=1;i<=5;i++)
    {
      op = document.createElement("option");
      op.value = i;
      op.innerHTML = i;
      ratingInput.appendChild(op);
    }
    rightDiv.appendChild(ratingDiv);
    if(this.user != null)
    {
    fetch("http://localhost:5272/Rating/GetRating/"+this.albumId+"/"+this.user.id)
    .then(response =>{
      if(response.status == 200)
      {
        let l = document.createElement("label");
        let modifyBtn = document.createElement("button");
        modifyBtn.classList.add("smallBtn","purpleBtn");
        let ratingId;
        response.json().then(rating =>{
          l.innerHTML = rating.grade;
          ratingId = rating.id;
          ratingDiv.appendChild(l); 
        });
        modifyBtn.innerHTML = "Delete grade";
        
        modifyBtn.onclick = async(ev) =>
        {
          await fetch("http://localhost:5272/Rating/DeleteRating/"+ratingId,{
            method: "DELETE",
            headers: {"Content-type":"Application/json"}
          });
          let ap = new AlbumPage(this.container,this.albumId,this.user);
          ap.Draw();
        }
        ratingDiv.appendChild(modifyBtn);

      }
      else if(response.status == 400 )
      {
        let rateAlbumBtn = document.createElement("button");
        rateAlbumBtn.classList.add("smallBtn","purpleBtn");
        rateAlbumBtn.innerHTML = "Rate album";
        
        rateAlbumBtn.onclick = async (ev) =>{
          var grade = document.querySelector(".Rating").value;

          let ratingData = {
            grade : grade,
            albumName : this.album.title,
            artistName : this.album.artist.name,
            albumImageURL : this.album.imageURL
          };
          await fetch("http://localhost:5272/Rating/RateAlbum/"+this.user.id+"/"+this.albumId,{
            method: "POST",
            headers: {"Content-type":"Application/json"},
            body: JSON.stringify(ratingData) 
          });
          let ap = new AlbumPage(this.container,this.albumId,this.user);
          ap.Draw();
        };
        ratingDiv.appendChild(ratingInput);
        ratingDiv.appendChild(rateAlbumBtn);
      }
    });

    let reviewInputDiv = document.createElement("div");
    rightDiv.appendChild(reviewInputDiv);
    if(await this.ReviewExist())
    {
      await this.GetReview();
      this.displayReview(reviewInputDiv)
    }
    else
    {
      let createReviewButton = document.createElement("button");
      createReviewButton.textContent = "Create Review";
      createReviewButton.classList.add("smallBtn","purpleBtn","createReviewBtn")
      createReviewButton.onclick = async () => {
        if (reviewInputDiv.contains(createReviewButton)) {
          reviewInputDiv.removeChild(createReviewButton);
        }
        let textarea = document.createElement("textarea");
        textarea.placeholder = "Write your review here...";
        reviewInputDiv.appendChild(textarea);

        let okButton = document.createElement("button");
        okButton.textContent = "OK";
        okButton.classList.add("smallBtn","purpleBtn");
        okButton.onclick = async (ev) => {
            let reviewContent = textarea.value;

            if (reviewContent.trim() !== "") {
                console.log(this.user);
                let reviewData = {
                  albumId: this.albumId,
                  userId: this.user.id,
                  content: reviewContent
                };
                await fetch("http://localhost:5272/Review/CreateReview",{
                  method:"POST",
                  headers:{"Content-type":"Application/Json"},
                  body:JSON.stringify(reviewData)
                });
                var ap = new AlbumPage(this.container,this.albumId,this.user);
                ap.Draw();
            } else {
                alert("Review must containt some conent!");
            }
        };

        
        reviewInputDiv.appendChild(okButton);
    };


    reviewInputDiv.appendChild(createReviewButton)
    }
    }

  let latestReviewsDiv = document.createElement("div");
  latestReviewsDiv.className = "AlbumPage-LetestReviews";
  rightDiv.appendChild(latestReviewsDiv);
  if(this.album.latestReviews != null)
  {
    let l = document.createElement("h4");
    l.innerHTML = "Latest reviews";
    latestReviewsDiv.appendChild(l);
    this.album.latestReviews.forEach(review =>{
      let r = new LatestReview(latestReviewsDiv,review);
      r.Draw();
    });
  }
  else
  {
    console.log("NO reviews");
  }
}
  async IsFavorite()
  {
    let request = await fetch("http://localhost:5272/Album/IsFavorite/"+this.album.albumId+"/"+this.user.id);
    let response = await request.json();
    return response;
  }
  async AddFavorite()
  {
    let response = await fetch("http://localhost:5272/Album/AddFavoriteAlbum/"+this.album.albumId+"/"+this.user.id,{
      method:"POST",
      headers: {"Content-type": "Application/json"}
    });
  }
  async RemoveFavorite()
  {
    let response = await fetch("http://localhost:5272/Album/RemoveFavoriteAlbum/"+this.album.albumId+"/"+this.user.id,{
      method: "DELETE",
      headers: {"Content-type": "Application/json"}
    });
  }
  async ReviewExist()
  {
    let response = await fetch("http://localhost:5272/Review/GetReview/"+this.user.id+"/"+this.album.albumId)
    if(response.ok)
      return true;
    else 
      return false;
  }
  async GetReview()
  {
    let response = await fetch("http://localhost:5272/Review/GetReview/"+this.user.id+"/"+this.album.albumId);
    this.review = await response.json();
  }
  displayReview(reviewInputDiv)
  {
    let label = document.createElement("label");
    label.innerHTML = this.review.content;
    reviewInputDiv.appendChild(label);

    let modifyButton = document.createElement("button");
    modifyButton.innerHTML = "Modify review";
    modifyButton.classList.add("smallBtn","purpleBtn");
    
    modifyButton.onclick = (ev) => {
        modifyButton.style.display = "none"
        let textarea = document.createElement("textarea");
        textarea.value = this.review.content;
        reviewInputDiv.replaceChild(textarea, label);
    
        let okButton = document.createElement("button");
        okButton.textContent = "OK";
        okButton.classList.add("smallBtn","purpleBtn");
        
        let review = this.review;
        okButton.onclick = async function () {
            modifyButton.style.display = "block";
            review.content = textarea.value; 
            await fetch("http://localhost:5272/Review/UpdateReview/"+review.id+"/"+review.content,{
              method: "PUT",
              headers: {"Content-type":"Application/json"}
            });
            label.textContent = textarea.value;
            reviewInputDiv.replaceChild(label, textarea);
            reviewInputDiv.removeChild(okButton);
            reviewInputDiv.removeChild(deleteButton)
        };
        this.review = review;
        reviewInputDiv.appendChild(okButton);
        let deleteButton = document.createElement("button");
        deleteButton.textContent = "Delete review";
        okButton.classList.add("smallBtn","purpleBtn");
        let c = this.container;
        let a = this.albumId;
        let cu = this.user;
        deleteButton.addEventListener("click", async function () {
            await fetch("http://localhost:5272/Review/DeleteReview/"+review.id,{
              method: "DELETE",
              headers: {"Content-type":"Application/json"}
            });
            let albumPage = new AlbumPage(c,a,cu);
            albumPage.Draw();
        });
        reviewInputDiv.appendChild(deleteButton);
      }
    reviewInputDiv.appendChild(modifyButton);
  }
  DrawDiscussion(host)
  {
    let disscussionDiv = document.createElement("div");
    host.appendChild(disscussionDiv);
    disscussionDiv.classList.add("AlbumPage-DiscussionDiv");

    let title = document.createElement("label");
    title.className = "title"
    title.innerHTML = "Disscussion"
    disscussionDiv.appendChild(title);  
    let discussionDivFeild = document.createElement("div");
    discussionDivFeild.classList.add("AlbumPage-DiscussionDivFeild");
    disscussionDiv.appendChild(discussionDivFeild);

    let messDiv;
    this.album.messages.forEach(mess =>{
      messDiv = document.createElement("div");
      messDiv.classList.add("AlbumPage-MessageDiv");
      discussionDivFeild.appendChild(messDiv);
      let row = document.createElement("div");
      row.classList.add("row");
      row.classList.add("spread");
      messDiv.appendChild(row);
      let l = document.createElement("label");
      l.innerHTML = mess.username+": "
      row.appendChild(l);
      l = document.createElement("label");
      l.innerHTML = mess.content;
      row.appendChild(l);
    })
    if(this.user != null)
    {
      console.log("ovde");
      let input = document.createElement("input");
      disscussionDiv.appendChild(input);

      let sendBtn = document.createElement("button");
      sendBtn.innerHTML = "Send message"
      disscussionDiv.appendChild(sendBtn)
      sendBtn.onclick = async (ev) =>
      {
        if(input.value.length === 0)
        {
          alert("Message must containt content");
          return;
        }
        await fetch("http://localhost:5272/Album/SendMessage/"+this.albumId+"/"+this.user.id+"/"+input.value,{
          method: "POST",
          headers: {"Content-type":"Application/json"}
        });
        let ap = new AlbumPage(this.container,this.albumId,this.user);
        ap.Draw();
      }

    }
  }
  async AddAlbumToList()
  {
    let monadDiv = document.createElement("div");
    monadDiv.classList.add("monad");
    this.container.appendChild(monadDiv);

    let inputDiv = document.createElement("div");
    inputDiv.className = "AlbumPage-InputDiv";
    monadDiv.appendChild(inputDiv);

    let titleDiv = document.createElement("div");
    titleDiv.classList.add("title");
    inputDiv.appendChild(titleDiv);

    let l = document.createElement("label");
    l.innerHTML = "Add album to list";
    titleDiv.appendChild(l);
    console.log(this.user)
    
    let row = document.createElement("div");
    row.classList.add("spread","padded")
    inputDiv.appendChild(row);
    l = document.createElement("label");
    l.innerHTML = "Chose list: ";
    row.appendChild(l);

    let select = document.createElement("select");
    let op;
    let userReq = await fetch("http://localhost:5272/User/GetUser/"+this.user.id);
    this.user = await userReq.json();
    this.user.listCards.forEach(card =>{
      op = document.createElement("option");
      op.innerHTML = card.title;
      op.value = card.listId;
      select.appendChild(op);
    })
    row.appendChild(select);

    row = document.createElement("div");
    row.classList.add("spread","padded");
    inputDiv.appendChild(row);
    l = document.createElement("label");
    l.innerHTML = "Description: ";
    row.appendChild(l);

    let textarea = document.createElement("textarea");
    row.appendChild(textarea);

    let buttonsDiv = document.createElement("div");
    buttonsDiv.classList.add("AlbumPage-InputDiv-ButtonsDiv");
    inputDiv.appendChild(buttonsDiv);

    let cancelBtn = document.createElement("button");
    cancelBtn.innerHTML = "Cancel";
    cancelBtn.classList.add("smallBtn","purpleBtn");
    cancelBtn.onclick = (ev) =>{
      let ap = new AlbumPage(this.container,this.albumId,this.user);
      ap.Draw();
    }
    buttonsDiv.appendChild(cancelBtn);

    let addBtn = document.createElement("button");
    addBtn.innerHTML = "Add";
    addBtn.classList.add("smallBtn","purpleBtn");
    addBtn.onclick = async (ev) =>{
      if(textarea.value == "")
      {
        alert("Please insert list description!");
        return;
      }
      let res = await fetch("http://localhost:5272/List/AddAlbumToList/"+select.value+"/"+this.albumId+"/"+textarea.value,{
        method:"PUT",
        headers: {"Content-type":"Application/json"}
      });
      if(res.ok)
      {
        alert("Album added to list!");
        let ap = new AlbumPage(this.container,this.albumId,this.user);
        ap.Draw();
      }
    }
    buttonsDiv.appendChild(addBtn);


  }

}