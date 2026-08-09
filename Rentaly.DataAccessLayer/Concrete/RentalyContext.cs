using Microsoft.EntityFrameworkCore;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class RentalyContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .HasOne(x => x.Brand)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(x => x.Model)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(x => x.Branch)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(x => x.Car)
                .WithMany(x => x.Rentals)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Rentals)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(x => x.PickupBranch)
                .WithMany(x => x.PickupRentals)
                .HasForeignKey(x => x.PickupBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(x => x.ReturnBranch)
                .WithMany(x => x.ReturnRentals)
                .HasForeignKey(x => x.ReturnBranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-GJQKLK1;Database=RentalyDb;Trusted_Connection=True; TrustServerCertificate=True;");
        }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
    }
}
