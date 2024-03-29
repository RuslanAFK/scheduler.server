namespace Server.Core.Models;

public class AuthResult
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    public AuthResult(User user, string token)
    {
        Id = user.Id;
        Username = user.Username;
        Token = token;
    }
}