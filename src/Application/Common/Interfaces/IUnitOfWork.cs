namespace Application.Common.Interfaces;

/// <summary>
/// Abstracts saving all changes within a single transaction.
/// Use this in command handlers after performing multiple repository operations.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
