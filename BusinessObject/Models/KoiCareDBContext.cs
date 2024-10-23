using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Models
{
    public partial class KoiCareDBContext : DbContext
    {
        public KoiCareDBContext() { }

        public KoiCareDBContext(DbContextOptions<KoiCareDBContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<Fish> Fishes { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Pool> Pools { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Waters> Waters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .SetBasePath(Directory.GetCurrentDirectory()).Build();

            var connectionString = configuration.GetSection("ConnectionStrings:KoiCareDB");

            optionsBuilder.UseSqlServer(connectionString.Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.DateOfPublish).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Blogs_MemberId");
            });

            modelBuilder.Entity<Fish>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Origin).HasMaxLength(100);

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.Fish)
                    .HasForeignKey(d => d.FoodId)
                    .HasConstraintName("FK_Fishes_FoodId");

                entity.HasOne(d => d.Pool)
                    .WithMany(p => p.Fish)
                    .HasForeignKey(d => d.PoolId)
                    .HasConstraintName("FK_Fishes_PoolId");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Members_RoleId");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CloseDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue(string.Empty);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValue(string.Empty);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue(string.Empty);

                entity.Property(e => e.TotalCost)
                    .IsRequired()
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_MemberId");

               
                entity.HasMany(e => e.OrderProducts) 
                    .WithOne(op => op.Order)
                    .HasForeignKey(op => op.OrderId) 
                    .OnDelete(DeleteBehavior.Cascade) 
                    .HasConstraintName("FK_OrderProducts_Orders");

                modelBuilder.Entity<OrderProduct>()
                    .HasOne(op => op.Product) 
                    .WithMany(p => p.OrderProducts) 
                    .HasForeignKey(op => op.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderProducts_Products"); 
            });

            modelBuilder.Entity<Pool>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Pools)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Pools_MemberId");

                entity.HasOne(d => d.Water)
                    .WithMany(p => p.Pools)
                    .HasForeignKey(d => d.WaterId)
                    .HasConstraintName("FK_Pools_WaterId");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsUnicode(false); // Đảm bảo rằng ID là không phải Unicode nếu cần thiết

                entity.Property(e => e.Code)
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasMaxLength(100);

                entity.Property(e => e.Origin)
                    .HasMaxLength(100);

                entity.Property(e => e.Cost)
                    .IsRequired()
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InStock)
                    .IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Products_UserId");

                entity.HasMany(e => e.OrderProducts)
                    .WithOne(op => op.Product)
                    .HasForeignKey(op => op.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderProducts_Products");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Waters>(entity =>
            {
                entity.Property(e => e.Id).IsUnicode(false);

                entity.Property(e => e.Temperature).HasColumnName("Temperature");

                entity.Property(e => e.No2).HasColumnName("NO2");

                entity.Property(e => e.No3).HasColumnName("NO3");

                entity.Property(e => e.Po4).HasColumnName("PO4");

                entity.Property(e => e.Salt).HasColumnName("Salt");

                entity.Property(e => e.Ph).HasColumnName("Ph");

                entity.Property(e => e.O2).HasColumnName("O2");
            });

            modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
