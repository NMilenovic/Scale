import { LoginPage } from "./LoginPage.js";

export class LoginIndexPage{
  constructor(host)
  {
    this.container = host;
  }

  Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    let indexDiv = document.createElement("div");
    indexDiv.className = "LoginIndex";
    this.container.appendChild(indexDiv);
    let loginButton = document.createElement("button");
    loginButton.classList.add("bigestBtn","purpleBtn");
    loginButton.innerHTML = "Login";
    loginButton.onclick = (ev) =>{
      var lp = new LoginPage(this.container);
      lp.Draw();
    }

    indexDiv.appendChild(loginButton);

  }
}