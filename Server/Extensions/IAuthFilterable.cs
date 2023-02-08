using Server.Core.Models;

namespace Server.Extensions;

public interface IAuthFilterable
{
    public User? User { get; set; }
}