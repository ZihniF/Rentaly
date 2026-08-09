using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class Car
    {
        public int CarId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty; // Şasi No
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public int ModelId { get; set; }
        public CarModel Model { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        public int Year { get; set; }
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int SeatCount { get; set; }
        public int LuggageCount { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public List<Rental> Rentals { get; set; } = new();


    }
}
