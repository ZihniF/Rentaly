using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public List<CarModel> CarModels { get; set; } = new();
        public List<Car> Cars { get; set; } = new();
    }
}
