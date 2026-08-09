using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Car> Cars { get; set; } = new();
        public List<Rental> PickupRentals { get; set; } = new();
        public List<Rental> ReturnRentals { get; set; } = new();
    }
}
