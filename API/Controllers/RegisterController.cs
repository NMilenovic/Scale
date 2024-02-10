using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.User;
using Services;

namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
  RegisterService registerService;

  public RegisterController()
  {
    registerService = new RegisterService();
  }
  
  [HttpPost("Register")]
  public async Task<IActionResult> Register([FromBody]UserRegisterDTO urDTO)
  {
    if(urDTO.Username.Length > 30 || String.IsNullOrWhiteSpace(urDTO.Username))
      return UnprocessableEntity("Username should be between 1 and 30 charachaters long");
    if(urDTO.Password1 != urDTO.Password2)
      return BadRequest("Passwords are not the same");
    if(urDTO.Password1.Length > 30 || urDTO.Password1.Length< 6 || urDTO.Password2.Length > 30 || urDTO.Password2.Length<6)
      return UnprocessableEntity("Password lenght must be between 6 and 30 charachters ");
    EmailAddressAttribute emailAtribute =  new EmailAddressAttribute();
    if(!emailAtribute.IsValid(urDTO.Email))
      return UnprocessableEntity("Format of email is invalid!");
    if(urDTO.Role == UserRole.Admin)
      return BadRequest("Admins cant be made using this endpoint!");
    var registerCheck = await registerService.Register(urDTO);
    if(!registerCheck)
      return BadRequest("User already exist");
    return Ok("User registered");
  }

  [HttpPost("RegisterAdmin")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> RegisterAdmin([FromBody]UserRegisterDTO urDTO)
  {
    if(urDTO.Username.Length > 30 || String.IsNullOrWhiteSpace(urDTO.Username))
      return UnprocessableEntity("Username should be between 1 and 30 charachaters long");
    if(urDTO.Password1 != urDTO.Password2)
      return BadRequest("Passwords are not the same");
    if(urDTO.Password1.Length > 30 || urDTO.Password1.Length< 6 || urDTO.Password2.Length > 30 || urDTO.Password2.Length<6)
      return UnprocessableEntity("Password lenght must be between 6 and 30 charachters ");
    EmailAddressAttribute emailAtribute =  new EmailAddressAttribute();
    if(!emailAtribute.IsValid(urDTO.Email))
      return UnprocessableEntity("Format of email is invalid!");
    if(urDTO.Role == UserRole.User)
      return BadRequest("Users cant be made using this endpoint!");
    var registerCheck = await registerService.Register(urDTO);
    if(!registerCheck)
      return BadRequest("Admin already exist");
    return Ok("User registered"); 
  }
}

