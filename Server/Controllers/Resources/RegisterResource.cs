using System.ComponentModel.DataAnnotations;

namespace Server.Controllers.Resources;

public class RegisterResource
{
    [Required, MaxLength(16)]
    public string Username { get; set; }
    [Required, MaxLength(16)]
    public string Password { get; set; }
}