using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Rentaly.EntityLayer.Entities
{
    public class Branch
    {
        public int BranchId { get; set; }
        [Required(ErrorMessage = "Şube adı zorunludur.")]
        public string BranchName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Şehir zorunludur.")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "Adres zorunludur.")]
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<Car> Cars { get; set; } = new();
        public List<Rental> PickupRentals { get; set; } = new();
        public List<Rental> ReturnRentals { get; set; } = new();
    }
}
