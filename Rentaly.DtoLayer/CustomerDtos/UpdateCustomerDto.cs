using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DtoLayer.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public string DrivingLicenseNumber { get; set; } = string.Empty;
        public DateTime DrivingLicenseDate { get; set; }
    }
}
