using Microsoft.AspNetCore.Mvc;
using Models.User;
using Service;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  UserService userService;

  public UserController()
  {
    userService = new UserService();
  } 

  [HttpGet("GetUser/{id}")]
  public async Task<IActionResult> GetUser(string id)
  {
    if(String.IsNullOrWhiteSpace(id))
      return UnprocessableEntity("User id is required!");
    try
    {
      var page = await userService.GetUser(id);
      return Ok(page);
    }
    catch(Exception e)
    {
      return BadRequest("Internal server error: "+e.Message+" Stack trace: "+e.StackTrace);
    }
  }
  [HttpGet("SearchUser/{patern}")]
  public async Task<IActionResult> SearchUser(string patern)
  {
    if(String.IsNullOrWhiteSpace(patern))
      return UnprocessableEntity("Pattern must exist");
    if(patern.Length <3)
      return UnprocessableEntity("Search querry must be 3 or more charachters");
    try
    {
      List<UserCard> searchResult = await userService.SearchUser(patern);
      return Ok(searchResult);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}