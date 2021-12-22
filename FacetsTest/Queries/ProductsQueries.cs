using System.Collections.Generic;
using System.Threading.Tasks;
using FacetsTest.Models;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Queries.Facets;

namespace FacetsTest.Queries
{
    public class ProductsQueries
    {
        public async Task<IList<Product>> GetProducts(string indexName, bool? available, IDictionary<string, IList<string>> filters)
        {
            using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                var query = session.Advanced.AsyncDocumentQuery<Product>(indexName).UsingDefaultOperator(QueryOperator.And).Statistics(out var stats);
                if (available.HasValue)
                    query.WhereLucene("Option_Available", available.ToString());

                foreach (var filter in filters)
                {
                    query.WhereLucene(filter.Key, $"({string.Join(" OR ", filter.Value)})");
                }

                return await query.ToListAsync();
            }
        }

        public async Task<IDictionary<string, FacetResult>> GetProductsFacets(string indexName, bool? available, IDictionary<string, IList<string>> filters)
        {
            using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                var query = session.Advanced.AsyncDocumentQuery<Product>(indexName).UsingDefaultOperator(QueryOperator.And).Statistics(out var stats);
                if (available.HasValue)
                    query.WhereLucene("Option_Available", available.ToString());

                foreach (var filter in filters)
                {
                    query.WhereLucene(filter.Key, $"({string.Join(" OR ", filter.Value)})");
                }

                return await query.AggregateBy(GetFacets()).ExecuteAsync();
            }
        }

        private IEnumerable<Facet> GetFacets()
        {
            var facets = new List<Facet>();

            facets.Add(new Facet
            {
                FieldName = "Brand",
                DisplayFieldName = "Brand"
            });
            facets.Add(new Facet
            {
                FieldName = "Option_Available",
                DisplayFieldName = "Availability"
            });
            facets.Add(new Facet
            {
                FieldName = "Size",
                DisplayFieldName = "Size"
            });
            facets.Add(new Facet
            {
                FieldName = "Color",
                DisplayFieldName = "Color"
            });

            return facets;
        }

        public async Task InitializeCollection()
        {
            var product1 = new Product
            {
                Id = "Products/Nike-shoes-abc",
                Title = "Nike shoes abc",
                Brand = "Nike",
                Options = new List<ProductOption>
                {
                    new ProductOption
                    {
                        OptionId = 1,
                        Price = 15,
                        Available = false,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "9" },
                            new ProductOptionAttribute { Name = "Color", Value = "Red" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 2,
                        Price = 15,
                        Available = true,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "10" },
                            new ProductOptionAttribute { Name = "Color", Value = "Red" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 3,
                        Price = 15,
                        Available = false,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "11" },
                            new ProductOptionAttribute { Name = "Color", Value = "Red" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 4,
                        Price = 17,
                        Available = false,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "9" },
                            new ProductOptionAttribute { Name = "Color", Value = "Black" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 5,
                        Price = 17,
                        Available = true,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "10" },
                            new ProductOptionAttribute { Name = "Color", Value = "Black" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 6,
                        Price = 17,
                        Available = false,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "11" },
                            new ProductOptionAttribute { Name = "Color", Value = "Black" }
                        }
                    }
                }
            };

            var product2 = new Product
            {
                Id = "Products/Adidas-shoes-def",
                Title = "Adidas shoes def",
                Brand = "Adidas",
                Options = new List<ProductOption>
                {
                    new ProductOption
                    {
                        OptionId = 7,
                        Price = 12,
                        Available = true,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "9" },
                            new ProductOptionAttribute { Name = "Color", Value = "Red" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 8,
                        Price = 12,
                        Available = false,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "11" },
                            new ProductOptionAttribute { Name = "Color", Value = "Red" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 9,
                        Price = 12,
                        Available = true,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "9" },
                            new ProductOptionAttribute { Name = "Color", Value = "Yellow" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 11,
                        Price = 15,
                        Available = true,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "10" },
                            new ProductOptionAttribute { Name = "Color", Value = "Yellow" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 12,
                        Price = 15,
                        Available = false,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "11" },
                            new ProductOptionAttribute { Name = "Color", Value = "Yellow" }
                        }
                    },
                    new ProductOption
                    {
                        OptionId = 12,
                        Price = 18,
                        Available = true,
                        Attributes = new List<ProductOptionAttribute>
                        {
                            new ProductOptionAttribute { Name = "Size", Value = "12" },
                            new ProductOptionAttribute { Name = "Color", Value = "Yellow" }
                        }
                    }
                }
            };

            using (var session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                await session.StoreAsync(product1);
                await session.StoreAsync(product2);
                await session.SaveChangesAsync();
            }
        }
    }
}