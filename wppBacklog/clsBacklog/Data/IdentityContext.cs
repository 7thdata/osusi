using clsBacklog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace clsBacklog.Data;

public class IdentityContext : IdentityDbContext<UserModel>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }


    // Org.
    public DbSet<OrganizationModel> Organizations { get; set; }
    public DbSet<OrganizationMemberModel> OrganizationMembers { get; set; }

    // Subscription
    public DbSet<SubscriptionModel> Subscriptions { get; set; }
    public DbSet<SubscriptionLogModel> SubscriptionLogs { get; set; }
}
