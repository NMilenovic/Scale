using System.ComponentModel.DataAnnotations;

namespace Models{
  public class UserLoginDTO{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MaxLength(30)]
    [MinLength(6)]
    public string? Password { get; set; }
  }
}