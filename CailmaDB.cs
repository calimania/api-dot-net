using Microsoft.EntityFrameworkCore;

class CalimaDB : DbContext
{
    public CalimaDB(DbContextOptions<CalimaDB> options)
        : base(options) { }

    public DbSet<Calima> Calimas => Set<Calima>();
}
