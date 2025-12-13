using Domain.Repositories;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Persistence.Implementation;

public class UnitOfWork(AuthDbContext context) : IUnitOfWork
{

    public async Task<int> CompleteAsync() => await context.SaveChangesAsync();
    public int Complete() => context.SaveChanges();

    public void Dispose() => context.Dispose();
}
