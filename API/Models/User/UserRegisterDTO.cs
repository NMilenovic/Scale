using System.ComponentModel.DataAnnotations;

namespace Models.User
{
  public enum UserRole
  {
    User,
    Admin
  }

  public class UserRegisterDTO
  {
    [Required]
    [MaxLength(30)]
    public string? Username { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    [MaxLength(30)]
    [MinLength(6)]
    public string? Password1 { get; set; }
    [Required]
    [MaxLength(30)]
    [MinLength(6)]
    public string? Password2 { get; set; }
    [Required]
    public UserRole Role {get;set;}

  }
}