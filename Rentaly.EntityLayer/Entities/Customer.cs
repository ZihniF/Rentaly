using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public string DrivingLicenseNumber { get; set; } = string.Empty;
        public DateTime DrivingLicenseDate { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Rental> Rentals { get; set; } = new();

    }
}
