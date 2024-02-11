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

    let div = document.createElement("div");
    div.classList = "LoginDiv";
    this.container.appendChild(div);

    let formDiv = document.createElement("div");
    formDiv.classList = "LoginDiv-Form";
    div.appendChild(formDiv);

    let emailInput = document.createElement("input");
    emailInput.className = "bigInput";
    emailInput.placeholder = "Your email";
    formDiv.appendChild(emailInput);
    let passwordInput = document.createElement("input");
    passwordInput.type = "password";
    passwordInput.className = "bigInput";
    passwordInput.placeholder = "Your password";
    formDiv.appendChild(passwordInput);

    let loginButton = document.createElement("button");
    loginButton.innerHTML = "Login";
    loginButton.classList.add("bigestBtn","purpleBtn");
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
    formDiv.appendChild(loginButton);
  }
}