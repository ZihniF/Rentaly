using Rentaly.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DtoLayer.RentalDtos
{
    public class UpdateRentalStatusDto
    {
        public int RentalId { get; set; }
        public RentalStatus Status { get; set; }
    }
}
