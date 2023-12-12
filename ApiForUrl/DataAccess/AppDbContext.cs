using ApiForUrl.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiForUrl.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<ShortUrlModel> shortUrlModels {  get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
               .HasMany(i => i.ShortUrls)
               .WithOne(i => i.User)
               .HasForeignKey(i => i.UserId)
               .OnDelete(DeleteBehavior.ClientCascade);

        base.OnModelCreating(builder);
    }
}
