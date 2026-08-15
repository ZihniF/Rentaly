using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Rentaly.DtoLayer.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public int CustomerId { get; set; }
        [Required, StringLength(60)] public string Name { get; set; } = string.Empty;
        [Required, StringLength(60)] public string Surname { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = string.Empty;
        [Required, Phone, StringLength(30)] public string Phone { get; set; } = string.Empty;
        [RegularExpression("^$|^[0-9]{11}$", ErrorMessage = "T.C. kimlik numarası 11 haneli olmalıdır.")]
        public string IdentityNumber { get; set; } = string.Empty;
        [StringLength(30)]
        public string DrivingLicenseNumber { get; set; } = string.Empty;
        public DateTime DrivingLicenseDate { get; set; }
    }
}
