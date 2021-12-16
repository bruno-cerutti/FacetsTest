using System.Collections.Generic;

namespace FacetsTest.Models
{
    public class ProductOption
    {
        public int OptionId { get; set; }
        public bool Available { get; set; }
        public decimal Price { get; set; }
        public IList<ProductOptionAttribute> Attributes { get; set; }
    }
}