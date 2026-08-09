using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class CarModel
    {
        public int CarModelId { get; set; }
        public string ModelName { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public List<Car> Cars { get; set; } = new();

    }
}
