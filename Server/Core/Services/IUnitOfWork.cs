namespace Server.Core.Services;

public interface IUnitOfWork
{
    Task<int> CompleteAsync();
}