using Microsoft.EntityFrameworkCore;
using Rentaly.EntityLayer.Entities;


namespace Rentaly.DataAccessLayer.Concrete
{
    public class RentalyContext : DbContext
    {
        public RentalyContext(DbContextOptions<RentalyContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CarModel>()
                .HasOne(x => x.Brand)
                .WithMany(x => x.CarModels)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
            // Car alan ayarları
            modelBuilder.Entity<Car>()
                .Property(x => x.PlateNumber)
                .HasMaxLength(20);

            modelBuilder.Entity<Car>()
                .Property(x => x.VIN)
                .HasMaxLength(17);

            modelBuilder.Entity<Car>()
                .Property(x => x.DailyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Car>()
                .Property(x => x.DepositAmount)
                .HasPrecision(18, 2);

            // Plaka ve şasi numarası benzersiz olmalı
            modelBuilder.Entity<Car>()
                .HasIndex(x => x.PlateNumber)
                .IsUnique();

            modelBuilder.Entity<Car>()
                .HasIndex(x => x.VIN)
                .IsUnique();

            // Car - Brand ilişkisi
            modelBuilder.Entity<Car>()
                .HasOne(x => x.Brand)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Car - CarModel ilişkisi
            modelBuilder.Entity<Car>()
                .HasOne(x => x.Model)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Car - Category ilişkisi
            modelBuilder.Entity<Car>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Car - Branch ilişkisi
            modelBuilder.Entity<Car>()
                .HasOne(x => x.Branch)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rental - Car ilişkisi
            modelBuilder.Entity<Rental>()
                .HasOne(x => x.Car)
                .WithMany(x => x.Rentals)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rental - Customer ilişkisi
            modelBuilder.Entity<Rental>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Rentals)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rental - PickupBranch ilişkisi
            modelBuilder.Entity<Rental>()
                .HasOne(x => x.PickupBranch)
                .WithMany(x => x.PickupRentals)
                .HasForeignKey(x => x.PickupBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rental - ReturnBranch ilişkisi
            modelBuilder.Entity<Rental>()
                .HasOne(x => x.ReturnBranch)
                .WithMany(x => x.ReturnRentals)
                .HasForeignKey(x => x.ReturnBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rental durumunu SQL'de metin olarak sakla
            modelBuilder.Entity<Rental>()
                .Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Toplam fiyat hassasiyeti
            modelBuilder.Entity<Rental>()
                .Property(x => x.TotalPrice)
                .HasPrecision(18, 2);

            // Araç müsaitlik sorgusu için indeks
            modelBuilder.Entity<Rental>()
                .HasIndex(x => new
                {
                    x.CarId,
                    x.Status,
                    x.PickupDate,
                    x.ReturnDate
                })
                .HasDatabaseName("IX_Rentals_Availability");

            modelBuilder.Entity<HomeContent>()
                .Property(x => x.Section)
                .HasConversion<string>()
                .HasMaxLength(30);
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<HomeContent> HomeContents { get; set; }
    }
}
