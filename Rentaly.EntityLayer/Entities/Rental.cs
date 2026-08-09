using System;
using System.Collections.Generic;
using System.Text;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.EntityLayer.Entities
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public int PickupBranchId { get; set; }
        public Branch PickupBranch { get; set; } = null!;
        public int ReturnBranchId { get; set; }
        public Branch ReturnBranch { get; set; } = null!;
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public RentalStatus Status { get; set; } = RentalStatus.Pending;

    }
}
