using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Models.List
{
  public class List 
  {
    public ObjectId Id { get; set; }
    public ObjectId UserId { get; set; }
    public string? Username {get;set;}
    public string? Title { get; set; }
    [MaxLength(5000)]
    public string? Description { get; set; }
    public string? ListImageURL { get; set; }
    public List<ListItem>? ListItems { get; set; }
  }
}