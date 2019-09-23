using GFS.Data.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Data.EFCore
{
    public class GfsDbContext : DbContext
    {
        public GfsDbContext(DbContextOptions<GfsDbContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<TriangularFormatter> TriangularFormatters { get; set; }
        public DbSet<RectangularFormatter> RectangularFormatters { get; set; }
        public DbSet<PentagonFormatter> PentagonFormatters { get; set; }
        public DbSet<LFormatter> LFormatters { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Fabric> Fabrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildOrderMapping(modelBuilder.Entity<Order>());
            BuildTriangularFormatterMapping(modelBuilder.Entity<TriangularFormatter>());
            BuildRectangularFormatterMapping(modelBuilder.Entity<RectangularFormatter>());
            BuildPentagonFormatterMapping(modelBuilder.Entity<PentagonFormatter>());
            BuildLFormatterMapping(modelBuilder.Entity<LFormatter>());
            BuildFurnitureMapping(modelBuilder.Entity<Furniture>());
            BuildFabricMapping(modelBuilder.Entity<Fabric>());

        }
      

        private static void BuildOrderMapping(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasOne(o=> o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            builder.HasKey(o => o.Id);
           
        }
        private static void BuildFurnitureMapping(EntityTypeBuilder<Furniture> builder)
        {
            builder
                .HasOne(f => f.Order)
                .WithMany(o => o.Furnitures)
                .HasForeignKey(f => f.OrderId);
            builder
                .HasKey(f => f.Id);
        }
        private static void BuildTriangularFormatterMapping(EntityTypeBuilder<TriangularFormatter> builder)
        {
            builder
                .HasOne(f => f.Furniture)
                .WithMany(o => o.TriangularFormatters)
                .HasForeignKey(f => f.FurnitureId);
            builder
                .HasOne(f => f.Fabric)
                .WithMany(t => t.TriangularFormatters)
                .HasForeignKey(f => f.FabricId);
             
            builder
                .HasKey(f => f.Id);
        }
        private static void BuildRectangularFormatterMapping(EntityTypeBuilder<RectangularFormatter> builder)
        {
            builder
                .HasOne(f => f.Furniture)
                .WithMany(o => o.RectangularFormatters)
                .HasForeignKey(f => f.FurnitureId);
            builder
                .HasOne(f => f.Fabric)
                .WithMany(t => t.RectangularFormatters)
                .HasForeignKey(f => f.FabricId);
            builder
                .HasKey(f => f.Id);
        }
        private static void BuildPentagonFormatterMapping(EntityTypeBuilder<PentagonFormatter> builder)
        {
            builder
                .HasOne(f => f.Furniture)
                .WithMany(o => o.PentagonFormatters)
                .HasForeignKey(f => f.FurnitureId);
            builder
                .HasOne(f => f.Fabric)
                .WithMany(t => t.PentagonFormatters)
                .HasForeignKey(f => f.FabricId);
            builder
                .HasKey(f => f.Id);
        }
        private static void BuildLFormatterMapping(EntityTypeBuilder<LFormatter> builder)
        {
            builder
                .HasOne(f => f.Furniture)
                .WithMany(o => o.LFormatters)
                .HasForeignKey(f => f.FurnitureId);
            builder
                .HasOne(f => f.Fabric)
                .WithMany(t => t.LFormatters)
                .HasForeignKey(f => f.FabricId);
            builder
                .HasKey(f => f.Id);
        }
        private static void BuildFabricMapping(EntityTypeBuilder<Fabric> builder)
        {
            builder
                .HasOne(f => f.Producer)
                .WithMany(p => p.Fabrics)
                .HasForeignKey(f => f.ProducerId);
            builder
                .HasKey(f => f.Id);
        }

    }
}