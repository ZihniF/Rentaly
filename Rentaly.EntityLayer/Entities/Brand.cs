using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ImageUrl { get; set; }

        public List<CarModel> CarModels { get; set; } = new();
        public List<Car> Cars { get; set; } = new();
    }
}
