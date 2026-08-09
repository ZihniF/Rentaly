using Rentaly.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DtoLayer.RentalDtos
{
    public class ResultRentalDto
    {
        public int RentalId { get; set; }

        public int CarId { get; set; }
        public string PlateNumber { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;

        public int CustomerId { get; set; }
        public string CustomerFullName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;

        public string PickupBranchName { get; set; } = null!;
        public string ReturnBranchName { get; set; } = null!;

        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public RentalStatus Status { get; set; }
    }
}
