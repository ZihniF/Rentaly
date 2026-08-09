using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // SUV, Sedan, Hatchback vs
        public List<Car> Cars { get; set; }

    }
}
