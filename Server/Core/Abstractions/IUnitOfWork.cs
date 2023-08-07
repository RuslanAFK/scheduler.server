namespace Server.Core.Abstractions;

public interface IUnitOfWork
{
    Task CompleteAsyncOrThrowIfNotCompleted();
}