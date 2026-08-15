using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Rentaly.EntityLayer.Entities
{
    public class CarModel
    {
        public int CarModelId { get; set; }
        [Required(ErrorMessage = "Model adı zorunludur.")]
        public string ModelName { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Marka seçilmelidir.")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public List<Car> Cars { get; set; } = new();

    }
}
