using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /*
        ✓ Category
        ✓ Product code (should be a unique one e.g.: P01, P02, etc...)
        ✓ Name
        ✓ Image should be able to be saved in the local storage.
        ✓ Price
        ✓ Minimum Quantity
        ✓ Discount Rate
     */
    public class Product : BaseEntity
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public decimal DiscountRate { get; set; }
        public string Category { get; set; }
    }
}
