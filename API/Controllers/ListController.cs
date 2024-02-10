using Microsoft.AspNetCore.Mvc;
using Models.List;
using Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ListController : ControllerBase
{
  private ListService listService;

  public ListController()
  {
    listService = new ListService();
  }

  [HttpPost("CreateList")]
  public async Task<IActionResult> CreateList([FromBody]ListInfo listInfo)
  {
    if(String.IsNullOrWhiteSpace(listInfo.UserId))
      return UnprocessableEntity("UserID must exist!");
    if(String.IsNullOrWhiteSpace(listInfo.Title))
      return UnprocessableEntity("Title must exist!");
    if(listInfo.Description.Length > 5000)
      return UnprocessableEntity("List description cant be longer than 5000 charachters");
    try
    {
      await listService.CreateList(listInfo);
      return Ok("List created");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    } 
  }
  [HttpDelete("RemoveList/{listId}/{userId}")]
  public async Task<IActionResult> RemoveList(string listId,string userId)
  {
    if(String.IsNullOrWhiteSpace(listId))
      return UnprocessableEntity("ListID must exist");
    try 
    {
      await listService.RemoveList(listId,userId);
      return Ok("List deleted");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message + "Stack trace: "+e.StackTrace);
    }
  }

  [HttpPut("AddAlbumToList/{listId}/{albumId}/{description}")]
  public async Task<IActionResult> AddAlbumToList(string listId, string albumId,string description)
  {
    if(String.IsNullOrWhiteSpace(listId))
      return UnprocessableEntity("ListID must exist");
    if(String.IsNullOrWhiteSpace(albumId))
      return UnprocessableEntity("AlbumID must exist");
    try 
    {
      await listService.AddAlbumToList(listId,albumId,description);
      return Ok("Album added to list");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message + "Stack trace: "+e.StackTrace);
    }
  }
  [HttpGet("SearchList/{patern}")]
  public async Task<IActionResult> SearchList(string patern)
  {
    if(String.IsNullOrWhiteSpace(patern))
      return UnprocessableEntity("Pattern must exist");
    if(patern.Length <3)
      return UnprocessableEntity("Search querry must be 3 or more charachters");
    try
    {
      List<ListCard> searchResult = await listService.SearchList(patern);
      return Ok(searchResult);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
  [HttpPut("DeleteAlbumFromList/{listId}/{place}")]
  public async Task<IActionResult> DeleteAlbumFromList(string listId,int place)
  {
    if(String.IsNullOrWhiteSpace(listId))
      return UnprocessableEntity("ListID must exist");
    if(place <0)
      return UnprocessableEntity("Place must be 0 or greater!!");
    try
    {
      await listService.DeleteAlbumFromList(listId,place);
      return Ok("Deleted album from list");
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  } 
  [HttpGet("GetListById/{listId}")]
  public IActionResult GetListById(string listId)
  {
    if(String.IsNullOrWhiteSpace(listId))
      return UnprocessableEntity("ListID must exist");
    try
    {
      ListPage page =  listService.GetListById(listId);
      return Ok(page);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }

}