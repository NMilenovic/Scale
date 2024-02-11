import { ProfilePage } from "./ProfilePage.js";

export class NewListForm
{
  constructor(page,host,user)
  {
    this.page = page;
    this.host = host;
    this.user = user;
  }

  Draw()
  {
    while(this.host.firstChild)
      this.host.removeChild(this.host.firstChild);

      this.host.className = "NewList";
      console.log(this.host);
      let row = document.createElement("div");
      row.className = "NewListRow";
      this.host.appendChild(row);
    
      let label1 = document.createElement("label");
      label1.innerHTML = "List title";
      row.appendChild(label1);
      let input1 = document.createElement("input");
      row.appendChild(input1);
      
      row = document.createElement("div");
      row.className = "NewListRow";
      this.host.appendChild(row);
    
      let label2 = document.createElement("label");
      label2.innerHTML = "Image URL";
      row.appendChild(label2);
      let input2 = document.createElement("input");
      row.appendChild(input2);

      row = document.createElement("div");
      row.className = "NewListRow";
      this.host.appendChild(row);
      let label3 = document.createElement("label");
      label3.innerHTML = "Description";
      row.appendChild(label3);
      let textarea = document.createElement("textarea");
      row.appendChild(textarea);

    

      row = document.createElement("div");
      row.className = "NewListRow";
      this.host.appendChild(row);
      let createButton = document.createElement("button");
      createButton.innerHTML = "Create";
      createButton.classList.add("smallBtn","purpleBtn");
      createButton.onclick = (ev) =>{
        var payload = {
          userId : this.user.id,
          title : input1.value,
          description : textarea.value,
          listImageURL : input2.value
        }

        fetch("http://localhost:5272/List/CreateList",{
        method:"POST",
        headers: {"Content-Type":"Application/json"},
        body: JSON.stringify(payload)
      })
      .then(p =>{
        if(p.ok)
        {
          var pp = new ProfilePage(this.page,this.user,this.user.id);
          pp.Draw();
        };
      })
      }
      row.appendChild(createButton);
      
  }
}