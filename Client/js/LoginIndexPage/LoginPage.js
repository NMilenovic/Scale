import { IndexPage } from "../IndexPage/IndexPage.js";
import { User } from "../UserPage/User.js";

export class LoginPage{
  constructor(host)
  {
    this.container = host;
  }

  Draw()
  {
    while(this.container.firstChild)
      this.container.removeChild(this.container.firstChild)

    let emailInput = document.createElement("input");
    this.container.appendChild(emailInput);
    let passwordInput = document.createElement("input");
    this.container.appendChild(passwordInput);

    let loginButton = document.createElement("button");
    loginButton.innerHTML = "Login";
    loginButton.onclick = (ev) =>{
      fetch("http://localhost:5272/Login/Login/"+emailInput.value+"/"+passwordInput.value)
      .then(p =>{
        p.json().then(u =>{
          var user = new User(u.userId,u.role);
          var indexPage = new IndexPage(this.container,user);
          indexPage.Draw();
        });
      });
    }
    this.container.appendChild(loginButton);
  }
}