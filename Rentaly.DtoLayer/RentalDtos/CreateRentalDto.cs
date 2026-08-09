using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DtoLayer.RentalDtos
{
    public class CreateRentalDto
    {
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int PickupBranchId { get; set; }
        public int ReturnBranchId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
