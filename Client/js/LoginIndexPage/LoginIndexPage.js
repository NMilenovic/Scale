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

    let loginButton = document.createElement("button");
    loginButton.innerHTML = "Login";
    loginButton.onclick = (ev) =>{
      var lp = new LoginPage(this.container);
      lp.Draw();
    }

    this.container.appendChild(loginButton);

  }
}