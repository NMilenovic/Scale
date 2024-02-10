import { NavBar } from "../../Components/NavBar.js";
import { FeaturedReview } from "./FeaturedReview.js";

export class IndexPage
{
  constructor(host,user)
  {
    this.container = host;
    this.user = user;
  }

  Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    
    //NavBar full widht
    let navBar = new NavBar(this.container,this.user);
    navBar.Draw();

    //ContentDiv 
    let contentDiv = document.createElement("div");
    contentDiv.className = "contentDiv";
    this.container.appendChild(contentDiv);
    
    //Welcome
    let welcomeDiv = document.createElement("div");
    let label = document.createElement("label");
    label.classList.add("IndexWelcomeLabel");
    label.innerHTML = "Welcome to Scale - where music speaks volumes! Join us to rate, review, and discuss the beats that move you. Let's amplify the conversation together!";
    welcomeDiv.appendChild(label);
    contentDiv.append(welcomeDiv);

    //Featured review titles

    //Featured reviews


    fetch("http://localhost:5272/Review/GetFeaturedReviews")
    .then(p =>{
      p.json().then(reviews =>{
        let reviewDiv;
        let featuredRevew;
        reviews.forEach(review =>{
          reviewDiv = document.createElement("div");
          contentDiv.appendChild(reviewDiv);
          featuredRevew = new FeaturedReview(
            this.container,
            reviewDiv, 
            review.fReview.username, 
            review.artistName, 
            review.artistId, 
            review.albumTitle, 
            review.fReview.albumId, 
            review.albumImageURL, 
            review.fReview.content,
            review.fReview.userId,
            this.user
            );          
            featuredRevew.Draw();
        });
      });
    });





  }

}