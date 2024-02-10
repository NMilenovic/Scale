namespace Models.List
{
  public class ListPage
  {
    public string? Id { get; set; }
    public string? OwnerId { get; set; }
    public string? OwnerUsername { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ListImageURL { get; set; }
    public List<ListItem>? ListItems { get; set; }
  }
}