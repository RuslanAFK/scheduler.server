
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers.Resources;

public class LoginResource
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}