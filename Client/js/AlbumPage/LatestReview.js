export class LatestReview
{
  constructor(host,review)
  {
    this.container = host;
    this.review = review;
  }

  Draw()
  {
    let divForReview = document.createElement("div");
    divForReview.className = "divForReview";
    console.log(this.review);
    this.container.appendChild(divForReview);

    let label = document.createElement("label");
    label.innerHTML = this.review.content;
    divForReview.appendChild(label);
  }
}