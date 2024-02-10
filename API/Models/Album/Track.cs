using System.ComponentModel.DataAnnotations;

namespace Models
{
  public class Track
  {
    [Required]
    public uint TrackNumber { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public int Minutes { get; set; }
    [Required]
    public int Seconds { get; set; }

  }
}