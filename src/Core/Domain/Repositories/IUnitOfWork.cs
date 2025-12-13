namespace Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Saves all changes made in this unit of work to the underlying database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> CompleteAsync();
    int Complete();
}