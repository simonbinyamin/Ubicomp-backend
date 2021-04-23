using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
    {
        EnsureDatabaseCreatedAsync();
    }
    public DbSet<UserModel> Users { get; set; }

    public void EnsureDatabaseCreatedAsync()
    {
        if (this.Database.EnsureCreated())
        {
            this.SaveChanges();
        }
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserModel>(entity =>
        {
            entity.HasKey(e => e.EmailAddress);
        });
    }
}