using System.Collections.ObjectModel;

namespace Server.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public ICollection<Subject> Subjects { get; set; }

    public User()
    {
        Subjects = new Collection<Subject>();
    }
}
