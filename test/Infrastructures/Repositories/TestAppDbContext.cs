using csharp_training_202605.Infrastructures.Context;
using Microsoft.EntityFrameworkCore;

namespace csharp_training_202605.tests.TestDoubles;

internal sealed class TestAppDbContext : AppDbContext
{
    public TestAppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public override int SaveChanges()
    {
        return 0;
    }
}
