using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class KoiCareDBContext : DbContext
{
    public KoiCareDBContext() { }

    public KoiCareDBContext(DbContextOptions<KoiCareDBContext> options)
        : base(options)
    {
    }

    // DbSets for your entities
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Fish> Fishes { get; set; }
    public DbSet<FishProperties> FishProperties { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Pool> Pools { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Subcriptions> Subcriptions { get; set; }
    public DbSet<UserSubcriptions> UserSubcriptions { get; set; }
    public DbSet<WaterProperties> WaterProperties { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Waters> Waters { get; set; }  // Added Waters DbSet

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("KoiCareDB"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships for Blog entity
        modelBuilder.Entity<Blog>()
            .HasOne(b => b.Member)
            .WithMany(m => m.Blogs)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationships for Fish entity
        modelBuilder.Entity<Fish>()
            .HasOne(f => f.Pool)
            .WithMany(p => p.Fish)
            .HasForeignKey(f => f.PoolId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Fish>()
            .HasOne(f => f.Food)
            .WithMany(f => f.Fish)
            .HasForeignKey(f => f.FoodId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationships for FishProperties
        modelBuilder.Entity<FishProperties>()
            .HasOne(fp => fp.Fish)
            .WithMany(f => f.FishProperties)
            .HasForeignKey(fp => fp.FishId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure relationships for Member entity
        modelBuilder.Entity<Member>()
            .HasOne(m => m.Role)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationships for Order entity
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Member)
            .WithMany(m => m.Orders)
            .HasForeignKey(o => o.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationships for OrderProduct entity
        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Subcriptions)
            .WithMany()
            .HasForeignKey(op => op.SubcriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure relationships for Pool entity
        modelBuilder.Entity<Pool>()
            .HasOne(p => p.Member)
            .WithMany(m => m.Pools)
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pool>()
            .HasOne(p => p.Water)
            .WithMany(w => w.Pools)  // Many Pools can belong to one Water type
            .HasForeignKey(p => p.WaterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationships for UserSubcriptions entity
        modelBuilder.Entity<UserSubcriptions>()
        .HasOne(us => us.Member)
        .WithMany(m => m.UserSubcriptions)
        .HasForeignKey(us => us.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserSubcriptions>()
            .HasOne(us => us.Subcriptions)
            .WithMany(s => s.UserSubcriptions)
            .HasForeignKey(us => us.SubcriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure relationships for WaterProperties entity
        modelBuilder.Entity<WaterProperties>()
            .HasOne(wp => wp.Water)
            .WithMany(w => w.WaterProperties)  // Many WaterProperties to one Water type
            .HasForeignKey(wp => wp.WaterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure relationships for Food entity
        modelBuilder.Entity<Food>()
            .HasMany(f => f.Fish)
            .WithOne(f => f.Food)
            .HasForeignKey(f => f.FoodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
