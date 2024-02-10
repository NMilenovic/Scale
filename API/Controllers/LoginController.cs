using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;

namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
  LoginService loginService;

  public LoginController()
  {
    loginService = new LoginService();
  }

  [AllowAnonymous]
  [HttpGet("Login/{email}/{password}")]
  public async Task<IActionResult> Login(string email,string password)
  {
    var ulDTO = new UserLoginDTO{
      Email = email,
      Password = password
    };
    if(ulDTO.Password == null || ulDTO.Password.Length < 6 || ulDTO.Password.Length > 30)
       return UnprocessableEntity("Password lenght must be between 6 and 30 charachters ");
    EmailAddressAttribute emailAtribute =  new EmailAddressAttribute();
    if(!emailAtribute.IsValid(ulDTO.Email))
      return UnprocessableEntity("Emial format is invalid");
    try
    {
      var ud = await loginService.Login(ulDTO);
      if(ud == null)
        return BadRequest("User dosent exist");
      return Ok(ud);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}