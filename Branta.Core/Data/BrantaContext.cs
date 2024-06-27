using Branta.Core.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Branta.Core.Data;

public class BrantaContext : DbContext
{
    public virtual DbSet<ExtendedKey> ExtendedKey { get; set; }

    public BrantaContext()
    {
    }

    public BrantaContext(DbContextOptions<BrantaContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\Temp\BrantaCore.db");
        }
    }
}
