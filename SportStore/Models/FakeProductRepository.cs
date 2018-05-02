//fake data used until DB is done

using System.Collections.Generic;

namespace SportStore.Models
{
    public class FakeProductRepository : IProductRepository
    {
        public IEnumerable<Product> Products => new List<Product>
        {
            new Product {Name = "Football", Price = 25},
            new Product {Name = "Shinai", Price = 179},
            new Product {Name = "Shoes", Price = 95},
        };
    }
}
